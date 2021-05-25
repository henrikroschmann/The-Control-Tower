using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using The_Control_Tower.Events;
using The_Control_Tower.Models;

namespace The_Control_Tower
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        internal Airport Airport;
        private Thread _thread;
        private readonly string[] _planeQueue = new string[10];
        private bool _gameOn = false;
        private readonly MediaPlayer _player = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            Airport = Airport.CreateAirport();
            InitializeGui();
        }

        /// <summary>
        /// Initialize the Gui and set all values
        /// </summary>
        private void InitializeGui()
        {
            SendAircraftToRunWay.Content = "Game not started click to start";
            SendAircraftToRunWay.Click += StartGame_Click;
            InitializeAirport();
            ListView.ItemsSource = Airport.Airplanes;
            var man = new Manual();
            man.StartGame += StartGame;
            man.Show();
        }

        private void StartGame(object sender, bool e)
        {
            if (!e) return;
            SendAircraftToRunWay.Content = "Send next Airplane to the Runway";
            SendAircraftToRunWay.Click -= StartGame_Click;
            SendAircraftToRunWay.Click += SendAircraftToRunWay_Click;
            _gameOn = true;
            _thread = new Thread(() => PopulateList(ListView));
            _thread.Start();
        }

        /// <summary>
        /// Initialize the Airport populate the add one Aircraft to start.
        /// </summary>
        private void InitializeAirport()
        {
            Airport.Airplanes = new List<IAirplane> { new Airplane().CreateAirplane(Status.Hangar, DocksEnum.Dock1) };
            RefreshAirport();
        }

        /// <summary>
        /// The subscribe method used to subscribe to a publisher
        /// </summary>
        /// <param name="ap"></param>
        private void Subscribe(Airplane ap)
        {
            var frm = new Aircraft(ap);
            frm.Show();

            frm.Land += OnLand;
            frm.TakeOff += OnTakeOff;
            frm.FlyingAway += OnFlyingAway;
            frm.RejectLanding += OnRejectLanding;
        }


        #region Events

        #region pubsub
        /// <summary>
        /// RejectLanding event called from the publisher
        /// Airplane circles around and refresh the airport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRejectLanding(object sender, RejectLanding e)
        {
            var airline = Airport.Airplanes?.FirstOrDefault(x => x.FlightCode == e.Flight);
            if (airline != null) airline.Statuses = Status.NowHeading30Deg;
            RefreshAirport();
        }

        /// <summary>
        /// Flyaway event called from the publisher
        /// Remove airline and refresh the airport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFlyingAway(object sender, FlyingAway e)
        {
            Airport.Airplanes.RemoveAt(Airport.Airplanes.FindIndex(x => x.FlightCode == e.Flight));
            RefreshAirport();
        }

        /// <summary>
        /// Take off event called from the publisher
        /// Free up runway and play animation and refresh airport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTakeOff(object sender, TakeOff e)
        {
            if (Airport.Docks == null) return;
            DepartingPlane(e);
        }



        /// <summary>
        /// Received event Land from publisher, updates Airport object based on information received from subscriber.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLand(object sender, Land e)
        {
            ArrivingPlane(e);
        }

        #endregion pubsub

        /// <summary>
        /// Click event when sending aircraft to runway
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendAircraftToRunWay_Click(object sender, RoutedEventArgs e)
        {
            if (Airport.Runway.Count(x => x.Name == RunwayEnum.Departure && x.InUse) > 0)
            {
                MessageBox.Show("Runway is in use we cannot allow more planes to lift off.");
                return;
            }

            var a = Airport.Airplanes?.FirstOrDefault(x => (x.FlightCode == TbFlightCode.Text || x.FlightCode.Replace(" ", "") == TbFlightCode.Text) && x.Statuses == Status.Hangar);
            if (a == null)
            {
                MessageBox.Show("No valid flight from the hangar selected");
                return;
            }
            var dock = Airport.Docks.FirstOrDefault(d => a != null && d.Name == a.Dock);
            if (dock != null) dock.InUse = false;
            if (a != null)
            {
                a.Statuses = Status.SentToRunway;
                a.Dock = DocksEnum.None;
            }

            var runway = Airport.Runway.FirstOrDefault(x => x.Name == RunwayEnum.Departure);
            if (runway != null)
            {
                runway.InUse = true;
                DepartPlane.Opacity = 1;
                Canvas.SetTop(DepartPlane, 37);
            }

            TbFlightCode.Text = "flight code";
            RefreshAirport();
        }
        /// <summary>
        /// Click event from list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDoubleClickClickListBoxItem(object sender, MouseButtonEventArgs e)
        {
            var aircraft = (Airplane)ListView.SelectedItem;
            if (aircraft == null) return;
            if (aircraft.Statuses == Status.Hangar)
            {
                TbFlightCode.Text = aircraft.FlightCode;
                return;
            }
            Subscribe(aircraft);
        }

        /// <summary>
        /// Event needed to allow window to be drag-able
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCTLetButtonDownEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        /// <summary>
        /// Exit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion Events

        #region General Gui stuff

        /// <summary>
        /// Main Game logic to add new aircraft.
        /// This will run as a separate task to not interfere with the main UI thread
        /// </summary>
        /// <returns></returns>
        private async void PopulateList(ListView listView)
        {
            while (_gameOn)
            {
                await listView.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    var ap = new Airplane().CreateAirplane();
                    Airport.Airplanes.Add(ap);

                    if (_planeQueue.Count(x => x == null) == 0)
                    {
                        new GameOver().Show();
                        _gameOn = false;
                    }

                    try
                    {
                        _planeQueue[_planeQueue.Count(x => x != null)] = ap.FlightCode;
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        new GameOver().Show();
                        _gameOn = false;
                    }
                    
                    _player.Open(new Uri("../../Media/Airplane-ding-sound.mp3", UriKind.RelativeOrAbsolute));
                    _player.Play();
                    RefreshAirport();
                    ListView.Items.Refresh();
                }));
                await Task.Delay(10000);
            }
        }

        /// <summary>
        /// Animation to be played when arriving and Arrival instructions.
        /// </summary>
        public async void ArrivingPlane(Land e)
        {

            if (Airport.Docks == null || e.Dock == DocksEnum.None)
            {
                new CaptainsWord($"Aircraft {e.FlightCode} rejected Command docking in None is not an option").Show();
                return;
            }
            if (Airport.Docks.Count(x => !x.InUse) <= 0)
            {
                new CaptainsWord($"Aircraft {e.FlightCode} rejected Command no dock available circulates airport another round").Show();
                return;
            }
            if (Airport.Docks.Count(x => x.Name == e.Dock && x.InUse) > 0)
            {
                new CaptainsWord($"Aircraft {e.FlightCode} rejected Command no double parking is allowed").Show();
                return;
            }


            ArrivalPlane.Opacity = 1;
            for (var i = 275; i > 37; i--)
            {
                Canvas.SetTop(ArrivalPlane, i);
                await Task.Delay(5);
            }
            ArrivalPlane.Opacity = 0;
            Canvas.SetTop(ArrivalPlane, 275);
            

            var a = Airport.Airplanes?.FirstOrDefault(x => x.FlightCode == e.FlightCode);
            if (a != null)
            {
                a.Dock = e.Dock;
                a.Statuses = Status.Hangar;
            }

            var d = Airport.Docks?.FirstOrDefault(x => x.Name == e.Dock);
            if (d != null)
            {
                d.InUse = true;
            }

            try
            {
                var index = Array.IndexOf(_planeQueue, e.FlightCode);
                if (index != null)
                    _planeQueue[index] = null;
            }
            catch (Exception exception)
            {
            }


            _player.Open(new Uri("../../Media/Landed-sound.mp3", UriKind.RelativeOrAbsolute));
            _player.Play();
            RefreshAirport();
        }

        /// <summary>
        /// Check Status of Airport is docks available runways etc.
        /// Populate fields.
        /// </summary>0
        private void RefreshAirport()
        {
            TbDocksInUse.Text = string.Join(",", Airport.Docks.Where(x => x.InUse).Select(x => x.Name));
            TbDocksAvailable.Text = string.Join(",", Airport.Docks.Where(x => !x.InUse).Select(x => x.Name));
            foreach (var docks in Airport.Docks)
            {
                switch (docks.Name)
                {
                    case DocksEnum.Dock1:
                        Dock1.Opacity = docks.InUse ? 1 : 0;
                        break;

                    case DocksEnum.Dock2:
                        Dock2.Opacity = docks.InUse ? 1 : 0;
                        break;

                    case DocksEnum.Dock3:
                        Dock3.Opacity = docks.InUse ? 1 : 0;
                        break;

                    case DocksEnum.Dock4:
                        Dock4.Opacity = docks.InUse ? 1 : 0;
                        break;

                    case DocksEnum.Dock5:
                        Dock5.Opacity = docks.InUse ? 1 : 0;
                        break;

                    case DocksEnum.None:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            PbStatus.Value = _planeQueue.Count(x => x != null);
            ListView.Items.Refresh();
        }

        /// <summary>
        /// Animation to be played when departing and departing instructions.
        /// </summary>
        private async void DepartingPlane(TakeOff e)
        {
            for (var i = 37; i < 200; i++)
            {
                Canvas.SetTop(DepartPlane, i);
                await Task.Delay(5);
            }
            DepartPlane.Opacity = 0;
            RefreshAirport();

            var a = Airport.Airplanes?.FirstOrDefault(x => x.FlightCode == e.FlightCode);
            if (a != null)
            {
                a.Dock = e.Dock;
                a.Statuses = Status.Started;
            }

            var d = Airport.Docks?.FirstOrDefault(x => x.Name == e.Dock);
            if (d != null)
            {
                d.InUse = false;
            }

            var r = Airport.Runway?.FirstOrDefault(x => x.Name == RunwayEnum.Departure);
            if (r != null)
            {
                r.InUse = false;
            }
        }

        #endregion General Gui stuff


        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            StartGame(this, true);
        }
    }
}