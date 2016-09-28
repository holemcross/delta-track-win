using DeltaTrack.DataContexts;
using DeltaTrack.Models;
using DeltaTrack.Services;
using DeltaTrack.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DeltaTrack.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Dashboard : Page
    {
        private int DEFUALT_STATION_MAPID = 5;
        private int CLOCK_REFRESH_INTERVAL_SECONDS = 5;
        private int ARRIVALS_REFRESH_INTERVAL_SECONDS = 5;
        private int ARRIVALS_REPLACE_INTERVAL_SECONDS = 30;
        private CtaService _ctaService;

        private DashboardData _dashboardData;
        private DispatcherTimer _clockTimer;
        private DispatcherTimer _replaceArrivalsTimer;
        private DispatcherTimer _updateArrivalsTimer;

        
        public Dashboard()
        {
            this.InitializeComponent();

            _ctaService = new CtaService();

            InitalizeDataContext();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await UpdateTrainArrivals();
            SetTimers();
        }

        private void InitalizeDataContext()
        {
            var lastStation = GetLastStation();

            _dashboardData = new DashboardData
            {
                LastUpdateTime = DateTime.Now,
                TrainArrivals = new TrainArrivalCollection(),
                CurrentStation = lastStation ?? new Station
                {
                    MapId = 40730,
                    StationName = "Washington/Wells",
                    DescriptiveName = "Washington/Wells"
                }
            };

            this.DataContext = _dashboardData;
        }

        private Station GetLastStation()
        {
            // TODO - Add a check for getting last station

            return null;
        }

        private async Task<List<TrainArrival>> GetArrivals()
        {
            var currentMapId = _dashboardData.CurrentStation?.MapId;

            if (!currentMapId.HasValue) return new List<TrainArrival>();

            var result = await _ctaService.GetArrivalsForMapId(currentMapId.Value);

            return result.TrainArrivals ?? new List<TrainArrival>();
        }

        private async Task UpdateTrainArrivals()
        {
            var arrivals = await GetArrivals();

            _dashboardData.ReplaceTrainArrivals(arrivals);
        }

        private void SetTimers()
        {
            if (_clockTimer == null)
            {
                _clockTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, CLOCK_REFRESH_INTERVAL_SECONDS)};
                _clockTimer.Tick += ClockRefreshEvent;
            }
            _clockTimer.Start();

            if (_updateArrivalsTimer == null)
            {
                _updateArrivalsTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, ARRIVALS_REFRESH_INTERVAL_SECONDS)};
                _updateArrivalsTimer.Tick += ArrivalsRefreshEvent;
            }
            _updateArrivalsTimer.Start();

            if (_replaceArrivalsTimer == null)
            {
                _replaceArrivalsTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, ARRIVALS_REPLACE_INTERVAL_SECONDS)};
                _replaceArrivalsTimer.Tick += ArrivalsReplaceEvent;
            }
            _replaceArrivalsTimer.Start();
        }

        // Timer Events

        void ClockRefreshEvent(object sender, object e)
        {
            _dashboardData.RefreshClockTime();
        }

        void ArrivalsRefreshEvent(object sender, object e)
        {
            _dashboardData.UpdateArrivals();
        }

        async void ArrivalsReplaceEvent(object sender, object e)
        {
            await UpdateTrainArrivals();
        }


    }
}
