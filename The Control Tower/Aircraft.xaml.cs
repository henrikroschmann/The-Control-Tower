using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using The_Control_Tower.Events;
using The_Control_Tower.Models;

namespace The_Control_Tower
{
    /// <summary>
    /// Interaction logic for Aircraft.xaml
    /// </summary>
    public partial class Aircraft
    {
        private readonly IAirplane _airplane;

        public event EventHandler<Land> Land;

        public event EventHandler<TakeOff> TakeOff;

        public event EventHandler<FlyingAway> FlyingAway;
        public event EventHandler<RejectLanding> RejectLanding;

        public Aircraft(IAirplane airplane)
        {
            this._airplane = airplane;
            InitializeComponent();
            InitializeGui();
        }

        private void InitializeGui()
        {
            TbTitle.Text = $"Aircraft {_airplane.FlightCode}";
            SetImage();
            switch (_airplane.Statuses)
            {
                case Status.Landing:
                case Status.RequestToLand:
                case Status.NowHeading30Deg:
                    BtStart.IsEnabled = false;
                    CbAircraft.ItemsSource = Enum.GetValues(typeof(DocksEnum)).Cast<DocksEnum>();
                    BtRejectLanding.IsEnabled = true;
                    break;

                case Status.Started:
                case Status.SentToRunway:
                    BtLand.IsEnabled = false;
                    BtRejectLanding.IsEnabled = false;
                    CbAircraft.IsEnabled = false;
                    break;

                case Status.Hangar:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Event

        /// <summary>
        /// Event needed to allow window to be drag-able
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCTLetButtonDownEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        /// <summary>
        /// Click event to start and invoke event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartClick(object sender, RoutedEventArgs e)
        {
            var takOff = new TakeOff(_airplane.FlightCode, DocksEnum.None);
            TakeOff?.Invoke(this, takOff);
            var fa = new FlyingAway(_airplane.FlightCode);
            FlyingAway?.Invoke(this, fa);
            this.Close();
        }

        /// <summary>
        /// Click handler for land and invoke
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLandClick(object sender, RoutedEventArgs e)
        {
            if (!(CbAircraft == null || CbAircraft.Text != ""))
            {
                new CaptainsWord(
                    $"Aircraft {_airplane.FlightCode}, Tower no docking information provided in my drop down rejecting landing request").Show();
                return;
            }

            if (CbAircraft != null)
            {
                var land = new Land(_airplane.FlightCode, (DocksEnum)Enum.Parse(typeof(DocksEnum), CbAircraft.Text));
                Land?.Invoke(this, land);
            }

            this.Close();
        }

        /// <summary>
        /// Click event for closing the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseAircraft_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion Event

        #region Gui Stuff

        /// <summary>
        /// Method used to set the image for the Airline
        /// </summary>
        private void SetImage()
        {
            switch (_airplane.FlightCode.Substring(0, 3).ToLower())
            {
                case "abc":
                    ImgAircraft.Source = new BitmapImage(new Uri("/Media/abc.png", UriKind.Relative));
                    break;

                case "sas":
                    ImgAircraft.Source = new BitmapImage(new Uri("/Media/Sas.png", UriKind.Relative));
                    break;

                case "luf":
                    ImgAircraft.Source = new BitmapImage(new Uri("/Media/Lufthansa.png", UriKind.Relative));
                    break;

                case "klm":
                    ImgAircraft.Source = new BitmapImage(new Uri("/Media/KLM.png", UriKind.Relative));
                    break;
            }
        }

        #endregion Gui Stuff

        private void OnRejectLanding(object sender, RoutedEventArgs e)
        {
            var rejectLanding = new RejectLanding(_airplane.FlightCode);
            RejectLanding?.Invoke(this, rejectLanding);
        }
    }
}