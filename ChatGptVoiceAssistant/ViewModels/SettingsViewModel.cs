using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using HeyGPT.Models;

namespace HeyGPT.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private string _wakeWord = "Hey GPT";
        private string _monitorInfo = "Not configured";
        private bool _isMonitorConfigured = false;
        private Point _targetMonitorCenter = Point.Empty;
        private int _buttonClickDelay = 1000;
        private float _confidenceThreshold = 0.7f;
        private string _chatGptAppPath = "chatgpt";
        private string _newChatButtonText = "New chat";
        private string _voiceModeButtonText = "Voice";
        private Point _newChatButtonPosition = Point.Empty;
        private bool _isNewChatButtonConfigured = false;
        private string _newChatButtonInfo = "Not configured";
        private Point _voiceModeButtonPosition = Point.Empty;
        private bool _isVoiceModeButtonConfigured = false;
        private string _voiceModeButtonInfo = "Not configured";
        private bool _enableWakeWordIsolation = true;
        private int _silenceThreshold = 10;
        private int _minimumSilenceDurationMs = 800;
        private int _cooldownPeriodMs = 2500;
        private bool _startWithWindows = false;
        private bool _startMinimized = false;
        private Point _micButtonPosition = Point.Empty;
        private bool _isMicButtonConfigured = false;
        private string _micButtonInfo = "Not configured";
        private Point _exitVoiceModeButtonPosition = Point.Empty;
        private bool _isExitVoiceModeButtonConfigured = false;
        private string _exitVoiceModeButtonInfo = "Not configured";

        public string WakeWord
        {
            get => _wakeWord;
            set
            {
                if (_wakeWord != value)
                {
                    _wakeWord = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MonitorInfo
        {
            get => _monitorInfo;
            set
            {
                if (_monitorInfo != value)
                {
                    _monitorInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsMonitorConfigured
        {
            get => _isMonitorConfigured;
            set
            {
                if (_isMonitorConfigured != value)
                {
                    _isMonitorConfigured = value;
                    OnPropertyChanged();
                }
            }
        }

        public Point TargetMonitorCenter
        {
            get => _targetMonitorCenter;
            set
            {
                if (_targetMonitorCenter != value)
                {
                    _targetMonitorCenter = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ButtonClickDelay
        {
            get => _buttonClickDelay;
            set
            {
                if (_buttonClickDelay != value)
                {
                    _buttonClickDelay = value;
                    OnPropertyChanged();
                }
            }
        }

        public float ConfidenceThreshold
        {
            get => _confidenceThreshold;
            set
            {
                if (Math.Abs(_confidenceThreshold - value) > 0.001f)
                {
                    _confidenceThreshold = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ChatGptAppPath
        {
            get => _chatGptAppPath;
            set
            {
                if (_chatGptAppPath != value)
                {
                    _chatGptAppPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NewChatButtonText
        {
            get => _newChatButtonText;
            set
            {
                if (_newChatButtonText != value)
                {
                    _newChatButtonText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string VoiceModeButtonText
        {
            get => _voiceModeButtonText;
            set
            {
                if (_voiceModeButtonText != value)
                {
                    _voiceModeButtonText = value;
                    OnPropertyChanged();
                }
            }
        }

        public Point NewChatButtonPosition
        {
            get => _newChatButtonPosition;
            set
            {
                if (_newChatButtonPosition != value)
                {
                    _newChatButtonPosition = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsNewChatButtonConfigured
        {
            get => _isNewChatButtonConfigured;
            set
            {
                if (_isNewChatButtonConfigured != value)
                {
                    _isNewChatButtonConfigured = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NewChatButtonInfo
        {
            get => _newChatButtonInfo;
            set
            {
                if (_newChatButtonInfo != value)
                {
                    _newChatButtonInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        public Point VoiceModeButtonPosition
        {
            get => _voiceModeButtonPosition;
            set
            {
                if (_voiceModeButtonPosition != value)
                {
                    _voiceModeButtonPosition = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsVoiceModeButtonConfigured
        {
            get => _isVoiceModeButtonConfigured;
            set
            {
                if (_isVoiceModeButtonConfigured != value)
                {
                    _isVoiceModeButtonConfigured = value;
                    OnPropertyChanged();
                }
            }
        }

        public string VoiceModeButtonInfo
        {
            get => _voiceModeButtonInfo;
            set
            {
                if (_voiceModeButtonInfo != value)
                {
                    _voiceModeButtonInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool EnableWakeWordIsolation
        {
            get => _enableWakeWordIsolation;
            set
            {
                if (_enableWakeWordIsolation != value)
                {
                    _enableWakeWordIsolation = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SilenceThreshold
        {
            get => _silenceThreshold;
            set
            {
                if (_silenceThreshold != value)
                {
                    _silenceThreshold = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MinimumSilenceDurationMs
        {
            get => _minimumSilenceDurationMs;
            set
            {
                if (_minimumSilenceDurationMs != value)
                {
                    _minimumSilenceDurationMs = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CooldownPeriodMs
        {
            get => _cooldownPeriodMs;
            set
            {
                if (_cooldownPeriodMs != value)
                {
                    _cooldownPeriodMs = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool StartWithWindows
        {
            get => _startWithWindows;
            set
            {
                if (_startWithWindows != value)
                {
                    _startWithWindows = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool StartMinimized
        {
            get => _startMinimized;
            set
            {
                if (_startMinimized != value)
                {
                    _startMinimized = value;
                    OnPropertyChanged();
                }
            }
        }

        public Point MicButtonPosition
        {
            get => _micButtonPosition;
            set
            {
                if (_micButtonPosition != value)
                {
                    _micButtonPosition = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsMicButtonConfigured
        {
            get => _isMicButtonConfigured;
            set
            {
                if (_isMicButtonConfigured != value)
                {
                    _isMicButtonConfigured = value;
                    OnPropertyChanged();
                    MicButtonInfo = value ? $"Configured at ({_micButtonPosition.X}, {_micButtonPosition.Y})" : "Not configured";
                }
            }
        }

        public string MicButtonInfo
        {
            get => _micButtonInfo;
            set
            {
                if (_micButtonInfo != value)
                {
                    _micButtonInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        public Point ExitVoiceModeButtonPosition
        {
            get => _exitVoiceModeButtonPosition;
            set
            {
                if (_exitVoiceModeButtonPosition != value)
                {
                    _exitVoiceModeButtonPosition = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsExitVoiceModeButtonConfigured
        {
            get => _isExitVoiceModeButtonConfigured;
            set
            {
                if (_isExitVoiceModeButtonConfigured != value)
                {
                    _isExitVoiceModeButtonConfigured = value;
                    OnPropertyChanged();
                    ExitVoiceModeButtonInfo = value ? $"Configured at ({_exitVoiceModeButtonPosition.X}, {_exitVoiceModeButtonPosition.Y})" : "Not configured";
                }
            }
        }

        public string ExitVoiceModeButtonInfo
        {
            get => _exitVoiceModeButtonInfo;
            set
            {
                if (_exitVoiceModeButtonInfo != value)
                {
                    _exitVoiceModeButtonInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        public void LoadFromSettings(AppSettings settings)
        {
            WakeWord = settings.WakeWord;
            TargetMonitorCenter = settings.TargetMonitorCenter;
            IsMonitorConfigured = settings.IsMonitorConfigured;
            ButtonClickDelay = settings.ButtonClickDelayMs;
            ConfidenceThreshold = settings.SpeechConfidenceThreshold;
            ChatGptAppPath = settings.ChatGptAppPath;
            NewChatButtonText = settings.NewChatButtonText;
            VoiceModeButtonText = settings.VoiceModeButtonText;
            NewChatButtonPosition = settings.NewChatButtonPosition;
            IsNewChatButtonConfigured = settings.IsNewChatButtonConfigured;
            VoiceModeButtonPosition = settings.VoiceModeButtonPosition;
            IsVoiceModeButtonConfigured = settings.IsVoiceModeButtonConfigured;
            EnableWakeWordIsolation = settings.EnableWakeWordIsolation;
            SilenceThreshold = settings.SilenceThreshold;
            MinimumSilenceDurationMs = settings.MinimumSilenceDurationMs;
            CooldownPeriodMs = settings.CooldownPeriodMs;
            StartWithWindows = settings.StartWithWindows;
            StartMinimized = settings.StartMinimized;
            MicButtonPosition = settings.MicButtonPosition;
            IsMicButtonConfigured = settings.IsMicButtonConfigured;
            ExitVoiceModeButtonPosition = settings.ExitVoiceModeButtonPosition;
            IsExitVoiceModeButtonConfigured = settings.IsExitVoiceModeButtonConfigured;

            if (IsMonitorConfigured)
            {
                UpdateMonitorInfo();
            }

            UpdateButtonInfo();
        }

        public AppSettings ToSettings()
        {
            return new AppSettings
            {
                WakeWord = WakeWord,
                TargetMonitorCenter = TargetMonitorCenter,
                IsMonitorConfigured = IsMonitorConfigured,
                ButtonClickDelayMs = ButtonClickDelay,
                SpeechConfidenceThreshold = ConfidenceThreshold,
                ChatGptAppPath = ChatGptAppPath,
                NewChatButtonText = NewChatButtonText,
                VoiceModeButtonText = VoiceModeButtonText,
                NewChatButtonPosition = NewChatButtonPosition,
                IsNewChatButtonConfigured = IsNewChatButtonConfigured,
                VoiceModeButtonPosition = VoiceModeButtonPosition,
                IsVoiceModeButtonConfigured = IsVoiceModeButtonConfigured,
                EnableWakeWordIsolation = EnableWakeWordIsolation,
                SilenceThreshold = SilenceThreshold,
                MinimumSilenceDurationMs = MinimumSilenceDurationMs,
                CooldownPeriodMs = CooldownPeriodMs,
                StartWithWindows = StartWithWindows,
                StartMinimized = StartMinimized,
                MicButtonPosition = MicButtonPosition,
                IsMicButtonConfigured = IsMicButtonConfigured,
                ExitVoiceModeButtonPosition = ExitVoiceModeButtonPosition,
                IsExitVoiceModeButtonConfigured = IsExitVoiceModeButtonConfigured
            };
        }

        public void UpdateMonitorInfo()
        {
            if (IsMonitorConfigured && TargetMonitorCenter != Point.Empty)
            {
                var screen = System.Windows.Forms.Screen.FromPoint(TargetMonitorCenter);
                int monitorIndex = Array.IndexOf(System.Windows.Forms.Screen.AllScreens, screen) + 1;
                MonitorInfo = $"Monitor {monitorIndex} ({screen.Bounds.Width}x{screen.Bounds.Height})";
            }
            else
            {
                MonitorInfo = "Not configured";
            }
        }

        public void UpdateButtonInfo()
        {
            if (IsNewChatButtonConfigured && NewChatButtonPosition != Point.Empty)
            {
                NewChatButtonInfo = $"Configured ({NewChatButtonPosition.X}, {NewChatButtonPosition.Y})";
            }
            else
            {
                NewChatButtonInfo = "Not configured";
            }

            if (IsVoiceModeButtonConfigured && VoiceModeButtonPosition != Point.Empty)
            {
                VoiceModeButtonInfo = $"Configured ({VoiceModeButtonPosition.X}, {VoiceModeButtonPosition.Y})";
            }
            else
            {
                VoiceModeButtonInfo = "Not configured";
            }

            if (IsMicButtonConfigured && MicButtonPosition != Point.Empty)
            {
                MicButtonInfo = $"Configured ({MicButtonPosition.X}, {MicButtonPosition.Y})";
            }
            else
            {
                MicButtonInfo = "Not configured";
            }

            if (IsExitVoiceModeButtonConfigured && ExitVoiceModeButtonPosition != Point.Empty)
            {
                ExitVoiceModeButtonInfo = $"Configured ({ExitVoiceModeButtonPosition.X}, {ExitVoiceModeButtonPosition.Y})";
            }
            else
            {
                ExitVoiceModeButtonInfo = "Not configured";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
