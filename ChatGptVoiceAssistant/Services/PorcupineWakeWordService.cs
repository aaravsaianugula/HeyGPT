using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using Pv;

namespace HeyGPT.Services
{
    public class PorcupineWakeWordService : IDisposable
    {
        private Porcupine? _porcupine;
        private WaveInEvent? _waveIn;
        private string _accessKey = "";
        private string? _customKeywordPath;
        private BuiltInKeyword _builtInKeyword = BuiltInKeyword.COMPUTER;
        private float _sensitivity = 0.5f;
        private bool _isListening = false;
        private readonly object _lockObject = new object();
        private readonly List<short> _audioBuffer = new List<short>();

        public event EventHandler<string>? WakeWordDetected;
        public event EventHandler<string>? StatusChanged;
        public event EventHandler<string>? ErrorOccurred;

        public bool IsListening => _isListening;

        private BuiltInKeyword MapWakeWordToKeyword(string wakeWord)
        {
            return wakeWord.ToLowerInvariant().Replace(" ", "") switch
            {
                "jarvis" => BuiltInKeyword.JARVIS,
                "alexa" => BuiltInKeyword.ALEXA,
                "computer" => BuiltInKeyword.COMPUTER,
                "heygoogle" or "hey google" => BuiltInKeyword.HEY_GOOGLE,
                "heysiri" or "hey siri" => BuiltInKeyword.HEY_SIRI,
                "okgoogle" or "ok google" => BuiltInKeyword.OK_GOOGLE,
                "picovoice" => BuiltInKeyword.PICOVOICE,
                "porcupine" => BuiltInKeyword.PORCUPINE,
                "bumblebee" => BuiltInKeyword.BUMBLEBEE,
                "terminator" => BuiltInKeyword.TERMINATOR,
                "americano" => BuiltInKeyword.AMERICANO,
                "blueberry" => BuiltInKeyword.BLUEBERRY,
                "grapefruit" => BuiltInKeyword.GRAPEFRUIT,
                "grasshopper" => BuiltInKeyword.GRASSHOPPER,
                _ => BuiltInKeyword.COMPUTER
            };
        }

        public void Initialize(string accessKey, string? customKeywordPath = null, float sensitivity = 0.5f, string wakeWord = "computer")
        {
            try
            {
                _accessKey = accessKey;
                _customKeywordPath = customKeywordPath;
                _sensitivity = sensitivity;
                _builtInKeyword = MapWakeWordToKeyword(wakeWord);

                if (string.IsNullOrWhiteSpace(_accessKey))
                {
                    throw new ArgumentException("Picovoice AccessKey is required. Get it free at https://console.picovoice.ai");
                }

                if (!string.IsNullOrEmpty(_customKeywordPath))
                {
                    _porcupine = Porcupine.FromKeywordPaths(
                        _accessKey,
                        new List<string> { _customKeywordPath },
                        modelPath: null,
                        sensitivities: new List<float> { _sensitivity }
                    );
                    StatusChanged?.Invoke(this, $"Porcupine initialized with custom keyword (sensitivity: {_sensitivity})");
                }
                else
                {
                    _porcupine = Porcupine.FromBuiltInKeywords(
                        _accessKey,
                        new List<BuiltInKeyword> { _builtInKeyword },
                        modelPath: null,
                        sensitivities: new List<float> { _sensitivity }
                    );
                    StatusChanged?.Invoke(this, $"Porcupine initialized with built-in keyword: {_builtInKeyword} (sensitivity: {_sensitivity})");
                }

                StatusChanged?.Invoke(this, $"Porcupine wake word engine ready (Frame length: {_porcupine.FrameLength}, Sample rate: {_porcupine.SampleRate} Hz)");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Porcupine initialization error: {ex.Message}");
                throw;
            }
        }

        public void StartListening()
        {
            if (_isListening)
            {
                StatusChanged?.Invoke(this, "Already listening");
                return;
            }

            if (_porcupine == null)
            {
                throw new InvalidOperationException("Porcupine not initialized. Call Initialize first.");
            }

            try
            {
                _waveIn?.Dispose();
                _waveIn = new WaveInEvent
                {
                    WaveFormat = new WaveFormat(_porcupine.SampleRate, 1),
                    BufferMilliseconds = (int)(_porcupine.FrameLength * 1000.0 / _porcupine.SampleRate)
                };

                _waveIn.DataAvailable += OnAudioDataAvailable;
                _waveIn.StartRecording();
                _isListening = true;

                StatusChanged?.Invoke(this, "Listening for wake word with Porcupine (high accuracy mode)");
            }
            catch (Exception ex)
            {
                _isListening = false;
                ErrorOccurred?.Invoke(this, $"Error starting Porcupine listener: {ex.Message}");
                throw;
            }
        }

        public void StopListening()
        {
            if (!_isListening)
            {
                return;
            }

            try
            {
                _waveIn?.StopRecording();
                _isListening = false;
                lock (_lockObject)
                {
                    _audioBuffer.Clear();
                }
                StatusChanged?.Invoke(this, "Stopped listening");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error stopping listener: {ex.Message}");
            }
        }

        private void OnAudioDataAvailable(object? sender, WaveInEventArgs e)
        {
            if (_porcupine == null || !_isListening)
            {
                return;
            }

            try
            {
                short[] audioData = new short[e.BytesRecorded / 2];
                Buffer.BlockCopy(e.Buffer, 0, audioData, 0, e.BytesRecorded);

                lock (_lockObject)
                {
                    _audioBuffer.AddRange(audioData);

                    while (_audioBuffer.Count >= _porcupine.FrameLength)
                    {
                        short[] frame = _audioBuffer.Take(_porcupine.FrameLength).ToArray();
                        _audioBuffer.RemoveRange(0, _porcupine.FrameLength);

                        int keywordIndex = _porcupine.Process(frame);

                        if (keywordIndex >= 0)
                        {
                            string detectedKeyword = _customKeywordPath != null
                                ? "Custom wake word"
                                : _builtInKeyword.ToString();

                            StatusChanged?.Invoke(this, $"✓ Wake word detected by Porcupine: '{detectedKeyword}' (Index: {keywordIndex})");
                            WakeWordDetected?.Invoke(this, detectedKeyword);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error processing audio: {ex.Message}");
            }
        }

        public void SetBuiltInKeyword(BuiltInKeyword keyword)
        {
            bool wasListening = _isListening;

            if (wasListening)
            {
                StopListening();
            }

            _builtInKeyword = keyword;
            _customKeywordPath = null;

            if (_porcupine != null)
            {
                _porcupine.Dispose();
                _porcupine = null;
            }

            if (!string.IsNullOrWhiteSpace(_accessKey))
            {
                Initialize(_accessKey, null, _sensitivity);

                if (wasListening)
                {
                    StartListening();
                }
            }
        }

        public void SetSensitivity(float sensitivity)
        {
            if (sensitivity < 0.0f || sensitivity > 1.0f)
            {
                throw new ArgumentException("Sensitivity must be between 0.0 and 1.0");
            }

            bool wasListening = _isListening;

            if (wasListening)
            {
                StopListening();
            }

            _sensitivity = sensitivity;

            if (_porcupine != null)
            {
                _porcupine.Dispose();
                _porcupine = null;
            }

            if (!string.IsNullOrWhiteSpace(_accessKey))
            {
                Initialize(_accessKey, _customKeywordPath, _sensitivity);

                if (wasListening)
                {
                    StartListening();
                }
            }

            StatusChanged?.Invoke(this, $"Sensitivity updated to {_sensitivity}");
        }

        public string GetVersion()
        {
            return _porcupine?.Version ?? "Not initialized";
        }

        public List<BuiltInKeyword> GetAvailableKeywords()
        {
            return Enum.GetValues(typeof(BuiltInKeyword)).Cast<BuiltInKeyword>().ToList();
        }

        public void Dispose()
        {
            StopListening();

            if (_waveIn != null)
            {
                _waveIn.DataAvailable -= OnAudioDataAvailable;
                _waveIn.Dispose();
                _waveIn = null;
            }

            if (_porcupine != null)
            {
                _porcupine.Dispose();
                _porcupine = null;
            }

            lock (_lockObject)
            {
                _audioBuffer.Clear();
            }
        }
    }
}
