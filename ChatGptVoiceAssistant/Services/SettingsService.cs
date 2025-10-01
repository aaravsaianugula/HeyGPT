using System;
using System.IO;
using HeyGPT.Models;
using Newtonsoft.Json;

namespace HeyGPT.Services
{
    public class SettingsService
    {
        private static readonly string SettingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "HeyGPT"
        );

        private static readonly string SettingsFilePath = Path.Combine(SettingsDirectory, "settings.json");

        public AppSettings LoadSettings()
        {
            try
            {
                if (!Directory.Exists(SettingsDirectory))
                {
                    Directory.CreateDirectory(SettingsDirectory);
                }

                if (File.Exists(SettingsFilePath))
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    var settings = JsonConvert.DeserializeObject<AppSettings>(json);

                    if (settings != null)
                    {
                        bool needsSave = false;

                        if (settings.ChatGptAppPath.Contains("ChatGPT.exe", StringComparison.OrdinalIgnoreCase))
                        {
                            settings.ChatGptAppPath = "chatgpt";
                            needsSave = true;
                        }

                        if (needsSave)
                        {
                            SaveSettings(settings);
                        }

                        return settings;
                    }

                    return new AppSettings();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
            }

            return new AppSettings();
        }

        public void SaveSettings(AppSettings settings)
        {
            try
            {
                if (!Directory.Exists(SettingsDirectory))
                {
                    Directory.CreateDirectory(SettingsDirectory);
                }

                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
                throw;
            }
        }
    }
}
