using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using HeyGPT.Services;
using HeyGPT.ViewModels;

namespace HeyGPT.Views
{
    public partial class SettingsWindow : Window
    {
        private readonly SettingsViewModel _viewModel;
        private readonly MonitorService _monitorService;
        private bool _isCapturing = false;
        private bool _isCapturingNewChatButton = false;
        private bool _isCapturingVoiceModeButton = false;
        private bool _isCapturingMicButton = false;
        private bool _isCapturingExitVoiceModeButton = false;

        public SettingsWindow(SettingsViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;

            _monitorService = new MonitorService();
            _monitorService.CountdownTick += OnCountdownTick;
            _monitorService.MonitorCaptured += OnMonitorCaptured;
        }

        private async void CaptureMonitorButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isCapturing)
            {
                _monitorService.CancelMonitorCapture();
                ResetCaptureUI();
                return;
            }

            _isCapturing = true;
            CaptureMonitorButton.Content = "Cancel Capture";
            CountdownTextBlock.Visibility = Visibility.Visible;

            try
            {
                await _monitorService.StartMonitorCapture();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during monitor capture: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                ResetCaptureUI();
            }
        }

        private void OnCountdownTick(object? sender, int secondsRemaining)
        {
            Dispatcher.Invoke(() =>
            {
                if (CountdownTextBlock.Child is TextBlock textBlock)
                {
                    textBlock.Text = $"Move mouse to target monitor center... {secondsRemaining}s";
                }
            });
        }

        private void OnMonitorCaptured(object? sender, System.Drawing.Point mousePosition)
        {
            Dispatcher.Invoke(() =>
            {
                _viewModel.TargetMonitorCenter = mousePosition;
                _viewModel.IsMonitorConfigured = true;
                _viewModel.UpdateMonitorInfo();

                MessageBox.Show($"Monitor configured successfully!\n\n{_viewModel.MonitorInfo}",
                              "Success",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);

                ResetCaptureUI();
            });
        }

        private void ResetCaptureUI()
        {
            _isCapturing = false;
            CaptureMonitorButton.Content = "Configure Monitor (10s countdown)";
            CountdownTextBlock.Visibility = Visibility.Collapsed;
            if (CountdownTextBlock.Child is TextBlock textBlock)
            {
                textBlock.Text = "";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_viewModel.WakeWord))
            {
                MessageBox.Show("Wake word cannot be empty.",
                              "Validation Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            if (!_viewModel.IsMonitorConfigured)
            {
                var result = MessageBox.Show("Monitor is not configured. Continue anyway?",
                                            "Warning",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void CaptureNewChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isCapturingNewChatButton)
            {
                _monitorService.CancelMonitorCapture();
                ResetNewChatCaptureUI();
                return;
            }

            _isCapturingNewChatButton = true;
            CaptureNewChatButton.Content = "Cancel Capture";
            NewChatCountdownTextBlock.Visibility = Visibility.Visible;

            EventHandler<int>? countdownHandler = null;
            EventHandler<System.Drawing.Point>? captureHandler = null;

            countdownHandler = (s, secondsRemaining) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (NewChatCountdownTextBlock.Child is TextBlock textBlock)
                    {
                        textBlock.Text = $"Hover mouse over New Chat button... {secondsRemaining}s";
                    }
                });
            };

            captureHandler = (s, mousePosition) =>
            {
                Dispatcher.Invoke(() =>
                {
                    _viewModel.NewChatButtonPosition = (System.Drawing.Point)mousePosition;
                    _viewModel.IsNewChatButtonConfigured = true;
                    _viewModel.UpdateButtonInfo();

                    MessageBox.Show($"New Chat button position captured!\n\nPosition: ({mousePosition.X}, {mousePosition.Y})",
                                  "Success",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);

                    _monitorService.CountdownTick -= countdownHandler;
                    _monitorService.MonitorCaptured -= captureHandler;
                    ResetNewChatCaptureUI();
                });
            };

            _monitorService.CountdownTick += countdownHandler;
            _monitorService.MonitorCaptured += captureHandler;

            try
            {
                await _monitorService.StartMonitorCapture();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during button capture: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                _monitorService.CountdownTick -= countdownHandler;
                _monitorService.MonitorCaptured -= captureHandler;
                ResetNewChatCaptureUI();
            }
        }

        private async void CaptureVoiceModeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isCapturingVoiceModeButton)
            {
                _monitorService.CancelMonitorCapture();
                ResetVoiceModeCaptureUI();
                return;
            }

            _isCapturingVoiceModeButton = true;
            CaptureVoiceModeButton.Content = "Cancel Capture";
            VoiceModeCountdownTextBlock.Visibility = Visibility.Visible;

            EventHandler<int>? countdownHandler = null;
            EventHandler<System.Drawing.Point>? captureHandler = null;

            countdownHandler = (s, secondsRemaining) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (VoiceModeCountdownTextBlock.Child is TextBlock textBlock)
                    {
                        textBlock.Text = $"Hover mouse over Voice Mode button... {secondsRemaining}s";
                    }
                });
            };

            captureHandler = (s, mousePosition) =>
            {
                Dispatcher.Invoke(() =>
                {
                    _viewModel.VoiceModeButtonPosition = (System.Drawing.Point)mousePosition;
                    _viewModel.IsVoiceModeButtonConfigured = true;
                    _viewModel.UpdateButtonInfo();

                    MessageBox.Show($"Voice Mode button position captured!\n\nPosition: ({mousePosition.X}, {mousePosition.Y})",
                                  "Success",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);

                    _monitorService.CountdownTick -= countdownHandler;
                    _monitorService.MonitorCaptured -= captureHandler;
                    ResetVoiceModeCaptureUI();
                });
            };

            _monitorService.CountdownTick += countdownHandler;
            _monitorService.MonitorCaptured += captureHandler;

            try
            {
                await _monitorService.StartMonitorCapture();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during button capture: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                _monitorService.CountdownTick -= countdownHandler;
                _monitorService.MonitorCaptured -= captureHandler;
                ResetVoiceModeCaptureUI();
            }
        }

        private void ResetNewChatCaptureUI()
        {
            _isCapturingNewChatButton = false;
            CaptureNewChatButton.Content = "Capture New Chat Button (10s countdown)";
            NewChatCountdownTextBlock.Visibility = Visibility.Collapsed;
            if (NewChatCountdownTextBlock.Child is TextBlock textBlock)
            {
                textBlock.Text = "";
            }
        }

        private void ResetVoiceModeCaptureUI()
        {
            _isCapturingVoiceModeButton = false;
            CaptureVoiceModeButton.Content = "Capture Voice Mode Button (10s countdown)";
            VoiceModeCountdownTextBlock.Visibility = Visibility.Collapsed;
            if (VoiceModeCountdownTextBlock.Child is TextBlock textBlock)
            {
                textBlock.Text = "";
            }
        }

        private async void CaptureMicButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isCapturingMicButton)
            {
                _monitorService.CancelMonitorCapture();
                ResetMicCaptureUI();
                return;
            }

            _isCapturingMicButton = true;
            CaptureMicButton.Content = "Cancel Capture";
            MicCountdownTextBlock.Visibility = Visibility.Visible;

            EventHandler<int>? countdownHandler = null;
            EventHandler<System.Drawing.Point>? captureHandler = null;

            countdownHandler = (s, secondsRemaining) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (MicCountdownTextBlock.Child is TextBlock textBlock)
                    {
                        textBlock.Text = $"Hover mouse over Mic button... {secondsRemaining}s";
                    }
                });
            };

            captureHandler = (s, mousePosition) =>
            {
                Dispatcher.Invoke(() =>
                {
                    _viewModel.MicButtonPosition = (System.Drawing.Point)mousePosition;
                    _viewModel.IsMicButtonConfigured = true;
                    _viewModel.UpdateButtonInfo();

                    MessageBox.Show($"Mic button position captured!\n\nPosition: ({mousePosition.X}, {mousePosition.Y})",
                                  "Success",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);

                    _monitorService.CountdownTick -= countdownHandler;
                    _monitorService.MonitorCaptured -= captureHandler;
                    ResetMicCaptureUI();
                });
            };

            _monitorService.CountdownTick += countdownHandler;
            _monitorService.MonitorCaptured += captureHandler;

            try
            {
                await _monitorService.StartMonitorCapture();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during button capture: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                _monitorService.CountdownTick -= countdownHandler;
                _monitorService.MonitorCaptured -= captureHandler;
                ResetMicCaptureUI();
            }
        }

        private async void CaptureExitVoiceModeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isCapturingExitVoiceModeButton)
            {
                _monitorService.CancelMonitorCapture();
                ResetExitVoiceModeCaptureUI();
                return;
            }

            _isCapturingExitVoiceModeButton = true;
            CaptureExitVoiceModeButton.Content = "Cancel Capture";
            ExitVoiceModeCountdownTextBlock.Visibility = Visibility.Visible;

            EventHandler<int>? countdownHandler = null;
            EventHandler<System.Drawing.Point>? captureHandler = null;

            countdownHandler = (s, secondsRemaining) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (ExitVoiceModeCountdownTextBlock.Child is TextBlock textBlock)
                    {
                        textBlock.Text = $"Hover mouse over Exit Voice Mode button... {secondsRemaining}s";
                    }
                });
            };

            captureHandler = (s, mousePosition) =>
            {
                Dispatcher.Invoke(() =>
                {
                    _viewModel.ExitVoiceModeButtonPosition = (System.Drawing.Point)mousePosition;
                    _viewModel.IsExitVoiceModeButtonConfigured = true;
                    _viewModel.UpdateButtonInfo();

                    MessageBox.Show($"Exit Voice Mode button position captured!\n\nPosition: ({mousePosition.X}, {mousePosition.Y})",
                                  "Success",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);

                    _monitorService.CountdownTick -= countdownHandler;
                    _monitorService.MonitorCaptured -= captureHandler;
                    ResetExitVoiceModeCaptureUI();
                });
            };

            _monitorService.CountdownTick += countdownHandler;
            _monitorService.MonitorCaptured += captureHandler;

            try
            {
                await _monitorService.StartMonitorCapture();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during button capture: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                _monitorService.CountdownTick -= countdownHandler;
                _monitorService.MonitorCaptured -= captureHandler;
                ResetExitVoiceModeCaptureUI();
            }
        }

        private void ResetMicCaptureUI()
        {
            _isCapturingMicButton = false;
            CaptureMicButton.Content = "Capture Mic Button (10s countdown)";
            MicCountdownTextBlock.Visibility = Visibility.Collapsed;
            if (MicCountdownTextBlock.Child is TextBlock textBlock)
            {
                textBlock.Text = "";
            }
        }

        private void ResetExitVoiceModeCaptureUI()
        {
            _isCapturingExitVoiceModeButton = false;
            CaptureExitVoiceModeButton.Content = "Capture Exit Voice Mode Button (10s countdown)";
            ExitVoiceModeCountdownTextBlock.Visibility = Visibility.Collapsed;
            if (ExitVoiceModeCountdownTextBlock.Child is TextBlock textBlock)
            {
                textBlock.Text = "";
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _monitorService.CountdownTick -= OnCountdownTick;
            _monitorService.MonitorCaptured -= OnMonitorCaptured;
            _monitorService.Dispose();
            base.OnClosed(e);
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open browser: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Animation.Storyboard windowFadeIn = new System.Windows.Media.Animation.Storyboard();
            System.Windows.Media.Animation.DoubleAnimation fadeAnimation = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.4)
            };
            System.Windows.Media.Animation.Storyboard.SetTarget(fadeAnimation, this);
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(OpacityProperty));
            windowFadeIn.Children.Add(fadeAnimation);
            windowFadeIn.Begin();
        }
    }
}
