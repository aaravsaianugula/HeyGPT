using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace HeyGPT.Services
{
    public class ImageRecognitionService
    {
        public unsafe bool DetectBlueOrb(Bitmap bitmap)
        {
            BitmapData? data = null;
            try
            {
                int centerX = bitmap.Width / 2;
                int centerY = bitmap.Height / 2;
                int searchRadius = Math.Min(bitmap.Width, bitmap.Height) / 4;

                int bluePixelCount = 0;
                int totalPixels = 0;

                data = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb
                );

                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;

                int startY = Math.Max(0, centerY - searchRadius);
                int endY = Math.Min(bitmap.Height, centerY + searchRadius);
                int startX = Math.Max(0, centerX - searchRadius);
                int endX = Math.Min(bitmap.Width, centerX + searchRadius);

                for (int y = startY; y < endY; y++)
                {
                    for (int x = startX; x < endX; x++)
                    {
                        int offset = y * stride + x * 4;
                        byte b = ptr[offset];
                        byte g = ptr[offset + 1];
                        byte r = ptr[offset + 2];

                        if (b > 150 && b > r + 30 && b > g + 30)
                        {
                            bluePixelCount++;
                        }
                        totalPixels++;
                    }
                }

                float blueRatio = (float)bluePixelCount / totalPixels;
                return blueRatio > 0.15f;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error detecting blue orb: {ex.Message}");
                return false;
            }
            finally
            {
                if (data != null)
                {
                    bitmap.UnlockBits(data);
                }
            }
        }

        public unsafe bool VerifyChatGptWindow(Bitmap bitmap)
        {
            BitmapData? data = null;
            try
            {
                int darkPixelCount = 0;
                int totalSamples = 0;
                int sampleStep = 10;

                bool hasMinimumSize = bitmap.Width > 400 && bitmap.Height > 300;
                if (!hasMinimumSize)
                {
                    return false;
                }

                data = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb
                );

                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;

                for (int y = 0; y < bitmap.Height; y += sampleStep)
                {
                    for (int x = 0; x < bitmap.Width; x += sampleStep)
                    {
                        int offset = y * stride + x * 4;
                        byte b = ptr[offset];
                        byte g = ptr[offset + 1];
                        byte r = ptr[offset + 2];

                        int brightness = (r + g + b) / 3;
                        if (brightness < 60)
                        {
                            darkPixelCount++;
                        }

                        totalSamples++;
                    }
                }

                float darkRatio = (float)darkPixelCount / totalSamples;
                return darkRatio > 0.5f;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error verifying ChatGPT window: {ex.Message}");
                return false;
            }
            finally
            {
                if (data != null)
                {
                    bitmap.UnlockBits(data);
                }
            }
        }

        public unsafe Rectangle? DetectVoiceModeButton(Bitmap bitmap)
        {
            BitmapData? data = null;
            try
            {
                int rightEdge = bitmap.Width;
                int searchWidth = 200;
                int searchLeft = rightEdge - searchWidth;

                data = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb
                );

                for (int y = bitmap.Height / 2; y < bitmap.Height - 100; y += 5)
                {
                    for (int x = searchLeft; x < rightEdge - 50; x += 5)
                    {
                        if (IsCircularRegion(bitmap, x, y, 20, data))
                        {
                            return new Rectangle(x - 20, y - 20, 60, 60);
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error detecting voice mode button: {ex.Message}");
                return null;
            }
            finally
            {
                if (data != null)
                {
                    bitmap.UnlockBits(data);
                }
            }
        }

        private bool IsBlueColor(Color color)
        {
            return color.B > 150 && color.B > color.R + 30 && color.B > color.G + 30;
        }

        private bool IsDarkColor(Color color)
        {
            int brightness = (color.R + color.G + color.B) / 3;
            return brightness < 60;
        }

        private unsafe bool IsCircularRegion(Bitmap bitmap, int centerX, int centerY, int radius, BitmapData data)
        {
            try
            {
                int edgePixels = 0;
                int totalPixels = 0;

                byte* ptr = (byte*)data.Scan0;
                int stride = data.Stride;

                for (int angle = 0; angle < 360; angle += 30)
                {
                    double radians = angle * Math.PI / 180;
                    int x = centerX + (int)(radius * Math.Cos(radians));
                    int y = centerY + (int)(radius * Math.Sin(radians));

                    if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height)
                    {
                        int offset = y * stride + x * 4;
                        byte b = ptr[offset];
                        byte g = ptr[offset + 1];
                        byte r = ptr[offset + 2];

                        int brightness = (r + g + b) / 3;

                        if (brightness > 100)
                        {
                            edgePixels++;
                        }

                        totalPixels++;
                    }
                }

                return totalPixels > 0 && (float)edgePixels / totalPixels > 0.5f;
            }
            catch
            {
                return false;
            }
        }
    }
}
