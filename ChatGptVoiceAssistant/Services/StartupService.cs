using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;

namespace HeyGPT.Services
{
    public class StartupService
    {
        private const string AppName = "HeyGPT";
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public bool IsStartupEnabled()
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false))
                {
                    if (key == null)
                        return false;

                    object? value = key.GetValue(AppName);
                    return value != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking startup status: {ex.Message}");
                return false;
            }
        }

        public bool EnableStartup()
        {
            try
            {
                string executablePath = Process.GetCurrentProcess().MainModule?.FileName ?? Assembly.GetExecutingAssembly().Location;

                if (executablePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    executablePath = executablePath.Replace(".dll", ".exe");
                }

                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key == null)
                        return false;

                    key.SetValue(AppName, $"\"{executablePath}\"");
                    Debug.WriteLine($"Startup enabled: {executablePath}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error enabling startup: {ex.Message}");
                return false;
            }
        }

        public bool DisableStartup()
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key == null)
                        return false;

                    if (key.GetValue(AppName) != null)
                    {
                        key.DeleteValue(AppName);
                        Debug.WriteLine("Startup disabled");
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error disabling startup: {ex.Message}");
                return false;
            }
        }

        public bool SetStartupEnabled(bool enabled)
        {
            return enabled ? EnableStartup() : DisableStartup();
        }
    }
}
