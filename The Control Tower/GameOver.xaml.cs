using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace The_Control_Tower
{
    /// <summary>
    /// Interaction logic for GameOver.xaml
    /// </summary>
    public partial class GameOver
    {
        public GameOver()
        {
            InitializeComponent();
            ImgGameOver.Source = new BitmapImage(new Uri("/Media/GameOver.jpg", UriKind.Relative));
        }
        /// <summary>
        /// Exit application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
