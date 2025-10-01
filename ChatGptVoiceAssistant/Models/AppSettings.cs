using System;
using System.Drawing;

namespace HeyGPT.Models
{
    public class AppSettings
    {
        public string WakeWord { get; set; } = "Hey GPT";
        public Point TargetMonitorCenter { get; set; } = Point.Empty;
        public bool IsMonitorConfigured { get; set; } = false;
        public string ChatGptAppPath { get; set; } = "chatgpt";
        public int ButtonClickDelayMs { get; set; } = 1000;
        public float SpeechConfidenceThreshold { get; set; } = 0.7f;
        public string NewChatButtonText { get; set; } = "New chat";
        public string VoiceModeButtonText { get; set; } = "Voice";
        public Point NewChatButtonPosition { get; set; } = Point.Empty;
        public bool IsNewChatButtonConfigured { get; set; } = false;
        public Point VoiceModeButtonPosition { get; set; } = Point.Empty;
        public bool IsVoiceModeButtonConfigured { get; set; } = false;
        public bool EnableWakeWordIsolation { get; set; } = true;
        public int SilenceThreshold { get; set; } = 10;
        public int MinimumSilenceDurationMs { get; set; } = 800;
        public int CooldownPeriodMs { get; set; } = 2500;
        public bool StartWithWindows { get; set; } = false;
        public bool StartMinimized { get; set; } = false;
    }
}
