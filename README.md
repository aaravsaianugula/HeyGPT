# HeyGPT - Voice-Activated ChatGPT Assistant

<div align="center">

![HeyGPT Logo](Assets/Logo.png)

**A modern, voice-activated desktop assistant for Windows that launches and controls ChatGPT using wake word detection.**

[![Release](https://img.shields.io/badge/Release-v1.1.0-blue.svg)](https://github.com/aaravsaianugula/HeyGPT/releases/latest)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Windows](https://img.shields.io/badge/Platform-Windows-0078D6?logo=windows)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

**[📥 Download](https://github.com/aaravsaianugula/HeyGPT/releases/latest) | [📖 User Guide](USER_GUIDE.md) | [🐛 Report Issue](https://github.com/aaravsaianugula/HeyGPT/issues)**

---

</div>

> **Just say your wake word and ChatGPT opens in voice mode - hands-free, fast, and accurate!**
>
> 🎯 **97%+ accuracy** with Porcupine AI • 🎤 **14 built-in wake words** + custom support • 🚀 **One-click install**

## ✨ Features

### 🎤 Voice Recognition
- **Porcupine High Accuracy Mode** - Industry-standard wake word detection with 97%+ accuracy and <1 false positive per 10 hours
- **14 Built-in Wake Words** - Jarvis, Alexa, Computer, Hey Google, Hey Siri, OK Google, Picovoice, Porcupine, Bumblebee, Terminator, Americano, Blueberry, Grapefruit, Grasshopper
- **Custom Wake Words** - Upload your own custom wake word `.ppn` files created at [Picovoice Console](https://console.picovoice.ai)
- **Dual Wake Word Engines** - Auto-detect Porcupine (high accuracy) when AccessKey provided, fallback to System.Speech
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

Before installing HeyGPT, make sure you have:

- ✅ **Windows 10/11** (version 10.0.17763.0 or higher)
- ✅ **[.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)** (free download from Microsoft)
- ✅ **[ChatGPT Desktop App](https://openai.com/chatgpt/desktop/)** (free download from OpenAI)
- ✅ **Working microphone** for voice input

**Optional but Recommended:**
- 🎯 **[Picovoice Account](https://console.picovoice.ai)** (free, no credit card) for 97%+ accuracy wake word detection

## 🚀 Installation

### Option 1: Windows Installer (Recommended)
1. Download **[HeyGPT-v1.1.0-Setup.exe](https://github.com/aaravsaianugula/HeyGPT/releases/tag/v1.1.0)** (11 MB)
2. Run the installer - it will install to `C:\Program Files\HeyGPT`
3. Creates Start Menu shortcuts automatically
4. **Can be uninstalled** from Windows Settings → Apps & Features → HeyGPT
5. Configure monitor in Settings
6. Click "Start Listening" and say "Hey GPT"!

**Benefits**: Automatic shortcuts, clean uninstall, integrates with Windows properly.

### Option 2: Portable ZIP Version
1. Download **[HeyGPT-v1.1.0-Windows.zip](https://github.com/aaravsaianugula/HeyGPT/releases/tag/v1.1.0)** (13 MB)
2. Extract to a permanent location (e.g., `C:\Program Files\HeyGPT`)
3. **Important**: Keep all files together - don't move just the `.exe`
4. Run `HeyGPT.exe` from the extracted folder
5. (Optional) Right-click `HeyGPT.exe` → **Send to** → **Desktop (create shortcut)**

**Note**: The first launch may take a few seconds while Windows loads .NET runtime.

### Option 3: Build from Source
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

## 📖 Quick Start (5 Minutes)

**[📘 Full User Guide](USER_GUIDE.md)** | **[💬 Get Help](https://github.com/aaravsaianugula/HeyGPT/issues)**

### 1. Get Porcupine AccessKey (30 sec)
1. Visit [console.picovoice.ai](https://console.picovoice.ai)
2. Sign up (free, no credit card)
3. Copy your AccessKey

### 2. Configure (2 min)
1. **Launch** → Windows Search → "HeyGPT"
2. **Settings** → Paste AccessKey in "🎯 Porcupine AI"
3. **Choose wake word** → `jarvis`, `alexa`, `computer`, etc.
4. **Configure Monitor** → Click button → Move mouse to screen center
5. **Save**

### 3. Test & Use (2 min)
1. Click **🧪 Test** → ChatGPT should launch on your monitor
2. Click **▶ Start Listening**
3. Say your wake word (e.g., "Jarvis")
4. ChatGPT launches in voice mode! ✨

---

## 🎨 Custom Wake Words (Beyond the 14 Built-Ins)

**Want "Hey Claude", "Okay Buddy", or any other wake word?**

### Quick Process (10-15 minutes):

1. **Sign up** at [console.picovoice.ai](https://console.picovoice.ai) (free, no credit card)
2. **Train keyword** → "Porcupine Wake Word" → "Train New Keyword" → Enter your wake word → Wait ~5-10 min
3. **Download .ppn** → Select Platform: **Windows** → Download file
4. **Upload to HeyGPT** → Settings → "🎨 Custom Wake Word" → Browse → Select .ppn file → Save

**📖 [Complete Step-by-Step Guide with Screenshots](USER_GUIDE.md#how-to-create-custom-wake-words-step-by-step)**

### Important Notes:

✅ **Built-in wake words** (jarvis, alexa, computer, etc.) work immediately - no .ppn needed
❌ **Custom wake words** REQUIRE .ppn file - you can't just type random text
⚠️ Must select **Windows** platform when downloading (not Mac/Linux)
⏱️ Training takes ~5-10 minutes (you'll get email when done)

### Troubleshooting:

**"File not found" error?** → Make sure you downloaded **Windows** .ppn (not Mac/Linux)
**Wake word not detected?** → Verify AccessKey is entered, speak clearly, adjust sensitivity
**Training failed?** → Use 2-3 words, avoid special characters, try different phrase

---

### Optional: Voice Commands
Setup in Settings to use "Mic On", "Mic Off", "Exit" commands in voice mode

## 💡 Tips for Best Results

✅ **Use Porcupine** - 97%+ accuracy, <1 false positive/10hrs
✅ **Speak clearly** - Normal volume, clear pronunciation
✅ **Built-in wake words** - Jarvis, Computer, Alexa work best
✅ **Keep mic close** - Within 3 feet for best detection
✅ **Minimize noise** - Quiet environment helps accuracy

**Skip Porcupine?** System.Speech works but has frequent false positives. Enable "Wake Word Isolation" in Advanced Settings if using fallback mode.

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

#### Uninstallation
- **Installed via installer**: Go to Windows Settings → Apps & Features → Search "HeyGPT" → Click Uninstall
- **Portable ZIP version**: Simply delete the HeyGPT folder. Settings are stored in `%APPDATA%\HeyGPT\` - delete this folder too if desired

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
│   │   ├── PorcupineWakeWordService.cs
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

HeyGPT respects your privacy:

- ✅ **100% Local Processing**: All wake word detection happens on your device (no audio sent to cloud)
- ✅ **No Telemetry**: Zero tracking, analytics, or data collection
- ✅ **Open Source**: Full source code available for review on GitHub
- ✅ **Local Settings**: All configuration stored locally in `%APPDATA%\HeyGPT\settings.json`
- ✅ **Optional Cloud**: Picovoice AccessKey is only used for wake word validation (no audio sent)

**Note**: ChatGPT itself requires internet and follows OpenAI's privacy policy.

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

### v1.1.0 (Latest)
**Major Update: Porcupine High Accuracy + Custom Wake Words** 🎉

**🎯 Major Features:**
- ✨ **Porcupine AI Wake Word Engine**: Industry-standard detection with 97%+ accuracy
- 🎨 **Custom Wake Words**: Upload your own `.ppn` files created at Picovoice Console
- 🎙 **14 Built-in Wake Words**: Jarvis, Alexa, Computer, Hey Google, Hey Siri, OK Google, Picovoice, Porcupine, Bumblebee, Terminator, Americano, Blueberry, Grapefruit, Grasshopper
- 🎤 **Voice Commands**: "mic on/off", "exit voice mode" (works in ChatGPT voice mode)
- 👁 **Blue Orb Detection**: Automatic voice mode tracking
- 📦 **Windows Installer**: Professional installer with full uninstall support
- 📖 **Comprehensive User Guide**: Complete documentation for all features

**🔧 Improvements:**
- Dramatically improved wake word accuracy (97%+ vs. 60-70% with System.Speech)
- <1 false positive per 10 hours (vs. frequent false alarms with System.Speech)
- Auto-detect Porcupine when AccessKey is provided
- Simplified Settings UI - removed confusing dual-engine checkbox
- Better validation and error messages in Settings
- Enhanced logging to show which wake word is active

**📦 What's Included:**
- Porcupine 3.0.10 for wake word detection
- NAudio 2.2.1 for audio capture
- Inno Setup installer for professional Windows installation
- Complete user guide (USER_GUIDE.md)

**🐛 Bug Fixes:**
- Fixed icon path errors on first launch
- Fixed XAML resource ordering issues
- Fixed wake word hardcoded to "COMPUTER" instead of user's choice
- Improved file path validation for custom wake words

**📖 Documentation:**
- Updated README with Porcupine setup instructions
- Added Porcupine troubleshooting guide
- Created comprehensive in-app user guide
- Added RELEASE_NOTES.txt to distribution package

**💿 Distribution:**
- Added Windows Installer (HeyGPT-v1.1.0-Setup.exe) for easy installation
- Full uninstall support via Windows Settings → Apps & Features
- Automatic Start Menu shortcuts
- Optional Windows startup integration

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
