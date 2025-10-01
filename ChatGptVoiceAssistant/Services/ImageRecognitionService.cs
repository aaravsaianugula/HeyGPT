using System;
using System.Drawing;

namespace HeyGPT.Services
{
    public class ImageRecognitionService
    {
        public bool DetectBlueOrb(Bitmap bitmap)
        {
            try
            {
                int centerX = bitmap.Width / 2;
                int centerY = bitmap.Height / 2;
                int searchRadius = Math.Min(bitmap.Width, bitmap.Height) / 4;

                int bluePixelCount = 0;
                int totalPixels = 0;

                for (int y = centerY - searchRadius; y < centerY + searchRadius; y++)
                {
                    for (int x = centerX - searchRadius; x < centerX + searchRadius; x++)
                    {
                        if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height)
                        {
                            Color pixelColor = bitmap.GetPixel(x, y);

                            if (IsBlueColor(pixelColor))
                            {
                                bluePixelCount++;
                            }

                            totalPixels++;
                        }
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
        }

        public bool VerifyChatGptWindow(Bitmap bitmap)
        {
            try
            {
                int darkPixelCount = 0;
                int totalSamples = 0;
                int sampleStep = 10;

                for (int y = 0; y < bitmap.Height; y += sampleStep)
                {
                    for (int x = 0; x < bitmap.Width; x += sampleStep)
                    {
                        Color pixelColor = bitmap.GetPixel(x, y);

                        if (IsDarkColor(pixelColor))
                        {
                            darkPixelCount++;
                        }

                        totalSamples++;
                    }
                }

                float darkRatio = (float)darkPixelCount / totalSamples;

                bool hasDarkTheme = darkRatio > 0.5f;

                bool hasMinimumSize = bitmap.Width > 400 && bitmap.Height > 300;

                return hasDarkTheme && hasMinimumSize;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error verifying ChatGPT window: {ex.Message}");
                return false;
            }
        }

        public Rectangle? DetectVoiceModeButton(Bitmap bitmap)
        {
            try
            {
                int rightEdge = bitmap.Width;
                int searchWidth = 200;
                int searchLeft = rightEdge - searchWidth;

                for (int y = bitmap.Height / 2; y < bitmap.Height - 100; y += 5)
                {
                    for (int x = searchLeft; x < rightEdge - 50; x += 5)
                    {
                        if (IsCircularRegion(bitmap, x, y, 20))
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

        private bool IsCircularRegion(Bitmap bitmap, int centerX, int centerY, int radius)
        {
            try
            {
                int edgePixels = 0;
                int totalPixels = 0;

                for (int angle = 0; angle < 360; angle += 30)
                {
                    double radians = angle * Math.PI / 180;
                    int x = centerX + (int)(radius * Math.Cos(radians));
                    int y = centerY + (int)(radius * Math.Sin(radians));

                    if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height)
                    {
                        Color pixelColor = bitmap.GetPixel(x, y);
                        int brightness = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

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
