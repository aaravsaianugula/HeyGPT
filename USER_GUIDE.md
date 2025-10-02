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

## How to Create Custom Wake Words (Step-by-Step)

**Want to use a wake word NOT in the 14 built-ins?** (Like "Hey Claude", "Okay Buddy", "Yo Computer", etc.)

You need to create a `.ppn` file using Picovoice Console. Here's exactly how:

### Step 1: Sign Up at Picovoice (30 seconds)

1. **Visit** → [https://console.picovoice.ai](https://console.picovoice.ai)
2. **Click** → "Sign Up" button (top right)
3. **Enter** → Your email and create password
4. **Verify** → Check email and click verification link
5. **Done** → You're now logged into Picovoice Console

**Cost**: FREE - No credit card required for personal use

---

### Step 2: Create Your Wake Word (5-10 minutes)

1. **Click** → "Porcupine Wake Word" in left sidebar menu
2. **Click** → "Train New Keyword" button (top right)
3. **Enter Details**:
   - **Keyword**: Type your wake word (e.g., "Hey Claude")
   - **Language**: Select "English (en)"
   - **Platform**: Leave as "All" (we'll select Windows when downloading)
4. **Click** → "Train Keyword" button
5. **Wait** → Training takes ~5-10 minutes (you'll get email when done)

**Tips for Good Wake Words**:
- ✅ 2-3 words work best ("Hey Claude", "Okay Buddy")
- ✅ Use distinct sounds (avoid common phrases)
- ✅ Test pronunciation (say it out loud a few times)
- ❌ Avoid single words (less accurate)
- ❌ Avoid very long phrases (harder to detect)

---

### Step 3: Download .ppn File (30 seconds)

1. **Wait for email** → "Your wake word training is complete"
2. **Go back** → [https://console.picovoice.ai](https://console.picovoice.ai)
3. **Click** → "Porcupine Wake Word" in left menu
4. **Find your keyword** → It's now in the list
5. **Click** → Download icon (↓) next to your wake word
6. **Select Platform** → **Windows** (IMPORTANT!)
7. **Download** → Save the `.ppn` file (e.g., `hey_claude_windows.ppn`)
8. **Remember location** → You'll need this file in the next step

**File Name**: Should end with `_windows.ppn` (e.g., `hey_claude_windows.ppn`)

---

### Step 4: Upload to HeyGPT (1 minute)

1. **Open HeyGPT** → Launch the app
2. **Click** → ⚙ Settings button
3. **Scroll down** → Find "🎨 Custom Wake Word (Requires .ppn file)" section
4. **Click** → "Browse..." button
5. **Select** → Your downloaded `.ppn` file from Step 3
6. **Verify** → File path appears in text box
7. **Click** → "Save Settings" (bottom of window)
8. **Done** → Custom wake word is active!

---

### Step 5: Test Your Custom Wake Word (1 minute)

1. **Close Settings** → Back to main window
2. **Click** → ▶ Start Listening
3. **Say your wake word** → Clearly and at normal volume (e.g., "Hey Claude")
4. **Watch** → ChatGPT should launch in voice mode!
5. **Success?** → Your custom wake word works! 🎉

**Not working?**
- Check Activity Log for errors
- Verify you downloaded the **Windows** .ppn file (not Mac/Linux)
- Try speaking more clearly or adjusting Porcupine sensitivity in Settings
- Make sure Picovoice AccessKey is entered in Settings

---

### Example: Creating "Hey Claude" Wake Word

**Full walkthrough**:

```
1. Visit console.picovoice.ai → Sign up (free)
2. Click "Porcupine Wake Word" → "Train New Keyword"
3. Enter "Hey Claude" → Language: English → Train
4. Wait 5-10 minutes (check email)
5. Download → Platform: Windows → Save hey_claude_windows.ppn
6. HeyGPT Settings → Browse → Select hey_claude_windows.ppn
7. Save Settings → Start Listening → Say "Hey Claude" → Works!
```

**Time**: ~10-15 minutes total (including training wait time)

---

### Troubleshooting Custom Wake Words

**Q: Training failed / "Invalid keyword" error**
→ Try different wake word (avoid special characters, use 2-3 words)

**Q: Downloaded .ppn but HeyGPT says "file not found"**
→ Make sure you selected **Windows** platform (not Mac/Linux)

**Q: Wake word not detected**
→ Check: Picovoice AccessKey entered? Sensitivity too low? Speaking clearly?

**Q: Can I use the same .ppn on multiple computers?**
→ Yes! Copy the .ppn file to each computer and upload in Settings

**Q: Can I have multiple custom wake words?**
→ No, only one custom wake word at a time. But you can switch by uploading different .ppn files.

**Q: How do I switch back to built-in wake words?**
→ Settings → Custom Wake Word → Click "Clear" button → Select built-in from dropdown

---

### Quick Reference

| Step | What | Where | Time |
|------|------|-------|------|
| 1 | Sign up | [console.picovoice.ai](https://console.picovoice.ai) | 30 sec |
| 2 | Train keyword | Picovoice Console → Train New | 5-10 min |
| 3 | Download .ppn | Picovoice Console → Download (Windows) | 30 sec |
| 4 | Upload to HeyGPT | Settings → Browse → Select file | 1 min |
| 5 | Test | Start Listening → Say wake word | 1 min |

**Total Time**: ~10-15 minutes (mostly waiting for training)

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

### 🎤 Wake Word (Built-In Only)

**14 Built-In Options** (work without .ppn files):
- jarvis, alexa, computer, hey google, hey siri, ok google
- picovoice, porcupine, bumblebee, terminator
- americano, blueberry, grapefruit, grasshopper

**Select from dropdown** or type one of the above.

**Confidence Threshold**: Only used if NO Porcupine AccessKey (0.6-0.8 recommended)

**Advanced Settings (Expander)**: Only applies when using fallback mode (no Porcupine)

### 🎨 Custom Wake Word (Requires .ppn File)

**Want something else?** (like "Okay Claude", "Hey Buddy")
1. Create at [console.picovoice.ai](https://console.picovoice.ai) (free)
2. Download `.ppn` file for Windows
3. Settings → Browse → Select file → Save

**Note**: .ppn file required because Porcupine uses AI models trained for specific wake words

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
