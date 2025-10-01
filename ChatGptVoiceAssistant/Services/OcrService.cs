using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

namespace HeyGPT.Services
{
    public class OcrResult
    {
        public string Text { get; set; } = string.Empty;
        public Rectangle BoundingBox { get; set; }
        public float Confidence { get; set; }
    }

    public class OcrService
    {
        private readonly OcrEngine? _ocrEngine;

        public OcrService()
        {
            _ocrEngine = OcrEngine.TryCreateFromLanguage(new Language("en"));
            if (_ocrEngine == null)
            {
                throw new InvalidOperationException("OCR engine could not be initialized. Language pack may not be installed.");
            }
        }

        public async Task<List<OcrResult>> RecognizeTextAsync(Bitmap bitmap)
        {
            List<OcrResult> results = new List<OcrResult>();

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap.Save(memoryStream, ImageFormat.Bmp);
                    memoryStream.Position = 0;

                    IRandomAccessStream randomAccessStream = memoryStream.AsRandomAccessStream();
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(randomAccessStream);

                    using (SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync())
                    {
                        SoftwareBitmap processedBitmap = softwareBitmap;
                        if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8)
                        {
                            processedBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8);
                        }

                        var ocrResult = await _ocrEngine!.RecognizeAsync(processedBitmap);

                        if (processedBitmap != softwareBitmap)
                        {
                            processedBitmap.Dispose();
                        }

                        foreach (var line in ocrResult.Lines)
                        {
                            foreach (var word in line.Words)
                            {
                                Rectangle boundingBox = new Rectangle(
                                    (int)word.BoundingRect.X,
                                    (int)word.BoundingRect.Y,
                                    (int)word.BoundingRect.Width,
                                    (int)word.BoundingRect.Height
                                );

                                results.Add(new OcrResult
                                {
                                    Text = word.Text,
                                    BoundingBox = boundingBox,
                                    Confidence = 1.0f
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OCR Error: {ex.Message}");
            }

            return results;
        }

        public async Task<Rectangle?> FindTextLocation(Bitmap bitmap, string searchText, bool partialMatch = true)
        {
            var ocrResults = await RecognizeTextAsync(bitmap);

            foreach (var result in ocrResults)
            {
                bool isMatch = partialMatch
                    ? result.Text.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    : result.Text.Equals(searchText, StringComparison.OrdinalIgnoreCase);

                if (isMatch)
                {
                    return result.BoundingBox;
                }
            }

            var combinedText = string.Join(" ", ocrResults.Select(r => r.Text));
            if (combinedText.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            {
                var relevantResults = ocrResults
                    .Where(r => combinedText.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (relevantResults.Any())
                {
                    return relevantResults.First().BoundingBox;
                }
            }

            return null;
        }

        public async Task<bool> VerifyTextExists(Bitmap bitmap, string searchText)
        {
            var ocrResults = await RecognizeTextAsync(bitmap);
            var allText = string.Join(" ", ocrResults.Select(r => r.Text));

            return allText.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }
    }
}
