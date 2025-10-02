# HeyGPT User Guide

Welcome to HeyGPT! This guide will help you set up and use your voice-activated ChatGPT assistant.

## Table of Contents

1. [First-Time Setup](#first-time-setup)
2. [Porcupine Setup (Recommended)](#porcupine-setup-recommended)
3. [Basic Usage](#basic-usage)
4. [Advanced Configuration](#advanced-configuration)
5. [Voice Commands](#voice-commands)
6. [Troubleshooting](#troubleshooting)
7. [FAQ](#faq)

---

## First-Time Setup

### Prerequisites

Before you begin, make sure you have:

1. ✅ **Windows 10/11** (version 10.0.17763.0 or higher)
2. ✅ **ChatGPT Desktop App** installed ([Download here](https://openai.com/chatgpt/desktop/))
3. ✅ **.NET 8.0 Runtime** installed ([Download here](https://dotnet.microsoft.com/download/dotnet/8.0))
4. ✅ **Working microphone** connected and enabled

### Step 1: Install HeyGPT

**Option A: Windows Installer (Recommended)**
1. Download `HeyGPT-v1.1.0-Setup.exe` from the [Releases page](https://github.com/aaravsaianugula/HeyGPT/releases)
2. Run the installer
3. Follow the installation wizard
4. HeyGPT will be installed to `C:\Program Files\HeyGPT`
5. Shortcuts will be added to your Start Menu

**Option B: Portable ZIP**
1. Download `HeyGPT-v1.1.0-Windows.zip` from the [Releases page](https://github.com/aaravsaianugula/HeyGPT/releases)
2. Extract to a permanent location (e.g., `C:\Program Files\HeyGPT`)
3. Keep all files together - don't move just the .exe
4. Run `HeyGPT.exe`

### Step 2: Initial Configuration

1. **Launch HeyGPT** (search "HeyGPT" in Windows search)
2. Click the **⚙ Settings** button
3. **Configure Monitor**:
   - Click "Configure Monitor (10s countdown)"
   - Move your mouse to the center of the monitor where you want ChatGPT to appear
   - Wait for the countdown to finish
   - You should see "Monitor configured successfully!"

4. **Set Wake Word** (optional):
   - Default is "Hey GPT"
   - You can change it to anything you like (e.g., "Jarvis", "Computer", "Alexa")
   - Built-in Porcupine wake words: `Jarvis`, `Alexa`, `Computer`, `Hey Google`, `Hey Siri`, `OK Google`, `Picovoice`, `Porcupine`, `Bumblebee`, `Terminator`, `Americano`, `Blueberry`, `Grapefruit`, `Grasshopper`

5. Click **Save Settings**

### Step 3: Test Your Setup

1. Click the **🧪 Test** button on the main window
2. ChatGPT should launch on your configured monitor
3. The window should maximize automatically
4. If it works, you're ready to go!

---

## Porcupine Setup (Recommended)

**Why Porcupine?**
- 97%+ accuracy vs. System.Speech's ~60-70%
- <1 false positive per 10 hours vs. System.Speech's frequent false alarms
- Works in noisy environments
- No additional configuration needed

### How to Enable Porcupine

1. **Get a FREE AccessKey**:
   - Visit [console.picovoice.ai](https://console.picovoice.ai)
   - Sign up (takes 30 seconds, no credit card required)
   - Copy your AccessKey from the dashboard

2. **Enter AccessKey in HeyGPT**:
   - Open HeyGPT Settings (⚙ button)
   - Scroll to "🎯 Porcupine AI Wake Word Engine"
   - Paste your AccessKey
   - Adjust sensitivity if needed (default 0.5 is perfect for most users)
   - Click Save

3. **That's it!** HeyGPT will automatically use Porcupine for wake word detection.

### Custom Wake Words (Advanced)

Want to use a wake word beyond the 14 built-in options?

1. Visit [console.picovoice.ai](https://console.picovoice.ai)
2. Go to "Porcupine Wake Word" → "Create Custom Wake Word"
3. Train your custom wake word (follow their wizard)
4. Download the `.ppn` file for **Windows**
5. In HeyGPT Settings, under "Custom Wake Word (.ppn file)", click **Browse...**
6. Select your downloaded `.ppn` file
7. Click Save

Your custom wake word will now be used instead of the built-in one!

---

## Basic Usage

### Starting HeyGPT

1. **Launch the app** (Windows Search → "HeyGPT")
2. Click **▶ Start Listening**
3. Say your wake word clearly (e.g., "Hey GPT")
4. ChatGPT launches automatically in voice mode!
5. The app keeps listening - say the wake word again anytime

### Stopping HeyGPT

1. Click **⏸ Stop Listening**
2. Or close the app

### System Tray Mode

- Click the **X** button to minimize to system tray
- Right-click the tray icon to restore or quit
- Enable "Start minimized to system tray" in Settings for background operation

---

## Advanced Configuration

### Button Positions

For improved reliability, you can capture exact button positions:

#### New Chat Button
1. Launch ChatGPT manually
2. Open HeyGPT Settings
3. Scroll to "🎯 Button Positions"
4. Click "Capture New Chat Button (10s countdown)"
5. Hover your mouse over the "New chat" button in ChatGPT
6. Wait for the countdown to finish

#### Voice Mode Button
1. Same process as above
2. Click "Capture Voice Mode Button (10s countdown)"
3. Hover over the voice mode button in ChatGPT

### ChatGPT Application Settings

- **Application Path**: Default is `chatgpt` (Windows PATH)
  - If ChatGPT isn't in your PATH, provide full path: `C:\Users\YourName\AppData\Local\Programs\ChatGPT\ChatGPT.exe`
- **Button Click Delay**: Time to wait between automation actions (default 1000ms)
  - Increase if your system is slow
  - Decrease for faster automation
- **Button Text**: Exact text shown on buttons in your ChatGPT app
  - Usually "New chat" and "Voice" (or your language equivalent)

### Wake Word Isolation (System.Speech Only)

If NOT using Porcupine, these settings help prevent false positives:

- **Enable Wake Word Isolation**: Ensures wake word is spoken alone, not in a sentence
- **Silence Threshold**: Audio level considered silence (0-30)
- **Minimum Silence Duration**: Silence required before wake word (500-1500ms recommended)
- **Cooldown Period**: Time between wake word activations (2000-5000ms recommended)
- **Confidence Threshold**: Speech recognition confidence (0.6-0.8 recommended)

---

## Voice Commands

Voice commands let you control ChatGPT while in voice mode (blue orb visible).

### Available Commands

| Command | Action |
|---------|--------|
| **"Mic On"** | Enable microphone in ChatGPT |
| **"Mic Off"** | Disable microphone in ChatGPT |
| **"Exit"** or **"Exit Voice Mode"** | Exit voice mode and return to text chat |

### Setting Up Voice Commands

1. **Launch ChatGPT and enter voice mode manually** (click the voice button)
2. Open HeyGPT Settings
3. Scroll to "🎙 Voice Command Buttons"
4. **Capture Mic Button**:
   - Click "Capture Mic Button (10s countdown)"
   - Hover over the mic button in ChatGPT (in voice mode)
   - Wait for countdown
5. **Capture Exit Button**:
   - Click "Capture Exit Voice Mode Button (10s countdown)"
   - Hover over the exit/close button in ChatGPT voice mode
   - Wait for countdown
6. Click Save

**Important**: Voice commands ONLY work when ChatGPT is in voice mode. HeyGPT monitors the blue orb every 2 seconds and enables/disables commands automatically.

---

## Troubleshooting

### Installation Issues

**Q: "Windows is searching for python.exe" error**
- This means you have a broken shortcut. Delete it and recreate by right-clicking `HeyGPT.exe` → Send to → Desktop (create shortcut)

**Q: App doesn't appear in Windows Search**
- Windows may need time to index. Launch directly from the installation folder or restart Windows

**Q: Missing .dll errors**
- Extract the entire ZIP contents together - don't move just the `.exe` file
- Or use the installer which handles this automatically

**Q: .NET runtime errors**
- Install [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

### Wake Word Detection Issues

**Q: Too many false positives (wake word triggers randomly)**
- **Solution 1**: Enable Porcupine High Accuracy Mode (97%+ accuracy)
- **Solution 2**: If using System.Speech, enable "Wake Word Isolation" in Settings
- **Solution 3**: Increase confidence threshold to 0.8

**Q: Wake word not detected with Porcupine**
- Verify AccessKey is valid (paste it again)
- Check microphone permissions (Windows Settings → Privacy → Microphone)
- Speak clearly and at normal volume
- Try adjusting sensitivity (0.3-0.7 range)

**Q: Wake word not detected with System.Speech**
- Check microphone permissions
- Speak wake word clearly and ALONE (not in a sentence)
- Try a simpler wake word (e.g., "Computer" instead of "Hey there GPT")
- Disable wake word isolation temporarily to test

### ChatGPT Launch Issues

**Q: ChatGPT doesn't launch**
- Verify ChatGPT is installed: Try running `chatgpt` in Command Prompt
- If that doesn't work, find the full path to ChatGPT.exe and enter it in Settings → Application Path
- Common path: `C:\Users\YourName\AppData\Local\Programs\ChatGPT\ChatGPT.exe`

**Q: ChatGPT launches on wrong monitor**
- Reconfigure monitor in Settings (make sure you move mouse to the correct monitor's center)

**Q: Voice mode doesn't activate**
- Manually configure button positions in Settings
- Check button text matches your ChatGPT language (Settings → Button Text)
- Increase button click delay if your system is slow

**Q: Blue orb not detected**
- This is normal on first launch
- The app uses screen color detection - ensure ChatGPT is visible (not minimized)

### Voice Command Issues

**Q: Voice commands not working**
- Voice commands ONLY work when ChatGPT is in voice mode (blue orb visible)
- Capture the correct button positions in Settings
- Speak commands clearly: "Mic On", "Mic Off", "Exit"

### Performance Issues

**Q: App uses too much CPU**
- This is normal during audio processing
- The app pauses briefly after each wake word detection to save resources

**Q: Slow automation**
- Increase button click delay in Settings (try 1500-2000ms)
- Your system may need more time to load ChatGPT

---

## FAQ

### General Questions

**Q: Is HeyGPT free?**
- Yes! HeyGPT is completely free and open-source (MIT License)

**Q: Do I need a Picovoice account?**
- No, but it's highly recommended for better accuracy
- Free tier is sufficient for personal use (no credit card required)

**Q: Can I use HeyGPT without internet?**
- No, ChatGPT requires internet to function
- However, wake word detection works offline

**Q: Does HeyGPT work with ChatGPT Plus?**
- Yes! HeyGPT works with both free and Plus accounts

**Q: Can I change the wake word?**
- Yes! You can use any of the 14 built-in wake words or create your own custom wake word

### Privacy & Security

**Q: Does HeyGPT send my audio to the cloud?**
- **With Porcupine**: Audio is processed locally on your device. Your AccessKey is sent to Picovoice servers for validation.
- **With System.Speech**: Everything is processed locally - zero cloud communication

**Q: Where is my Picovoice AccessKey stored?**
- Locally in `%APPDATA%\HeyGPT\settings.json`
- Never shared with anyone except Picovoice for wake word detection

**Q: Can I trust this app?**
- HeyGPT is open-source - you can review all the code on GitHub
- No telemetry, no tracking, no data collection

### Advanced Usage

**Q: Can I run multiple instances?**
- Not recommended - only one instance should listen for wake words at a time

**Q: Can I use HeyGPT on multiple computers?**
- Yes! Install it on each computer and configure separately

**Q: Can I contribute to HeyGPT?**
- Absolutely! Visit the [GitHub repository](https://github.com/aaravsaianugula/HeyGPT) and submit pull requests

**Q: How do I uninstall HeyGPT?**
- **If installed via installer**: Windows Settings → Apps & Features → HeyGPT → Uninstall
- **If using portable ZIP**: Delete the HeyGPT folder and `%APPDATA%\HeyGPT\` folder

---

## Getting Help

If you're still having issues:

1. **Check the Activity Log** in the main window for error messages
2. **Review this guide** for solutions
3. **Open an issue** on [GitHub Issues](https://github.com/aaravsaianugula/HeyGPT/issues)
4. **Provide details**: Your Windows version, error messages from the Activity Log, steps to reproduce

---

## Tips for Best Experience

1. ✅ **Use Porcupine** for 97%+ accuracy
2. ✅ **Speak clearly** and at normal volume
3. ✅ **Configure monitor** before first use
4. ✅ **Enable "Start with Windows"** for convenience
5. ✅ **Use built-in wake words** (Jarvis, Computer, Alexa) for best results
6. ✅ **Keep microphone close** (within 3 feet)
7. ✅ **Minimize background noise** for better detection

---

**Enjoy your hands-free ChatGPT experience! 🎤✨**

*For updates and announcements, visit the [GitHub repository](https://github.com/aaravsaianugula/HeyGPT)*
