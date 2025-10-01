using System.Windows;

namespace HeyGPT
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += (sender, args) =>
            {
                MessageBox.Show($"An unexpected error occurred:\n\n{args.Exception.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                args.Handled = true;
            };
        }
    }
}
