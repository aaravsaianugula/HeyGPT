# HeyGPT - Voice-Activated ChatGPT Assistant

<div align="center">

![HeyGPT Logo](Assets/Logo.png)

A modern, voice-activated desktop assistant for Windows that launches and controls ChatGPT using wake word detection.

[![Release](https://img.shields.io/badge/Release-v1.0.0-blue.svg)](https://github.com/aaravsaianugula/HeyGPT/releases/latest)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Windows](https://img.shields.io/badge/Platform-Windows-0078D6?logo=windows)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

</div>

## ✨ Features

### 🎤 Voice Recognition
- **Porcupine High Accuracy Mode** - Industry-standard wake word detection with 97%+ accuracy and <1 false positive per 10 hours
- **Dual Wake Word Engines** - Choose between Porcupine (high accuracy) or System.Speech (basic)
- **Voice Commands** - Control ChatGPT in voice mode: "mic on/off", "exit voice mode"
- **Strict Isolation Technology** - Advanced filtering prevents false triggers when wake word is embedded in sentences (System.Speech only)
- **Post-Recognition Validation** - 400ms confirmation period ensures wake word was spoken in isolation (System.Speech only)
- **Continuous Listening Mode** - Keeps listening after each detection, no need to restart
- **Configurable Sensitivity** - Adjust detection sensitivity for your environment

### 🖥️ Window Automation
- **Multi-Monitor Support** - Launch ChatGPT on any monitor
- **Smart Positioning** - Automatically positions and maximizes ChatGPT window
- **Button Detection** - OCR-based and position-based button clicking
- **Blue Orb Verification** - Confirms voice mode activation visually

### 🎨 Modern UI
- **Retro Japanese Aesthetic** - Inspired by 1950s-60s design with cherry blossoms
- **Fluid Animations** - Smooth entrance animations and button transitions
- **Responsive Design** - Scale and shadow effects on all interactive elements
- **System Tray Integration** - Minimize to tray for background operation

### ⚙️ Configuration
- **Position Capture Technology** - 10-second countdown to capture exact screen positions
- **Auto-Start** - Launch with Windows startup
- **Start Minimized** - Run silently in background
- **Customizable Delays** - Adjust automation timing for your system

## 📋 Requirements

- **OS**: Windows 10/11 (version 10.0.17763.0 or higher)
- **Runtime**: [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
- **ChatGPT**: [ChatGPT Desktop App](https://openai.com/chatgpt/desktop/) installed
- **Microphone**: Any working microphone for voice input

## 🚀 Installation

### Option 1: Download Release (Recommended)
1. Download **[HeyGPT v1.0.0](https://github.com/aaravsaianugula/HeyGPT/releases/tag/v1.0.0)** (9.6 MB)
2. Extract `HeyGPT-v1.0.0-Windows.zip` to a permanent location (e.g., `C:\Program Files\HeyGPT`)
3. **Important**: Keep all files together - don't move just the `.exe`
4. Run `HeyGPT.exe` from the extracted folder
5. (Optional) Right-click `HeyGPT.exe` → **Send to** → **Desktop (create shortcut)**
6. Configure monitor in Settings
7. Click "Start Listening" and say "Hey GPT"!

**Note**: The first launch may take a few seconds while Windows loads .NET runtime.

### Option 2: Build from Source
```bash
# Clone the repository
git clone https://github.com/aaravsaianugula/HeyGPT.git
cd HeyGPT

# Build the project
cd ChatGptVoiceAssistant
dotnet restore
dotnet build --configuration Release

# Run
dotnet run
```

## 🎯 How to Launch HeyGPT

After installation, you can launch HeyGPT using any of these methods:

1. **Windows Search**: Press `Win` key, type "HeyGPT", press Enter
2. **Desktop Shortcut**: Double-click the HeyGPT icon (if created)
3. **Direct Launch**: Navigate to installation folder and run `HeyGPT.exe`
4. **Auto-Start**: Enable in Settings to launch with Windows

## 📖 Quick Start Guide

### 1. Initial Setup
1. Launch HeyGPT
2. Click **Settings** (⚙ button)
3. Configure your target monitor:
   - Click "Configure Monitor (10s countdown)"
   - Move mouse to center of desired monitor
   - Wait for countdown to finish

### 2. Enable Porcupine High Accuracy Mode (Recommended)
To fix false positive issues with wake word detection:
1. Get a FREE Picovoice AccessKey from [console.picovoice.ai](https://console.picovoice.ai)
   - No credit card required
   - Takes 30 seconds to sign up
   - Free for personal use forever
2. In Settings, scroll to "🎯 Porcupine High Accuracy Mode"
3. Check "Enable Porcupine Wake Word Detection"
4. Paste your AccessKey
5. Adjust sensitivity if needed (default 0.5 works great)
6. Save settings

**Why Porcupine?** 97%+ accuracy with <1 false positive per 10 hours vs. System.Speech which has frequent false positives.

### 3. Configure Wake Word (Optional)
- Set custom wake word (default: "Hey GPT")
- Adjust confidence threshold (0.0 - 1.0) for System.Speech
- Enable/disable wake word isolation for System.Speech

### 4. Configure Button Positions (Optional)
For improved reliability, capture exact button positions:
- **New Chat Button**: Position of "New chat" button in ChatGPT
- **Voice Mode Button**: Position of voice mode button
- Uses 10-second countdown capture technology

### 5. Test Your Setup
1. Click **Test** button on main window
2. Verify ChatGPT launches on correct monitor
3. Confirm voice mode activates (blue orb detection)

### 6. Start Using
1. Click **Start Listening**
2. Say your wake word clearly (if using Porcupine, it will be detected automatically)
3. ChatGPT launches automatically in voice mode!
4. App continues listening - say wake word again anytime to launch another window
5. Click **Stop** when you're done

### 7. Voice Commands (In Voice Mode)
Once ChatGPT is in voice mode (blue orb visible), use voice commands to control it:

**Available Commands:**
- **"Mic On"** / **"Mic Off"** - Toggle microphone on/off in ChatGPT
- **"Exit"** / **"Exit Voice Mode"** - Exit voice mode and return to text chat

**Setup Voice Commands:**
1. Launch ChatGPT and enter voice mode manually
2. In HeyGPT Settings, scroll to "Voice Command Buttons"
3. Click "Capture Mic Button" and hover over the mic button in ChatGPT
4. Click "Capture Exit Voice Mode Button" and hover over the exit button
5. Save settings

**Important:** Voice commands ONLY work when ChatGPT is in voice mode (blue orb detection). The app monitors voice mode status every 2 seconds and enables/disables commands accordingly.

## 🎯 Usage Tips

### How Wake Word Isolation Works
HeyGPT uses **advanced isolation technology** to ensure wake words are only detected when spoken alone:

**Pre-Recognition Checks:**
- Maximum 5 speech hypotheses (rejects if part of longer sentence)
- Maximum 3 unique words (rejects if conversation detected)
- Maximum 2 second speech duration
- All words must be part of wake word (rejects "Hey what's up GPT")

**Post-Recognition Validation (400ms):**
- Monitors for continued speech after wake word
- Rejects if any additional words detected
- Ensures silence follows the wake word

**Result:** ✅ "Hey GPT" triggers, ❌ "Hey GPT can you..." rejects

### Wake Word Isolation Settings
- **Enable Wake Word Isolation**: Turn on/off strict filtering (recommended: ON)
- **Cooldown Period**: Time between detections (default: 1.5s)
- **Confidence Threshold**: Speech recognition confidence level (default: 0.7)

### Environment Presets
Adjust settings based on your environment:
- **Quiet Office**: Enable isolation, default settings work well
- **Noisy Environment**: Increase confidence threshold to 0.8+
- **Shared Space**: Enable isolation to prevent accidental triggers

### Troubleshooting

#### Launch Issues
- **"Windows is searching for python.exe" error**: This means you have a broken shortcut. Delete the shortcut and recreate it by right-clicking `HeyGPT.exe` → **Send to** → **Desktop (create shortcut)**
- **App doesn't appear in Windows Search**: Windows may need time to index. Try launching directly from the folder or restart Windows
- **Missing .dll errors**: Extract the entire zip file contents together - don't move just the `.exe` file
- **.NET runtime errors**: Install [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

#### Voice Recognition Issues
- **Frequent false positives**: Enable Porcupine High Accuracy Mode in Settings (97%+ accuracy vs System.Speech frequent false alarms)
- **Wake word not detected with Porcupine**: Verify AccessKey is valid, check microphone permissions, speak clearly
- **Wake word not detected with System.Speech**: Check microphone permissions, speak wake word clearly and alone (not in a sentence)
- **"Speech continued after wake word" rejections** (System.Speech): Pause briefly after saying wake word, isolation requires silence
- **"Invalid AccessKey" error**: Get a fresh key from https://console.picovoice.ai - takes 30 seconds, free for personal use

#### ChatGPT Integration Issues
- **ChatGPT doesn't launch**: Verify ChatGPT Desktop App is installed and "chatgpt" command works in PowerShell/CMD
- **Buttons not clicked**: Reconfigure button positions via Settings using the 10-second countdown capture
- **Blue orb not detected**: ChatGPT window layout may have changed; reconfigure voice mode button position

#### General Issues
- **App stops listening**: Now includes auto-restart; check Activity Log if issues persist
- **Settings not saving**: Check write permissions for `%APPDATA%\HeyGPT\` folder
- **App minimizes unexpectedly**: Check "Start Minimized" setting in Settings window

## 🏗️ Architecture

### Project Structure
```
HeyGPT/
├── Assets/                 # Application icons and images
│   ├── Logo.png           # Circular retro logo
│   └── SqaureLog.ico      # Square icon for taskbar
├── ChatGptVoiceAssistant/
│   ├── Models/            # Data models
│   │   └── AppSettings.cs
│   ├── Services/          # Core services
│   │   ├── SpeechRecognitionService.cs
│   │   ├── WindowAutomationService.cs
│   │   ├── ScreenshotService.cs
│   │   ├── OcrService.cs
│   │   ├── ImageRecognitionService.cs
│   │   ├── MonitorService.cs
│   │   ├── SettingsService.cs
│   │   └── StartupService.cs
│   ├── ViewModels/        # MVVM view models
│   │   └── SettingsViewModel.cs
│   ├── Views/             # WPF windows
│   │   ├── MainWindow.xaml
│   │   ├── SettingsWindow.xaml
│   │   └── UserGuideWindow.xaml
│   ├── Resources/         # UI themes and styles
│   │   └── EastAsianTheme.xaml
│   └── App.xaml           # Application entry point
└── README.md
```

### Key Technologies
- **WPF (Windows Presentation Foundation)**: Modern UI framework
- **Porcupine by Picovoice**: Industry-standard wake word detection (97%+ accuracy)
- **NAudio**: Audio capture for wake word processing
- **System.Speech**: Windows speech recognition (fallback + voice commands)
- **Windows.Media.Ocr**: Text detection for button finding
- **Win32 APIs**: Window manipulation and mouse control
- **MVVM Pattern**: Clean separation of concerns

### Core Features Implementation
- **Wake Word Detection (Porcupine)**: `PorcupineWakeWordService.cs` with high accuracy neural network
- **Wake Word Detection (Fallback)**: `SpeechRecognitionService.cs` with isolation logic
- **Voice Commands**: `SpeechRecognitionService.cs` for in-voice-mode commands
- **Position Capture**: `MonitorService.cs` with 10-second countdown
- **Blue Orb Detection**: `ImageRecognitionService.cs` with color analysis
- **Window Automation**: `WindowAutomationService.cs` with Win32 APIs
- **Settings Persistence**: JSON serialization to `%APPDATA%/HeyGPT/`

## 🎨 Theming

The app uses a retro Japanese-inspired theme:
- **Colors**: Turquoise, coral, navy, cream (1950s-60s palette)
- **Typography**: Clean, modern fonts with traditional Japanese text
- **Animations**: Smooth cubic easing functions (0.2-0.5s transitions)
- **Design Philosophy**: Wabi-sabi minimalism meets retro modernism

## 🔒 Privacy & Security

- **No Data Collection**: All processing happens locally on your machine
- **No Network Calls**: Except launching ChatGPT desktop app
- **Settings Storage**: Stored locally in `%APPDATA%\HeyGPT\settings.json`
- **Open Source**: Full code available for review

## 🤝 Contributing

Contributions are welcome! Here's how:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Setup
```bash
# Prerequisites
- Visual Studio 2022 or VS Code with C# extension
- .NET 8.0 SDK

# Clone and build
git clone https://github.com/aaravsaianugula/HeyGPT.git
cd HeyGPT/ChatGptVoiceAssistant
dotnet restore
dotnet build
```

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Logo design inspired by 1950s-60s retro Japanese aesthetics
- Wake word isolation techniques from speech recognition research
- ChatGPT Desktop App by OpenAI

## 📝 Changelog

### v1.0.0 (2025-10-01)
**First Official Release** 🎉

**🎤 Voice Recognition Improvements:**
- Implemented strict wake word isolation with 5-level filtering
- Added post-recognition validation (400ms confirmation period)
- Prevents false triggers when wake word is embedded in sentences
- Continuous listening mode - no need to restart after each detection
- Reduced default cooldown from 2.5s to 1.5s

**🖥️ Window Automation:**
- Multi-monitor support with position capture
- OCR-based and position-based button clicking
- Blue orb verification for voice mode
- Automatic window positioning and maximization

**🎨 UI/UX:**
- Retro Japanese aesthetic with fluid animations
- System tray integration
- Auto-start with Windows capability
- Start minimized option

**🔧 Technical:**
- Full .NET 8.0 WPF implementation
- MVVM architecture
- Local settings persistence
- Comprehensive activity logging

## 📞 Support

- **Issues**: [GitHub Issues](../../issues)
- **Discussions**: [GitHub Discussions](../../discussions)

## 🗺️ Roadmap

- [ ] Multi-language support (Korean, Japanese)
- [ ] Custom wake word training
- [ ] Plugin system for other AI assistants
- [ ] Hotkey activation alternative
- [ ] MacOS/Linux support (future)

---

<div align="center">

Made with ❤️ using .NET and WPF

⭐ Star this repo if you find it useful!

</div>
