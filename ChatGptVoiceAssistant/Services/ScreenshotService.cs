using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace HeyGPT.Services
{
    public class ScreenshotService
    {
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public Bitmap? CaptureWindow(IntPtr windowHandle)
        {
            try
            {
                if (windowHandle == IntPtr.Zero)
                {
                    return null;
                }

                if (!GetWindowRect(windowHandle, out RECT rect))
                {
                    return null;
                }

                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;

                if (width <= 0 || height <= 0)
                {
                    return null;
                }

                Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    IntPtr hdc = graphics.GetHdc();
                    PrintWindow(windowHandle, hdc, 0);
                    graphics.ReleaseHdc(hdc);
                }

                return bitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error capturing window: {ex.Message}");
                return null;
            }
        }

        public Bitmap? CaptureRegion(IntPtr windowHandle, Rectangle region)
        {
            try
            {
                Bitmap? fullScreenshot = CaptureWindow(windowHandle);
                if (fullScreenshot == null)
                {
                    return null;
                }

                if (region.X < 0 || region.Y < 0 ||
                    region.Width <= 0 || region.Height <= 0 ||
                    region.Right > fullScreenshot.Width ||
                    region.Bottom > fullScreenshot.Height)
                {
                    return fullScreenshot;
                }

                Bitmap croppedBitmap = fullScreenshot.Clone(region, fullScreenshot.PixelFormat);
                fullScreenshot.Dispose();

                return croppedBitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error capturing region: {ex.Message}");
                return null;
            }
        }
    }
}
