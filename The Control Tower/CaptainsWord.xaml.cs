using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace The_Control_Tower
{
    /// <summary>
    /// Interaction logic for CaptainsWord.xaml
    /// </summary>
    public partial class CaptainsWord
    {
        public CaptainsWord(string message)
        {
            InitializeComponent();
            ImgCaptain.Source = new BitmapImage(new Uri("/Media/CaptainSays.jpeg", UriKind.Relative));
            TbMessage.Text = message;
            SystemSounds.Asterisk.Play();
        }
        /// <summary>
        /// Click event for closing the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Be able to drag the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCTLetButtonDownEvent(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
