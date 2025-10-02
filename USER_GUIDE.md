# HeyGPT User Guide

**Welcome to HeyGPT!** This guide will help you set up and use your voice-activated ChatGPT assistant.

> 🎯 **Quick Start**: Get your FREE [Picovoice AccessKey](https://console.picovoice.ai) → Configure monitor → Start listening → Say "Jarvis"!

## Table of Contents

1. [5-Minute Setup](#5-minute-setup)
2. [Basic Usage](#basic-usage)
3. [Settings Explained](#settings-explained)
4. [Voice Commands](#voice-commands)
5. [Troubleshooting](#troubleshooting)
6. [FAQ](#faq)

---

## 5-Minute Setup

### Step 1: Install (1 minute)

1. Download **[HeyGPT-v1.1.0-Setup.exe](https://github.com/aaravsaianugula/HeyGPT/releases)**
2. Run installer → Done!

**Prerequisites**: Windows 10/11, [ChatGPT Desktop](https://openai.com/chatgpt/desktop/), [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0), Microphone

### Step 2: Get Porcupine AccessKey (30 seconds)

**For 97%+ Accuracy:**
1. Go to [console.picovoice.ai](https://console.picovoice.ai)
2. Sign up (free, no credit card)
3. Copy your AccessKey

**Skip this?** App will use System.Speech (lower accuracy, more false positives)

### Step 3: Configure HeyGPT (2 minutes)

1. **Launch HeyGPT** (Windows Search → "HeyGPT")

2. **Click ⚙ Settings**

3. **Paste AccessKey** (if you got one):
   - Section: "🎯 Porcupine AI"
   - Paste your key → Done!

4. **Choose Wake Word**:
   - Built-in: `jarvis`, `alexa`, `computer`, `hey google`, `hey siri`, etc.
   - Or keep default "Hey GPT"

5. **Configure Monitor**:
   - Click "Configure Monitor (10s countdown)"
   - Move mouse to center of target monitor
   - Wait for confirmation

6. **Click Save**

### Step 4: Test (1 minute)

1. Click **🧪 Test** button
2. ChatGPT should launch automatically
3. ✅ Success? You're ready!

### Step 5: Use It!

1. Click **▶ Start Listening**
2. Say your wake word (e.g., "Jarvis")
3. ChatGPT launches in voice mode!

---

## Advanced: Custom Wake Words

**Want "Okay Claude" or "Hey Buddy"?**

1. Visit [console.picovoice.ai](https://console.picovoice.ai)
2. Create custom wake word (follow their wizard)
3. Download `.ppn` file for Windows
4. In HeyGPT Settings → "🎨 Custom Wake Word" → Browse → Select file
5. Save!

---

## Basic Usage

1. **Launch** → Windows Search → "HeyGPT"
2. **Click** → ▶ Start Listening
3. **Say** → Your wake word (e.g., "Jarvis")
4. **Watch** → ChatGPT launches in voice mode!

**App keeps listening** - say wake word again anytime to launch another window.

**Minimize to tray** - Click X to hide in system tray (enable "Start minimized" in Settings)

**Stop listening** - Click ⏸ Stop button or close app

---

## Settings Explained

### 🎯 Porcupine AI (Recommended)

**AccessKey**: Get from [console.picovoice.ai](https://console.picovoice.ai)
**Sensitivity**: 0.5 default (higher = more sensitive, may have false positives)

### 🎤 Wake Word

**Choose Wake Word**: Type wake word or pick from built-ins
**Built-in Options**: jarvis, alexa, computer, hey google, hey siri, ok google, picovoice, porcupine, bumblebee, terminator, americano, blueberry, grapefruit, grasshopper
**Confidence Threshold**: Only used if NO Porcupine AccessKey (0.6-0.8 recommended)

**Advanced Settings (Expander)**: Only applies when using fallback mode (no Porcupine)

### 🎨 Custom Wake Word (Optional)

Upload `.ppn` files created at Picovoice Console for custom wake words like "Okay Claude"

### 🖥 Monitor & ChatGPT Setup

**Monitor**: Which screen to launch ChatGPT on
**ChatGPT Path**: Default `chatgpt` works if installed (otherwise enter full .exe path)
**Automation Delay**: Wait time between clicks (1000ms default)

### 🎯 Button Positions (Optional)

Capture exact button positions for improved reliability. Only needed if OCR fails.

### 🎙 Voice Command Buttons (Optional)

Capture mic/exit buttons to enable voice commands ("mic on", "mic off", "exit")

### ⚙ Application Settings

**Start with Windows**: Auto-launch on startup
**Start Minimized**: Launch hidden in system tray

---

## Voice Commands

**Control ChatGPT in voice mode** (when blue orb is visible):

- **"Mic On"** / **"Mic Off"** - Toggle microphone
- **"Exit"** / **"Exit Voice Mode"** - Exit to text chat

### Setup (Optional):

1. Launch ChatGPT, click voice button
2. HeyGPT Settings → "🎙 Voice Command Buttons"
3. Capture Mic Button (10s countdown) → hover over mic button
4. Capture Exit Button (10s countdown) → hover over exit button
5. Save

**Note**: Commands auto-enable when blue orb detected (checked every 2 seconds)

---

## Troubleshooting

### Common Issues

**❌ "Windows is searching for python.exe"**
→ Broken shortcut. Delete and recreate: Right-click `HeyGPT.exe` → Send to → Desktop

**❌ Missing .dll errors**
→ Install [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) or use the installer

**❌ App not in Windows Search**
→ Launch directly from install folder or restart Windows (indexing delay)

### Wake Word Issues

**❌ Too many false positives**
→ Get Porcupine AccessKey (97%+ accuracy, <1 false positive/10hrs)

**❌ Wake word not detected (Porcupine)**
→ Check: Valid AccessKey? Mic permissions? Speak clearly?
→ Try: Lower sensitivity (0.3), different wake word

**❌ Wake word not detected (System.Speech)**
→ Say wake word ALONE, not in sentence
→ Check mic permissions
→ Try simpler wake word ("computer" vs "hey there buddy")

### ChatGPT Issues

**❌ ChatGPT doesn't launch**
→ Test in CMD: type `chatgpt` and press Enter
→ Doesn't work? Enter full .exe path in Settings → ChatGPT Path
→ Example: `C:\Users\YourName\AppData\Local\Programs\ChatGPT\ChatGPT.exe`

**❌ Launches on wrong monitor**
→ Reconfigure: Settings → Configure Monitor → Move mouse to correct screen center

**❌ Voice mode doesn't activate**
→ Capture button positions in Settings
→ Increase automation delay (1500-2000ms)

**❌ Voice commands not working**
→ Commands only work when blue orb visible
→ Capture mic/exit button positions in Settings

---

## FAQ

**Q: Is it free?**
Yes! Open-source (MIT License)

**Q: Need Picovoice account?**
No, but highly recommended (97%+ accuracy, free tier, no credit card)

**Q: Works without internet?**
ChatGPT needs internet. Wake word detection works offline.

**Q: Works with ChatGPT Plus?**
Yes! Works with free and Plus.

**Q: Change wake word?**
Yes! 14 built-ins or create custom.

**Q: Is my audio sent to cloud?**
- Porcupine: Processed locally. AccessKey validates online.
- System.Speech: 100% local.

**Q: Where is AccessKey stored?**
`%APPDATA%\HeyGPT\settings.json` (local only)

**Q: Can I trust it?**
Open-source on GitHub. No telemetry/tracking.

**Q: Uninstall?**
- Installer: Windows Settings → Apps → HeyGPT → Uninstall
- ZIP: Delete folder + `%APPDATA%\HeyGPT\`

---

## Tips for Best Results

✅ Use Porcupine (97%+ accuracy)
✅ Speak clearly, normal volume
✅ Configure monitor first
✅ Use built-in wake words (Jarvis, Computer, Alexa)
✅ Keep mic within 3 feet
✅ Minimize background noise

---

**Need Help?**
1. Check Activity Log for errors
2. Visit [GitHub Issues](https://github.com/aaravsaianugula/HeyGPT/issues)
3. Provide: Windows version, error messages, steps to reproduce

**Enjoy hands-free ChatGPT! 🎤✨**

*Updates: [GitHub](https://github.com/aaravsaianugula/HeyGPT)*
