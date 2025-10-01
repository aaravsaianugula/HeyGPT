using System.Windows;

namespace HeyGPT.Views
{
    public partial class UserGuideWindow : Window
    {
        public UserGuideWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
