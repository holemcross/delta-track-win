using DeltaTrack.Models;
using DeltaTrack.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DeltaTrack.DataContexts
{
    public class DashboardData : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public Station CurrentStation { get; set; }
        public TrainArrivalCollection TrainArrivals { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public string LastUpdateTimeDisplay => $"Last updated {LastUpdateTime.ToString("hh:mm:ss tt")}";
        public string ClockDisplay => DateTime.Now.ToString("MMM d, hh:mmtt");


        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public DashboardData()
        {
            CurrentStation = new Station
            {
                MapId = 4000,
                StationName = "",
                DescriptiveName = ""
            };
            LastUpdateTime = DateTime.Now;
            TrainArrivals = new TrainArrivalCollection();
        }

        public void ChangeStation(Station newStation)
        {
            CurrentStation = newStation;
            OnPropertyChanged(nameof(CurrentStation));
        }

        public void RefreshClockTime()
        {
            OnPropertyChanged(nameof(ClockDisplay));
        }

        public void UpdateArrivals()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, TrainArrivals));
        }

        public void ReplaceTrainArrivals(List<TrainArrival> newTrainArrivals)
        {
            TrainArrivals.Clear();

            foreach (var trainArrival in newTrainArrivals.OrderBy(x => x.ArrivalTime))
            {
                TrainArrivals.Add(trainArrival);
            }

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, TrainArrivals));
            //OnPropertyChanged(nameof(TrainArrivals));

            LastUpdateTime = DateTime.Now;
            OnPropertyChanged(nameof(LastUpdateTimeDisplay));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
