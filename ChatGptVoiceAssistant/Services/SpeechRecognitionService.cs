using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;

namespace HeyGPT.Services
{
    public class SpeechRecognitionService : IDisposable
    {
        private SpeechRecognitionEngine? _recognizer;
        private SpeechRecognitionEngine? _commandRecognizer;
        private string _currentWakeWord = "Hey GPT";
        private float _confidenceThreshold = 0.7f;

        private int _silenceThreshold = 10;
        private int _minimumSilenceDurationMs = 500;
        private int _cooldownPeriodMs = 1500;
        private bool _enableIsolation = true;

        private DateTime _lastActivationTime = DateTime.MinValue;
        private DateTime _lastSilenceStartTime = DateTime.Now;
        private DateTime _lastSpeechTime = DateTime.Now;
        private DateTime _firstHypothesisTime = DateTime.MinValue;
        private DateTime _lastRecognitionTime = DateTime.MinValue;
        private bool _isCurrentlySilent = true;
        private int _currentAudioLevel = 0;
        private bool _waitingForSilenceConfirmation = false;
        private readonly List<string> _recentHypotheses = new List<string>();
        private readonly List<string> _postRecognitionHypotheses = new List<string>();
        private readonly object _lockObject = new object();

        private bool _isInVoiceMode = false;
        private bool _isCommandRecognitionEnabled = false;

        public event EventHandler<string>? WakeWordDetected;
        public event EventHandler<string>? StatusChanged;
        public event EventHandler<string>? ErrorOccurred;
        public event EventHandler<string>? CommandRecognized;

        public bool IsListening { get; private set; }
        public bool IsInVoiceMode
        {
            get => _isInVoiceMode;
            set
            {
                if (_isInVoiceMode != value)
                {
                    _isInVoiceMode = value;
                    if (_isInVoiceMode)
                    {
                        EnableCommandRecognition();
                    }
                    else
                    {
                        DisableCommandRecognition();
                    }
                }
            }
        }

        public void Initialize(string wakeWord, float confidenceThreshold)
        {
            _currentWakeWord = wakeWord;
            _confidenceThreshold = confidenceThreshold;

            try
            {
                _recognizer?.Dispose();
                _recognizer = new SpeechRecognitionEngine(new CultureInfo("en-US"));

                Choices wakeWordChoices = new Choices(_currentWakeWord);
                GrammarBuilder grammarBuilder = new GrammarBuilder(wakeWordChoices);
                Grammar grammar = new Grammar(grammarBuilder);

                _recognizer.LoadGrammar(grammar);

                _recognizer.SpeechRecognized += OnSpeechRecognized;
                _recognizer.SpeechRecognitionRejected += OnSpeechRejected;
                _recognizer.RecognizerUpdateReached += OnRecognizerUpdateReached;
                _recognizer.AudioLevelUpdated += OnAudioLevelUpdated;
                _recognizer.SpeechHypothesized += OnSpeechHypothesized;

                _recognizer.SetInputToDefaultAudioDevice();

                ResetIsolationState();

                StatusChanged?.Invoke(this, "Speech recognition initialized with advanced isolation");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Initialization error: {ex.Message}");
                throw;
            }
        }

        public void ConfigureIsolation(bool enableIsolation, int silenceThreshold, int minimumSilenceDurationMs, int cooldownPeriodMs)
        {
            _enableIsolation = enableIsolation;
            _silenceThreshold = silenceThreshold;
            _minimumSilenceDurationMs = minimumSilenceDurationMs;
            _cooldownPeriodMs = cooldownPeriodMs;

            StatusChanged?.Invoke(this, $"Isolation configured: Enabled={enableIsolation}, SilenceThreshold={silenceThreshold}, MinSilence={minimumSilenceDurationMs}ms, Cooldown={cooldownPeriodMs}ms");
        }

        private void ResetIsolationState()
        {
            lock (_lockObject)
            {
                _lastSilenceStartTime = DateTime.Now;
                _lastSpeechTime = DateTime.Now;
                _firstHypothesisTime = DateTime.MinValue;
                _lastRecognitionTime = DateTime.MinValue;
                _isCurrentlySilent = true;
                _currentAudioLevel = 0;
                _waitingForSilenceConfirmation = false;
                _recentHypotheses.Clear();
                _postRecognitionHypotheses.Clear();
            }
        }

        public void StartListening()
        {
            try
            {
                if (_recognizer == null)
                {
                    throw new InvalidOperationException("Speech recognizer not initialized. Call Initialize first.");
                }

                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
                IsListening = true;
                StatusChanged?.Invoke(this, $"Listening for wake word: '{_currentWakeWord}'");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error starting listener: {ex.Message}");
                throw;
            }
        }

        public void StopListening()
        {
            try
            {
                if (_recognizer != null && IsListening)
                {
                    _recognizer.RecognizeAsyncStop();
                    IsListening = false;
                    StatusChanged?.Invoke(this, "Stopped listening");
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error stopping listener: {ex.Message}");
            }
        }

        public void UpdateWakeWord(string newWakeWord, float confidenceThreshold)
        {
            bool wasListening = IsListening;

            if (wasListening)
            {
                StopListening();
            }

            Initialize(newWakeWord, confidenceThreshold);

            if (wasListening)
            {
                StartListening();
            }
        }

        public void InitializeCommands()
        {
            try
            {
                _commandRecognizer?.Dispose();
                _commandRecognizer = new SpeechRecognitionEngine(new CultureInfo("en-US"));

                Choices commandChoices = new Choices("mic on", "mic off", "exit", "exit voice mode");
                GrammarBuilder commandGrammarBuilder = new GrammarBuilder(commandChoices);
                Grammar commandGrammar = new Grammar(commandGrammarBuilder);

                _commandRecognizer.LoadGrammar(commandGrammar);

                _commandRecognizer.SpeechRecognized += OnCommandRecognized;
                _commandRecognizer.SpeechRecognitionRejected += OnCommandRejected;

                _commandRecognizer.SetInputToDefaultAudioDevice();

                StatusChanged?.Invoke(this, "Voice commands initialized");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Command initialization error: {ex.Message}");
                throw;
            }
        }

        public void EnableCommandRecognition()
        {
            try
            {
                if (_commandRecognizer == null)
                {
                    InitializeCommands();
                }

                if (_commandRecognizer != null && !_isCommandRecognitionEnabled)
                {
                    _commandRecognizer.RecognizeAsync(RecognizeMode.Multiple);
                    _isCommandRecognitionEnabled = true;
                    StatusChanged?.Invoke(this, "Voice commands ENABLED");
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error enabling commands: {ex.Message}");
            }
        }

        public void DisableCommandRecognition()
        {
            try
            {
                if (_commandRecognizer != null && _isCommandRecognitionEnabled)
                {
                    _commandRecognizer.RecognizeAsyncStop();
                    _isCommandRecognitionEnabled = false;
                    StatusChanged?.Invoke(this, "Voice commands DISABLED");
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error disabling commands: {ex.Message}");
            }
        }

        private void OnCommandRecognized(object? sender, SpeechRecognizedEventArgs e)
        {
            if (!_isInVoiceMode)
            {
                StatusChanged?.Invoke(this, $"Command ignored (not in voice mode): '{e.Result.Text}'");
                return;
            }

            if (e.Result.Confidence < 0.5f)
            {
                StatusChanged?.Invoke(this, $"Low confidence command: '{e.Result.Text}' ({e.Result.Confidence:P0})");
                return;
            }

            string command = e.Result.Text.ToLowerInvariant();
            StatusChanged?.Invoke(this, $"✓ Voice command detected: '{command}' (confidence: {e.Result.Confidence:P0})");
            CommandRecognized?.Invoke(this, command);
        }

        private void OnCommandRejected(object? sender, SpeechRecognitionRejectedEventArgs e)
        {
        }

        private void OnAudioLevelUpdated(object? sender, AudioLevelUpdatedEventArgs e)
        {
            lock (_lockObject)
            {
                _currentAudioLevel = e.AudioLevel;

                bool wasSilent = _isCurrentlySilent;
                _isCurrentlySilent = e.AudioLevel <= _silenceThreshold;

                if (!wasSilent && _isCurrentlySilent)
                {
                    _lastSilenceStartTime = DateTime.Now;

                    if (!_waitingForSilenceConfirmation && _recentHypotheses.Count > 0 && (DateTime.Now - _firstHypothesisTime).TotalMilliseconds > 3000)
                    {
                        _recentHypotheses.Clear();
                        _firstHypothesisTime = DateTime.MinValue;
                    }
                }
                else if (wasSilent && !_isCurrentlySilent)
                {
                    _lastSpeechTime = DateTime.Now;

                    if (!_waitingForSilenceConfirmation && _postRecognitionHypotheses.Count > 0)
                    {
                        _postRecognitionHypotheses.Clear();
                    }
                }
            }
        }

        private void OnSpeechHypothesized(object? sender, SpeechHypothesizedEventArgs e)
        {
            lock (_lockObject)
            {
                if (_waitingForSilenceConfirmation)
                {
                    _postRecognitionHypotheses.Add(e.Result.Text);
                    StatusChanged?.Invoke(this, $"⚠ Post-recognition speech detected: '{e.Result.Text}'");
                    return;
                }

                if (_recentHypotheses.Count == 0)
                {
                    _firstHypothesisTime = DateTime.Now;
                }

                _recentHypotheses.Add(e.Result.Text);

                if (_recentHypotheses.Count > 20)
                {
                    _recentHypotheses.RemoveAt(0);
                }
            }
        }

        private async void OnSpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < _confidenceThreshold)
            {
                StatusChanged?.Invoke(this, $"Low confidence recognition: '{e.Result.Text}' ({e.Result.Confidence:P0})");
                return;
            }

            string recognizedText = e.Result.Text;

            if (!_enableIsolation)
            {
                StatusChanged?.Invoke(this, $"Wake word detected (isolation disabled): '{recognizedText}' (confidence: {e.Result.Confidence:P0})");
                WakeWordDetected?.Invoke(this, recognizedText);
                return;
            }

            if (!PassesIsolationChecks(recognizedText, e.Result.Confidence))
            {
                return;
            }

            lock (_lockObject)
            {
                _lastRecognitionTime = DateTime.Now;
                _waitingForSilenceConfirmation = true;
                _postRecognitionHypotheses.Clear();
            }

            StatusChanged?.Invoke(this, $"Wake word recognized - waiting for silence confirmation...");

            await System.Threading.Tasks.Task.Delay(400);

            lock (_lockObject)
            {
                _waitingForSilenceConfirmation = false;

                if (_postRecognitionHypotheses.Count > 0)
                {
                    StatusChanged?.Invoke(this, $"⚠ Rejected: Speech continued after wake word ({_postRecognitionHypotheses.Count} post-recognition hypotheses)");
                    _recentHypotheses.Clear();
                    _postRecognitionHypotheses.Clear();
                    _firstHypothesisTime = DateTime.MinValue;
                    return;
                }

                var timeSinceSilence = (DateTime.Now - _lastSilenceStartTime).TotalMilliseconds;
                if (!_isCurrentlySilent && timeSinceSilence > 500)
                {
                    StatusChanged?.Invoke(this, $"⚠ Rejected: No silence after wake word (still hearing speech)");
                    _recentHypotheses.Clear();
                    _firstHypothesisTime = DateTime.MinValue;
                    return;
                }

                _lastActivationTime = DateTime.Now;
                _recentHypotheses.Clear();
                _firstHypothesisTime = DateTime.MinValue;
            }

            StatusChanged?.Invoke(this, $"✓ Wake word CONFIRMED (isolated): '{recognizedText}' (confidence: {e.Result.Confidence:P0})");
            WakeWordDetected?.Invoke(this, recognizedText);
        }

        private bool PassesIsolationChecks(string recognizedText, float confidence)
        {
            lock (_lockObject)
            {
                var now = DateTime.Now;
                var timeSinceLastActivation = (now - _lastActivationTime).TotalMilliseconds;

                if (timeSinceLastActivation < _cooldownPeriodMs)
                {
                    StatusChanged?.Invoke(this, $"⚠ Rejected: Cooldown ({(int)(_cooldownPeriodMs - timeSinceLastActivation)}ms remaining)");
                    return false;
                }

                var hypothesisDurationMs = _firstHypothesisTime == DateTime.MinValue ? 0 : (now - _firstHypothesisTime).TotalMilliseconds;
                if (hypothesisDurationMs > 2000)
                {
                    StatusChanged?.Invoke(this, $"⚠ Rejected: Speech duration too long ({(int)hypothesisDurationMs}ms) - part of conversation");
                    return false;
                }

                if (_recentHypotheses.Count > 5)
                {
                    StatusChanged?.Invoke(this, $"⚠ Rejected: Too many hypotheses ({_recentHypotheses.Count}/5) - embedded in sentence");
                    return false;
                }

                var allHypothesisWords = _recentHypotheses
                    .SelectMany(h => h.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    .Select(w => w.ToLowerInvariant())
                    .Distinct()
                    .ToList();

                if (allHypothesisWords.Count > 3)
                {
                    StatusChanged?.Invoke(this, $"⚠ Rejected: Too many unique words ({allHypothesisWords.Count}/3) - likely part of phrase");
                    return false;
                }

                var wakeWordWords = _currentWakeWord.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(w => w.ToLowerInvariant())
                    .ToHashSet();

                var nonWakeWordWords = allHypothesisWords.Where(w => !wakeWordWords.Contains(w)).ToList();
                if (nonWakeWordWords.Count > 0)
                {
                    StatusChanged?.Invoke(this, $"⚠ Rejected: Non-wake-word detected: '{string.Join(", ", nonWakeWordWords)}' - embedded in sentence");
                    return false;
                }

                StatusChanged?.Invoke(this, $"✓ Isolation PASSED: {_recentHypotheses.Count} hypotheses, {allHypothesisWords.Count} unique words, {(int)hypothesisDurationMs}ms duration");
                return true;
            }
        }


        private void OnSpeechRejected(object? sender, SpeechRecognitionRejectedEventArgs e)
        {
            StatusChanged?.Invoke(this, "Speech rejected - no match");
        }

        private void OnRecognizerUpdateReached(object? sender, RecognizerUpdateReachedEventArgs e)
        {
            StatusChanged?.Invoke(this, "Recognizer updated");
        }

        public void Dispose()
        {
            StopListening();
            DisableCommandRecognition();

            if (_recognizer != null)
            {
                _recognizer.SpeechRecognized -= OnSpeechRecognized;
                _recognizer.SpeechRecognitionRejected -= OnSpeechRejected;
                _recognizer.RecognizerUpdateReached -= OnRecognizerUpdateReached;
                _recognizer.AudioLevelUpdated -= OnAudioLevelUpdated;
                _recognizer.SpeechHypothesized -= OnSpeechHypothesized;
                _recognizer.Dispose();
                _recognizer = null;
            }

            if (_commandRecognizer != null)
            {
                _commandRecognizer.SpeechRecognized -= OnCommandRecognized;
                _commandRecognizer.SpeechRecognitionRejected -= OnCommandRejected;
                _commandRecognizer.Dispose();
                _commandRecognizer = null;
            }
        }
    }
}
