using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace HeyGPT.Services
{
    public class WindowAutomationService
    {
        private readonly ScreenshotService _screenshotService;
        private readonly OcrService _ocrService;
        private readonly ImageRecognitionService _imageRecognitionService;
        private Process? _cachedChatGptProcess;
        private DateTime _lastProcessCheck = DateTime.MinValue;
        private const int ProcessCacheDurationSeconds = 5;

        private static readonly string[] AllowedExecutables = new[]
        {
            "chatgpt",
            "chatgpt.exe",
            @"C:\Program Files\ChatGPT\ChatGPT.exe",
            @"%LOCALAPPDATA%\Programs\ChatGPT\ChatGPT.exe",
            @"%USERPROFILE%\AppData\Local\Programs\ChatGPT\ChatGPT.exe"
        };

        public event EventHandler<string>? DebugLog;

        public WindowAutomationService()
        {
            _screenshotService = new ScreenshotService();
            _ocrService = new OcrService();
            _imageRecognitionService = new ImageRecognitionService();
        }

        private void Log(string message)
        {
            Debug.WriteLine(message);
            DebugLog?.Invoke(this, message);
        }

        private bool IsAllowedExecutable(string path)
        {
            string expandedPath = Environment.ExpandEnvironmentVariables(path);

            foreach (string allowed in AllowedExecutables)
            {
                string expandedAllowed = Environment.ExpandEnvironmentVariables(allowed);
                if (string.Equals(expandedPath, expandedAllowed, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            if (File.Exists(expandedPath) && expandedPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                string fileName = Path.GetFileName(expandedPath);
                if (string.Equals(fileName, "ChatGPT.exe", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private string? ResolveExecutablePath(string appPath)
        {
            string expandedPath = Environment.ExpandEnvironmentVariables(appPath);

            if (!IsAllowedExecutable(appPath))
            {
                Log($"Security: Blocked unauthorized executable: {appPath}");
                return null;
            }

            if (File.Exists(expandedPath))
            {
                return expandedPath;
            }

            string[] searchPaths = new[]
            {
                expandedPath,
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs", "ChatGPT", "ChatGPT.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "ChatGPT", "ChatGPT.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "Programs", "ChatGPT", "ChatGPT.exe")
            };

            foreach (string path in searchPaths)
            {
                if (File.Exists(path))
                {
                    Log($"Found ChatGPT at: {path}");
                    return path;
                }
            }

            return null;
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);

        private const int SW_RESTORE = 9;
        private const int SW_SHOW = 5;
        private const int SW_MAXIMIZE = 3;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public async Task<bool> LaunchAndControlChatGpt(string appPath, Point targetMonitorCenter, int delayMs, string newChatButtonText, string voiceModeButtonText, Point newChatButtonPosition, bool isNewChatButtonConfigured, Point voiceModeButtonPosition, bool isVoiceModeButtonConfigured)
        {
            try
            {
                Process? chatGptProcess = GetChatGptProcess();

                if (chatGptProcess == null)
                {
                    string? resolvedPath = ResolveExecutablePath(appPath);
                    if (resolvedPath == null)
                    {
                        Log($"Failed to resolve executable path: {appPath}");
                        return false;
                    }

                    Log($"Launching ChatGPT: {resolvedPath}");

                    try
                    {
                        chatGptProcess = Process.Start(new ProcessStartInfo
                        {
                            FileName = resolvedPath,
                            UseShellExecute = false,
                            CreateNoWindow = false
                        });

                        if (chatGptProcess == null)
                        {
                            Log("Failed to start ChatGPT process");
                            return false;
                        }

                        Log("ChatGPT launch command executed, waiting for window...");
                        await Task.Delay(5000);

                        chatGptProcess = GetChatGptProcess();
                        if (chatGptProcess == null)
                        {
                            Log("ChatGPT process not found after launch, waiting additional time...");
                            await Task.Delay(3000);
                            chatGptProcess = GetChatGptProcess();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"Failed to launch ChatGPT: {ex.Message}");
                        return false;
                    }
                }
                else
                {
                    ShowWindow(chatGptProcess.MainWindowHandle, SW_RESTORE);
                    await Task.Delay(500);
                }

                if (chatGptProcess == null || chatGptProcess.MainWindowHandle == IntPtr.Zero)
                {
                    return false;
                }

                MoveWindowToMonitor(chatGptProcess.MainWindowHandle, targetMonitorCenter);
                await Task.Delay(500);

                SetForegroundWindow(chatGptProcess.MainWindowHandle);
                await Task.Delay(delayMs);

                bool windowOnCorrectMonitor = VerifyWindowPosition(chatGptProcess.MainWindowHandle, targetMonitorCenter);
                if (!windowOnCorrectMonitor)
                {
                    return false;
                }

                await ClickButtonByText(chatGptProcess.MainWindowHandle, newChatButtonText, newChatButtonPosition, isNewChatButtonConfigured);
                await Task.Delay(delayMs);

                await ClickButtonByText(chatGptProcess.MainWindowHandle, voiceModeButtonText, voiceModeButtonPosition, isVoiceModeButtonConfigured);
                await Task.Delay(delayMs);

                bool voiceModeActive = await VerifyVoiceModeActive(chatGptProcess.MainWindowHandle);
                if (voiceModeActive)
                {
                    Log("✓ Voice mode confirmed active (blue orb detected)");
                }
                else
                {
                    Log("⚠ Voice mode may not be active (blue orb not detected)");
                }

                return true;
            }
            catch (Exception ex)
            {
                Log($"Error in LaunchAndControlChatGpt: {ex.Message}");
                return false;
            }
        }

        private Process? GetChatGptProcess()
        {
            var now = DateTime.Now;

            if (_cachedChatGptProcess != null &&
                !_cachedChatGptProcess.HasExited &&
                (now - _lastProcessCheck).TotalSeconds < ProcessCacheDurationSeconds)
            {
                return _cachedChatGptProcess;
            }

            _lastProcessCheck = now;
            int currentProcessId = Process.GetCurrentProcess().Id;

            _cachedChatGptProcess = Process.GetProcesses()
                .FirstOrDefault(p => p.ProcessName.Equals("ChatGPT", StringComparison.OrdinalIgnoreCase)
                                     && p.MainWindowHandle != IntPtr.Zero
                                     && p.Id != currentProcessId);

            return _cachedChatGptProcess;
        }

        private void MoveWindowToMonitor(IntPtr windowHandle, Point targetMonitorCenter)
        {
            System.Windows.Forms.Screen targetScreen = System.Windows.Forms.Screen.FromPoint(targetMonitorCenter);
            Rectangle bounds = targetScreen.Bounds;

            ShowWindow(windowHandle, SW_RESTORE);
            Thread.Sleep(100);

            int x = bounds.Left + 100;
            int y = bounds.Top + 100;
            SetWindowPos(windowHandle, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);

            Thread.Sleep(200);

            ShowWindow(windowHandle, SW_MAXIMIZE);
        }

        private bool VerifyWindowPosition(IntPtr windowHandle, Point targetMonitorCenter)
        {
            if (GetWindowRect(windowHandle, out RECT rect))
            {
                Point windowCenter = new Point(
                    (rect.Left + rect.Right) / 2,
                    (rect.Top + rect.Bottom) / 2
                );

                System.Windows.Forms.Screen windowScreen = System.Windows.Forms.Screen.FromPoint(windowCenter);
                System.Windows.Forms.Screen targetScreen = System.Windows.Forms.Screen.FromPoint(targetMonitorCenter);

                return windowScreen.Equals(targetScreen);
            }

            return false;
        }

        private async Task ClickButtonByText(IntPtr windowHandle, string buttonText, Point buttonPosition, bool isButtonConfigured)
        {
            Log($"=== Attempting to click button: '{buttonText}' ===");

            if (!GetWindowRect(windowHandle, out RECT windowRect))
            {
                Log("Failed to get window rect");
                return;
            }

            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;
            Log($"Window dimensions: {windowWidth}x{windowHeight}");

            bool isNewChatButton = buttonText.Contains("New", StringComparison.OrdinalIgnoreCase) ||
                                   buttonText.Contains("chat", StringComparison.OrdinalIgnoreCase);
            bool isVoiceButton = buttonText.Contains("Voice", StringComparison.OrdinalIgnoreCase);

            if (isNewChatButton)
            {
                if (isButtonConfigured && buttonPosition != Point.Empty)
                {
                    Log($"Using configured New Chat button position: ({buttonPosition.X}, {buttonPosition.Y})");
                    ClickAtPosition(buttonPosition.X, buttonPosition.Y);
                    await Task.Delay(300);
                    Log($"✓ New Chat button clicked at configured position");
                    return;
                }
                else
                {
                    Log("New Chat button position not configured - using fallback position");
                    int clickX = windowRect.Left + 110;
                    int clickY = windowRect.Top + 65;
                    Log($"Clicking New Chat at fallback position: ({clickX}, {clickY})");
                    ClickAtPosition(clickX, clickY);
                    await Task.Delay(300);
                    Log($"✓ New Chat button clicked at fallback position");
                    return;
                }
            }

            if (isVoiceButton)
            {
                if (isButtonConfigured && buttonPosition != Point.Empty)
                {
                    Log($"Using configured Voice Mode button position: ({buttonPosition.X}, {buttonPosition.Y})");
                    ClickAtPosition(buttonPosition.X, buttonPosition.Y);
                    await Task.Delay(300);
                    Log($"✓ Voice Mode button clicked at configured position");
                    return;
                }
                else
                {
                    Log("Voice Mode button position not configured - using fallback position");
                    int clickX = windowRect.Right - 80;
                    int clickY = windowRect.Bottom - 80;
                    Log($"Clicking Voice Mode at fallback position: ({clickX}, {clickY})");
                    ClickAtPosition(clickX, clickY);
                    await Task.Delay(300);
                    Log($"✓ Voice Mode button clicked at fallback position");
                    return;
                }
            }

            Log($"⚠ Unknown button type: '{buttonText}' - attempting OCR fallback");

            try
            {
                using (var screenshot = _screenshotService.CaptureWindow(windowHandle))
                {
                    if (screenshot == null)
                    {
                        Log("Failed to capture window screenshot for OCR");
                        return;
                    }

                    Log($"Screenshot captured: {screenshot.Width}x{screenshot.Height}");

                    string sanitizedButtonText = new string(buttonText
                        .Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '_')
                        .ToArray())
                        .Replace(" ", "_");

                    if (sanitizedButtonText.Length > 50)
                    {
                        sanitizedButtonText = sanitizedButtonText.Substring(0, 50);
                    }

                    if (string.IsNullOrWhiteSpace(sanitizedButtonText))
                    {
                        sanitizedButtonText = "button";
                    }

                    string baseDir = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "HeyGPT");

                    string debugPath = Path.Combine(baseDir, $"debug_{sanitizedButtonText}_{DateTime.Now:HHmmss}.png");

                    string normalizedPath = Path.GetFullPath(debugPath);
                    string normalizedBaseDir = Path.GetFullPath(baseDir);

                    if (!normalizedPath.StartsWith(normalizedBaseDir, StringComparison.OrdinalIgnoreCase))
                    {
                        Log($"Security: Blocked path traversal attempt: {buttonText}");
                        return;
                    }

                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(debugPath)!);
                        screenshot.Save(debugPath, System.Drawing.Imaging.ImageFormat.Png);
                        Log($"Debug screenshot saved: {debugPath}");
                    }
                    catch (Exception ex)
                    {
                        Log($"Failed to save debug screenshot: {ex.Message}");
                    }

                    Log("Running OCR on screenshot...");
                    var allOcrResults = await _ocrService.RecognizeTextAsync(screenshot);
                    Log($"OCR found {allOcrResults.Count} text elements");

                    foreach (var result in allOcrResults.Take(10))
                    {
                        Log($"  - '{result.Text}' at ({result.BoundingBox.X}, {result.BoundingBox.Y})");
                    }

                    Rectangle? buttonLocation = await _ocrService.FindTextLocation(screenshot, buttonText, partialMatch: true);

                    if (buttonLocation.HasValue)
                    {
                        int screenX = windowRect.Left + buttonLocation.Value.X + buttonLocation.Value.Width / 2;
                        int screenY = windowRect.Top + buttonLocation.Value.Y + buttonLocation.Value.Height / 2;

                        Log($"Found via OCR - clicking at: ({screenX}, {screenY})");
                        ClickAtPosition(screenX, screenY);
                        Log($"✓ Click executed");
                    }
                    else
                    {
                        Log($"✗ OCR could not find '{buttonText}'");
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"OCR fallback error: {ex.Message}");
            }
        }

        private void ClickAtPosition(int x, int y)
        {
            System.Windows.Forms.Cursor.Position = new Point(x, y);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(50);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }

        private async Task<bool> VerifyVoiceModeActive(IntPtr windowHandle)
        {
            try
            {
                await Task.Delay(1000);

                using (var screenshot = _screenshotService.CaptureWindow(windowHandle))
                {
                    if (screenshot == null)
                    {
                        return false;
                    }

                    bool blueOrbDetected = _imageRecognitionService.DetectBlueOrb(screenshot);
                    return blueOrbDetected;
                }
            }
            catch (Exception ex)
            {
                Log($"Error verifying voice mode: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsVoiceModeActive()
        {
            try
            {
                await Task.Delay(100);

                Process? chatGptProcess = GetChatGptProcess();
                if (chatGptProcess == null || chatGptProcess.MainWindowHandle == IntPtr.Zero)
                {
                    return false;
                }

                using (var screenshot = _screenshotService.CaptureWindow(chatGptProcess.MainWindowHandle))
                {
                    if (screenshot == null)
                    {
                        return false;
                    }

                    bool blueOrbDetected = _imageRecognitionService.DetectBlueOrb(screenshot);
                    return blueOrbDetected;
                }
            }
            catch (Exception ex)
            {
                Log($"Error checking voice mode: {ex.Message}");
                return false;
            }
        }

        public bool IsChatGptWindowOpen()
        {
            try
            {
                Process? chatGptProcess = GetChatGptProcess();
                return chatGptProcess != null && chatGptProcess.MainWindowHandle != IntPtr.Zero;
            }
            catch (Exception ex)
            {
                Log($"Error checking ChatGPT window: {ex.Message}");
                return false;
            }
        }

        public async Task ClickMicButton(Point micButtonPosition, bool isMicButtonConfigured)
        {
            try
            {
                Process? chatGptProcess = GetChatGptProcess();
                if (chatGptProcess == null || chatGptProcess.MainWindowHandle == IntPtr.Zero)
                {
                    Log("ChatGPT process not found - cannot click mic button");
                    return;
                }

                Log("=== Clicking Mic Button ===");

                if (isMicButtonConfigured && micButtonPosition != Point.Empty)
                {
                    Log($"Using configured mic button position: ({micButtonPosition.X}, {micButtonPosition.Y})");
                    ClickAtPosition(micButtonPosition.X, micButtonPosition.Y);
                    await Task.Delay(300);
                    Log("✓ Mic button clicked");
                }
                else
                {
                    Log("⚠ Mic button position not configured");
                }
            }
            catch (Exception ex)
            {
                Log($"Error clicking mic button: {ex.Message}");
            }
        }

        public async Task ClickExitVoiceModeButton(Point exitVoiceModeButtonPosition, bool isExitVoiceModeButtonConfigured)
        {
            try
            {
                Process? chatGptProcess = GetChatGptProcess();
                if (chatGptProcess == null || chatGptProcess.MainWindowHandle == IntPtr.Zero)
                {
                    Log("ChatGPT process not found - cannot click exit voice mode button");
                    return;
                }

                Log("=== Clicking Exit Voice Mode Button ===");

                if (isExitVoiceModeButtonConfigured && exitVoiceModeButtonPosition != Point.Empty)
                {
                    Log($"Using configured exit button position: ({exitVoiceModeButtonPosition.X}, {exitVoiceModeButtonPosition.Y})");
                    ClickAtPosition(exitVoiceModeButtonPosition.X, exitVoiceModeButtonPosition.Y);
                    await Task.Delay(300);
                    Log("✓ Exit voice mode button clicked");
                }
                else
                {
                    Log("⚠ Exit voice mode button position not configured");
                }
            }
            catch (Exception ex)
            {
                Log($"Error clicking exit voice mode button: {ex.Message}");
            }
        }
    }
}
