using System;
using Pv;

class TestPorcupineInit
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Testing Porcupine Jarvis Initialization ===\n");

        string accessKey = "rppWsPXb+d5uxAFnZM4xTx/noDqaUd6fI9tzxGW59mRCmKG0mfwmKw==";

        try
        {
            Console.WriteLine("Attempting to initialize Porcupine with Jarvis wake word...");

            using (Porcupine porcupine = Porcupine.FromBuiltInKeywords(
                accessKey,
                new System.Collections.Generic.List<BuiltInKeyword> { BuiltInKeyword.JARVIS },
                modelPath: null,
                sensitivities: new System.Collections.Generic.List<float> { 0.5f }
            ))
            {
                Console.WriteLine($"\n✓ SUCCESS! Porcupine initialized with Jarvis");
                Console.WriteLine($"  Version: {porcupine.Version}");
                Console.WriteLine($"  Sample Rate: {porcupine.SampleRate} Hz");
                Console.WriteLine($"  Frame Length: {porcupine.FrameLength}");
                Console.WriteLine($"\nJarvis wake word is READY and WORKING!");
                Console.WriteLine("\nThe jarvis_windows.ppn file from Picovoice SDK is loaded correctly.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ FAILED: {ex.Message}");
            Console.WriteLine($"  Type: {ex.GetType().Name}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"  Inner: {ex.InnerException.Message}");
            }
            return;
        }

        Console.WriteLine("\n=== Test Complete ===");
    }
}
