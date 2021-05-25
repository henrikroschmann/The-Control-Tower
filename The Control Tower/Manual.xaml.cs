using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace The_Control_Tower
{
    /// <summary>
    /// Interaction logic for Manual.xaml
    /// </summary>
    public partial class Manual
    {
        public event EventHandler<bool> StartGame;
        private readonly string[] _manual = new string[3]
        {
            "/Media/Manual1.png",
            "/Media/Manual2.png",
            "/Media/Manual3.png"
        };

        private int _selected = 0;
        public Manual()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            SetManualPage(_selected);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Left = 2;
            this.Top = 400;
            //this.Height = 3;
        }

        private void SetManualPage(int i)
        {
            ImgStep.Source = new BitmapImage(new Uri(_manual[i], UriKind.Relative));
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (_selected > 0)
                BtPreviousButton.IsEnabled = false;
            if (_selected < 2)
                BtNextButton.IsEnabled = true;

            _selected--;
            SetManualPage(_selected);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (_selected == 0)
                BtPreviousButton.IsEnabled = true;
            if (_selected == 1)
                BtNextButton.IsEnabled = false;
            _selected++;
            SetManualPage(_selected);
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            StartGame?.Invoke(this, true);
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

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            StartGame?.Invoke(this, true);
            BtStartButton.IsEnabled = false;
        }
    }
}
