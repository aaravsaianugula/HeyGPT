using System;
using System.Windows;

namespace HeyGPT
{
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = args.ExceptionObject as Exception;
                MessageBox.Show($"XAML Parse Error:\n\n{ex?.Message}\n\n{ex?.InnerException?.Message}\n\n{ex?.StackTrace}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += (sender, args) =>
            {
                MessageBox.Show($"An unexpected error occurred:\n\n{args.Exception.Message}\n\nInner: {args.Exception.InnerException?.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                args.Handled = true;
            };
        }
    }
}
