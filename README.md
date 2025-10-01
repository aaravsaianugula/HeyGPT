# HeyGPT - Voice-Activated ChatGPT Assistant

<div align="center">

![HeyGPT Logo](Assets/Logo.png)

A modern, voice-activated desktop assistant for Windows that launches and controls ChatGPT using wake word detection.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Windows](https://img.shields.io/badge/Platform-Windows-0078D6?logo=windows)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

</div>

## ✨ Features

### 🎤 Voice Recognition
- **Wake Word Detection** - Activate with customizable wake word (default: "Hey GPT")
- **Advanced Isolation** - Prevents false positives with silence detection and cooldown periods
- **Configurable Confidence** - Adjust speech recognition sensitivity

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
1. Download the latest release from [Releases](../../releases)
2. Extract the ZIP file
3. Run `HeyGPT.exe`

### Option 2: Build from Source
```bash
# Clone the repository
git clone https://github.com/yourusername/HeyGPT.git
cd HeyGPT

# Build the project
cd ChatGptVoiceAssistant
dotnet restore
dotnet build --configuration Release

# Run
dotnet run
```

## 📖 Quick Start Guide

### 1. Initial Setup
1. Launch HeyGPT
2. Click **Settings** (⚙ button)
3. Configure your target monitor:
   - Click "Configure Monitor (10s countdown)"
   - Move mouse to center of desired monitor
   - Wait for countdown to finish

### 2. Configure Wake Word (Optional)
- Set custom wake word (default: "Hey GPT")
- Adjust confidence threshold (0.0 - 1.0)
- Enable/disable wake word isolation

### 3. Configure Button Positions (Optional)
For improved reliability, capture exact button positions:
- **New Chat Button**: Position of "New chat" button in ChatGPT
- **Voice Mode Button**: Position of voice mode button
- Uses 10-second countdown capture technology

### 4. Test Your Setup
1. Click **Test** button on main window
2. Verify ChatGPT launches on correct monitor
3. Confirm voice mode activates (blue orb detection)

### 5. Start Using
1. Click **Start Listening**
2. Say your wake word: "Hey GPT"
3. ChatGPT launches automatically in voice mode!

## 🎯 Usage Tips

### Wake Word Isolation Settings
- **Silence Threshold**: Audio level below which is considered silence (0-30)
- **Minimum Silence Duration**: Required silence before wake word (ms)
- **Cooldown Period**: Time between wake word detections (ms)

### Environment Presets
Adjust settings based on your environment:
- **Quiet Office**: Higher sensitivity, shorter cooldown
- **Noisy Environment**: Lower sensitivity, longer cooldown, higher silence threshold
- **Shared Space**: Enable isolation, increase cooldown

### Troubleshooting
- **Wake word not detected**: Increase confidence threshold, check microphone
- **False positives**: Enable isolation, increase silence threshold
- **ChatGPT doesn't launch**: Verify "chatgpt" command works in terminal
- **Buttons not clicked**: Reconfigure button positions via Settings

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
- **System.Speech**: Windows speech recognition
- **Windows.Media.Ocr**: Text detection for button finding
- **Win32 APIs**: Window manipulation and mouse control
- **MVVM Pattern**: Clean separation of concerns

### Core Features Implementation
- **Wake Word Detection**: `SpeechRecognitionService.cs` with isolation logic
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
git clone https://github.com/yourusername/HeyGPT.git
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

## 📞 Support

- **Issues**: [GitHub Issues](../../issues)
- **Discussions**: [GitHub Discussions](../../discussions)

## 🗺️ Roadmap

- [ ] Multi-language support (Korean, Japanese)
- [ ] Custom wake word training
- [ ] Plugin system for other AI assistants
- [ ] Voice command recognition beyond wake word
- [ ] Hotkey activation alternative
- [ ] MacOS/Linux support (future)

---

<div align="center">

Made with ❤️ using .NET and WPF

⭐ Star this repo if you find it useful!

</div>
