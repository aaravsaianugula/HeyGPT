using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using HeyGPT.Models;
using HeyGPT.Services;
using HeyGPT.ViewModels;
using MessageBox = System.Windows.MessageBox;

namespace HeyGPT.Views
{
    public partial class MainWindow : Window
    {
        private readonly SpeechRecognitionService _speechService;
        private PorcupineWakeWordService? _porcupineService;
        private readonly WindowAutomationService _windowService;
        private readonly SettingsService _settingsService;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly StartupService _startupService;
        private AppSettings _currentSettings;
        private NotifyIcon? _notifyIcon;
        private readonly StringBuilder _logBuilder;
        private readonly DispatcherTimer _voiceModeCheckTimer;
        private bool _usePorcupine = false;

        public MainWindow()
        {
            InitializeComponent();

            _speechService = new SpeechRecognitionService();
            _windowService = new WindowAutomationService();
            _settingsService = new SettingsService();
            _settingsViewModel = new SettingsViewModel();
            _startupService = new StartupService();
            _logBuilder = new StringBuilder();

            _voiceModeCheckTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _voiceModeCheckTimer.Tick += VoiceModeCheckTimer_Tick;

            _currentSettings = _settingsService.LoadSettings();
            _settingsViewModel.LoadFromSettings(_currentSettings);

            InitializeSpeechRecognition();
            SetupSystemTray();

            _windowService.DebugLog += (s, message) => Dispatcher.Invoke(() => AddLog(message));

            UpdateStatus("Ready. Please configure settings before starting.");

            if (_currentSettings.StartMinimized)
            {
                WindowState = WindowState.Minimized;
            }
        }

        private void InitializeSpeechRecognition()
        {
            try
            {
                _usePorcupine = _currentSettings.UsePorcupine && !string.IsNullOrWhiteSpace(_currentSettings.PicovoiceAccessKey);

                if (_usePorcupine)
                {
                    _porcupineService = new PorcupineWakeWordService();
                    _porcupineService.Initialize(_currentSettings.PicovoiceAccessKey, null, _currentSettings.PorcupineSensitivity);
                    _porcupineService.WakeWordDetected += OnWakeWordDetected;
                    _porcupineService.StatusChanged += OnSpeechStatusChanged;
                    _porcupineService.ErrorOccurred += OnSpeechError;
                    AddLog($"✓ Using Porcupine wake word engine (high accuracy)");
                    AddLog($"Porcupine version: {_porcupineService.GetVersion()}");
                }
                else
                {
                    _speechService.Initialize(_currentSettings.WakeWord, _currentSettings.SpeechConfidenceThreshold);
                    _speechService.ConfigureIsolation(
                        _currentSettings.EnableWakeWordIsolation,
                        _currentSettings.SilenceThreshold,
                        _currentSettings.MinimumSilenceDurationMs,
                        _currentSettings.CooldownPeriodMs
                    );
                    _speechService.WakeWordDetected += OnWakeWordDetected;
                    _speechService.StatusChanged += OnSpeechStatusChanged;
                    _speechService.ErrorOccurred += OnSpeechError;
                    AddLog("Using System.Speech for wake word detection");
                }

                _speechService.InitializeCommands();
                _speechService.CommandRecognized += OnCommandRecognized;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize speech recognition:\n{ex.Message}\n\nPlease ensure a microphone is connected and Picovoice AccessKey is valid.",
                              "Initialization Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void SetupSystemTray()
        {
            Icon? trayIcon = null;
            try
            {
                string iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqaureLog.ico");
                if (System.IO.File.Exists(iconPath))
                {
                    trayIcon = new Icon(iconPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load custom tray icon: {ex.Message}");
            }

            _notifyIcon = new NotifyIcon
            {
                Icon = trayIcon ?? SystemIcons.Application,
                Visible = true,
                Text = "HeyGPT - Voice Assistant"
            };

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Show", null, (s, e) => ShowWindow());
            contextMenu.Items.Add("Settings", null, (s, e) => ShowSettings());
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Exit", null, (s, e) => ExitApplication());

            _notifyIcon.ContextMenuStrip = contextMenu;
            _notifyIcon.DoubleClick += (s, e) => ShowWindow();
        }

        private void ShowWindow()
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        private void ShowSettings()
        {
            ShowWindow();
            OpenSettingsWindow();
        }

        private void ExitApplication()
        {
            _notifyIcon?.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentSettings.IsMonitorConfigured)
            {
                MessageBox.Show("Please configure the target monitor in Settings before starting.",
                              "Configuration Required",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_usePorcupine && _porcupineService != null)
                {
                    _porcupineService.StartListening();
                    AddLog("🎤 Porcupine listening for wake word (high accuracy mode)");
                }
                else
                {
                    _speechService.StartListening();
                    AddLog($"🎤 Say '{_currentSettings.WakeWord}' to launch ChatGPT");
                }

                _voiceModeCheckTimer.Start();
                StartButton.IsEnabled = false;
                StopButton.IsEnabled = true;
                UpdateStatus("Listening for wake word (Continuous)");
                AddLog("Speech recognition started - will continue listening after each detection");
                AddLog("Voice mode monitoring active - commands will work when ChatGPT is in voice mode");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start listening:\n{ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_usePorcupine && _porcupineService != null)
            {
                _porcupineService.StopListening();
            }
            else
            {
                _speechService.StopListening();
            }

            _voiceModeCheckTimer.Stop();
            _speechService.IsInVoiceMode = false;
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            UpdateStatus("Stopped listening");
            AddLog("Speech recognition stopped");
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenSettingsWindow();
        }

        private void UserGuideButton_Click(object sender, RoutedEventArgs e)
        {
            UserGuideWindow userGuideWindow = new UserGuideWindow
            {
                Owner = this
            };
            userGuideWindow.ShowDialog();
        }

        private void OpenSettingsWindow()
        {
            SettingsViewModel tempViewModel = new SettingsViewModel();
            tempViewModel.LoadFromSettings(_currentSettings);

            SettingsWindow settingsWindow = new SettingsWindow(tempViewModel)
            {
                Owner = this
            };

            if (settingsWindow.ShowDialog() == true)
            {
                var oldSettings = _currentSettings;
                _currentSettings = tempViewModel.ToSettings();
                _settingsService.SaveSettings(_currentSettings);
                _settingsViewModel.LoadFromSettings(_currentSettings);

                bool wasListening = _speechService.IsListening;
                if (wasListening)
                {
                    _speechService.StopListening();
                }

                _speechService.UpdateWakeWord(_currentSettings.WakeWord, _currentSettings.SpeechConfidenceThreshold);
                _speechService.ConfigureIsolation(
                    _currentSettings.EnableWakeWordIsolation,
                    _currentSettings.SilenceThreshold,
                    _currentSettings.MinimumSilenceDurationMs,
                    _currentSettings.CooldownPeriodMs
                );

                if (wasListening)
                {
                    _speechService.StartListening();
                }

                UpdateStatus("Settings saved and applied successfully");
                AddLog("=== Settings Applied ===");
                LogSettingsChanges(oldSettings, _currentSettings);
            }
        }

        private async void TestButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentSettings.IsMonitorConfigured)
            {
                MessageBox.Show("Please configure the target monitor in Settings first.",
                              "Configuration Required",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            UpdateStatus("Testing ChatGPT launch and automation...");
            AddLog("Starting test launch...");
            TestButton.IsEnabled = false;

            try
            {
                bool success = await _windowService.LaunchAndControlChatGpt(
                    _currentSettings.ChatGptAppPath,
                    _currentSettings.TargetMonitorCenter,
                    _currentSettings.ButtonClickDelayMs,
                    _currentSettings.NewChatButtonText,
                    _currentSettings.VoiceModeButtonText,
                    _currentSettings.NewChatButtonPosition,
                    _currentSettings.IsNewChatButtonConfigured,
                    _currentSettings.VoiceModeButtonPosition,
                    _currentSettings.IsVoiceModeButtonConfigured
                );

                if (success)
                {
                    UpdateStatus("Test successful! ChatGPT launched and buttons clicked.");
                    AddLog("✓ Test completed successfully");
                    MessageBox.Show("Test successful!\n\nChatGPT was launched, positioned on your target monitor, and voice mode was activated.",
                                  "Success",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
                else
                {
                    UpdateStatus("Test failed. Check if ChatGPT is installed and the command works.");
                    AddLog("✗ Test failed - see details above");
                    MessageBox.Show($"Test failed!\n\nPlease check:\n\n" +
                                  $"1. ChatGPT command: '{_currentSettings.ChatGptAppPath}'\n" +
                                  $"2. Try running 'chatgpt' in PowerShell/CMD\n" +
                                  $"3. Monitor is configured: {_currentSettings.IsMonitorConfigured}\n" +
                                  $"4. Check the activity log for details",
                                  "Test Failed",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Test error: {ex.Message}");
                AddLog($"✗ Error: {ex.Message}");
            }
            finally
            {
                TestButton.IsEnabled = true;
            }
        }

        private async void OnWakeWordDetected(object? sender, string recognizedText)
        {
            Dispatcher.Invoke(() =>
            {
                UpdateStatus($"Wake word detected: '{recognizedText}' - Launching ChatGPT...");
                AddLog($"🎤 Wake word detected: '{recognizedText}'");
            });

            try
            {
                bool success = await _windowService.LaunchAndControlChatGpt(
                    _currentSettings.ChatGptAppPath,
                    _currentSettings.TargetMonitorCenter,
                    _currentSettings.ButtonClickDelayMs,
                    _currentSettings.NewChatButtonText,
                    _currentSettings.VoiceModeButtonText,
                    _currentSettings.NewChatButtonPosition,
                    _currentSettings.IsNewChatButtonConfigured,
                    _currentSettings.VoiceModeButtonPosition,
                    _currentSettings.IsVoiceModeButtonConfigured
                );

                Dispatcher.Invoke(() =>
                {
                    if (success)
                    {
                        UpdateStatus($"ChatGPT launched successfully! Still listening for '{_currentSettings.WakeWord}'...");
                        AddLog("✓ ChatGPT automation completed");
                        AddLog($"🎤 Ready for next wake word: '{_currentSettings.WakeWord}'");
                    }
                    else
                    {
                        UpdateStatus("Failed to launch or control ChatGPT");
                        AddLog("✗ Automation failed");
                    }

                    if (!_speechService.IsListening)
                    {
                        AddLog("⚠ Speech recognition stopped - restarting...");
                        try
                        {
                            _speechService.StartListening();
                            UpdateStatus($"Listening resumed for wake word: '{_currentSettings.WakeWord}'");
                            AddLog("✓ Speech recognition restarted");
                        }
                        catch (Exception restartEx)
                        {
                            AddLog($"✗ Failed to restart listening: {restartEx.Message}");
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    UpdateStatus($"Error: {ex.Message}");
                    AddLog($"✗ Error: {ex.Message}");
                });
            }
        }

        private void OnSpeechStatusChanged(object? sender, string status)
        {
            Dispatcher.Invoke(() => AddLog(status));
        }

        private void OnSpeechError(object? sender, string error)
        {
            Dispatcher.Invoke(() =>
            {
                UpdateStatus($"Speech error: {error}");
                AddLog($"✗ Error: {error}");
            });
        }

        private async void VoiceModeCheckTimer_Tick(object? sender, EventArgs e)
        {
            bool isVoiceModeActive = await _windowService.IsVoiceModeActive();
            if (_speechService.IsInVoiceMode != isVoiceModeActive)
            {
                _speechService.IsInVoiceMode = isVoiceModeActive;
                if (isVoiceModeActive)
                {
                    AddLog("✓ Voice mode detected - voice commands ENABLED");
                }
                else
                {
                    AddLog("Voice mode not active - voice commands DISABLED");
                }
            }
        }

        private async void OnCommandRecognized(object? sender, string command)
        {
            Dispatcher.Invoke(() =>
            {
                AddLog($"🎤 Voice command: '{command}'");
            });

            try
            {
                if (command.Contains("mic on") || command.Contains("mic off"))
                {
                    if (!_currentSettings.IsMicButtonConfigured)
                    {
                        Dispatcher.Invoke(() => AddLog("⚠ Mic button position not configured - configure in settings"));
                        return;
                    }

                    await _windowService.ClickMicButton(_currentSettings.MicButtonPosition, _currentSettings.IsMicButtonConfigured);
                    Dispatcher.Invoke(() => AddLog($"✓ Mic button clicked ({command})"));
                }
                else if (command.Contains("exit"))
                {
                    if (!_currentSettings.IsExitVoiceModeButtonConfigured)
                    {
                        Dispatcher.Invoke(() => AddLog("⚠ Exit voice mode button position not configured - configure in settings"));
                        return;
                    }

                    await _windowService.ClickExitVoiceModeButton(_currentSettings.ExitVoiceModeButtonPosition, _currentSettings.IsExitVoiceModeButtonConfigured);
                    Dispatcher.Invoke(() => AddLog("✓ Exit voice mode button clicked"));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => AddLog($"✗ Command error: {ex.Message}"));
            }
        }

        private void UpdateStatus(string status)
        {
            StatusTextBlock.Text = $"{DateTime.Now:HH:mm:ss} - {status}";
        }

        private void AddLog(string message)
        {
            _logBuilder.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}\n");

            if (_logBuilder.Length > 5000)
            {
                _logBuilder.Length = 5000;
            }

            LogTextBlock.Text = _logBuilder.ToString();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (_notifyIcon != null)
                {
                    _notifyIcon.BalloonTipTitle = "HeyGPT";
                    _notifyIcon.BalloonTipText = "Application minimized to system tray";
                    _notifyIcon.ShowBalloonTip(2000);
                }
            }

            base.OnStateChanged(e);
        }

        private void LogSettingsChanges(AppSettings oldSettings, AppSettings newSettings)
        {
            if (oldSettings.WakeWord != newSettings.WakeWord)
                AddLog($"  Wake Word: '{oldSettings.WakeWord}' → '{newSettings.WakeWord}'");

            if (Math.Abs(oldSettings.SpeechConfidenceThreshold - newSettings.SpeechConfidenceThreshold) > 0.01f)
                AddLog($"  Confidence Threshold: {oldSettings.SpeechConfidenceThreshold:F1} → {newSettings.SpeechConfidenceThreshold:F1}");

            if (oldSettings.EnableWakeWordIsolation != newSettings.EnableWakeWordIsolation)
                AddLog($"  Isolation Enabled: {oldSettings.EnableWakeWordIsolation} → {newSettings.EnableWakeWordIsolation}");

            if (oldSettings.SilenceThreshold != newSettings.SilenceThreshold)
                AddLog($"  Silence Threshold: {oldSettings.SilenceThreshold} → {newSettings.SilenceThreshold}");

            if (oldSettings.MinimumSilenceDurationMs != newSettings.MinimumSilenceDurationMs)
                AddLog($"  Min Silence Duration: {oldSettings.MinimumSilenceDurationMs}ms → {newSettings.MinimumSilenceDurationMs}ms");

            if (oldSettings.CooldownPeriodMs != newSettings.CooldownPeriodMs)
                AddLog($"  Cooldown Period: {oldSettings.CooldownPeriodMs}ms → {newSettings.CooldownPeriodMs}ms");

            if (oldSettings.ButtonClickDelayMs != newSettings.ButtonClickDelayMs)
                AddLog($"  Button Click Delay: {oldSettings.ButtonClickDelayMs}ms → {newSettings.ButtonClickDelayMs}ms");

            if (oldSettings.ChatGptAppPath != newSettings.ChatGptAppPath)
                AddLog($"  ChatGPT Path: '{oldSettings.ChatGptAppPath}' → '{newSettings.ChatGptAppPath}'");

            if (oldSettings.IsMonitorConfigured != newSettings.IsMonitorConfigured || oldSettings.TargetMonitorCenter != newSettings.TargetMonitorCenter)
            {
                if (newSettings.IsMonitorConfigured)
                    AddLog($"  Monitor: Configured at ({newSettings.TargetMonitorCenter.X}, {newSettings.TargetMonitorCenter.Y})");
                else
                    AddLog($"  Monitor: Not configured");
            }

            if (oldSettings.IsNewChatButtonConfigured != newSettings.IsNewChatButtonConfigured || oldSettings.NewChatButtonPosition != newSettings.NewChatButtonPosition)
            {
                if (newSettings.IsNewChatButtonConfigured)
                    AddLog($"  New Chat Button: Configured at ({newSettings.NewChatButtonPosition.X}, {newSettings.NewChatButtonPosition.Y})");
                else
                    AddLog($"  New Chat Button: Not configured (using defaults)");
            }

            if (oldSettings.IsVoiceModeButtonConfigured != newSettings.IsVoiceModeButtonConfigured || oldSettings.VoiceModeButtonPosition != newSettings.VoiceModeButtonPosition)
            {
                if (newSettings.IsVoiceModeButtonConfigured)
                    AddLog($"  Voice Mode Button: Configured at ({newSettings.VoiceModeButtonPosition.X}, {newSettings.VoiceModeButtonPosition.Y})");
                else
                    AddLog($"  Voice Mode Button: Not configured (using defaults)");
            }

            if (oldSettings.StartWithWindows != newSettings.StartWithWindows)
            {
                AddLog($"  Start with Windows: {oldSettings.StartWithWindows} → {newSettings.StartWithWindows}");
                _startupService.SetStartupEnabled(newSettings.StartWithWindows);
            }

            if (oldSettings.StartMinimized != newSettings.StartMinimized)
                AddLog($"  Start Minimized: {oldSettings.StartMinimized} → {newSettings.StartMinimized}");

            AddLog("✓ All settings saved and applied");
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _speechService?.Dispose();
            _notifyIcon?.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimateWindowEntrance();
        }

        private async void AnimateWindowEntrance()
        {
            System.Windows.Media.Animation.Storyboard windowFadeIn = new System.Windows.Media.Animation.Storyboard();
            System.Windows.Media.Animation.DoubleAnimation fadeAnimation = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            System.Windows.Media.Animation.Storyboard.SetTarget(fadeAnimation, this);
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(OpacityProperty));
            windowFadeIn.Children.Add(fadeAnimation);
            windowFadeIn.Begin();

            await Task.Delay(100);
            AnimateElement(HeaderPanel, 0);

            await Task.Delay(100);
            AnimateElement(StatusCard, 0.1);

            await Task.Delay(100);
            AnimateElement(ActivityLogCard, 0.15);

            await Task.Delay(100);
            AnimateElement(ActionButtonsPanel, 0.2, true);

            await Task.Delay(100);
            AnimateElement(BottomButtonsPanel, 0.25, true);
        }

        private void AnimateElement(FrameworkElement element, double delay, bool slideFromBottom = false)
        {
            System.Windows.Media.Animation.Storyboard storyboard = new System.Windows.Media.Animation.Storyboard();
            storyboard.BeginTime = TimeSpan.FromSeconds(delay);

            System.Windows.Media.Animation.DoubleAnimation fadeIn = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut }
            };
            System.Windows.Media.Animation.Storyboard.SetTarget(fadeIn, element);
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(fadeIn, new PropertyPath(OpacityProperty));

            if (element.RenderTransform is System.Windows.Media.TranslateTransform translateTransform)
            {
                System.Windows.Media.Animation.DoubleAnimation slideAnimation = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = slideFromBottom ? 30 : -30,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut }
                };
                System.Windows.Media.Animation.Storyboard.SetTarget(slideAnimation, element);
                System.Windows.Media.Animation.Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("RenderTransform.Y"));
                storyboard.Children.Add(slideAnimation);
            }
            else if (element.RenderTransform is System.Windows.Media.ScaleTransform)
            {
                System.Windows.Media.Animation.DoubleAnimation scaleXAnimation = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0.9,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5),
                    EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut }
                };
                System.Windows.Media.Animation.Storyboard.SetTarget(scaleXAnimation, element);
                System.Windows.Media.Animation.Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("RenderTransform.ScaleX"));

                System.Windows.Media.Animation.DoubleAnimation scaleYAnimation = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 0.9,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5),
                    EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut }
                };
                System.Windows.Media.Animation.Storyboard.SetTarget(scaleYAnimation, element);
                System.Windows.Media.Animation.Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("RenderTransform.ScaleY"));

                storyboard.Children.Add(scaleXAnimation);
                storyboard.Children.Add(scaleYAnimation);
            }

            storyboard.Children.Add(fadeIn);
            storyboard.Begin();
        }
    }
}
