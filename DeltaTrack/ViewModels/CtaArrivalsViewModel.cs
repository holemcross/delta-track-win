using DeltaTrack.Models;
using System;
using System.Collections.Generic;

namespace DeltaTrack.ViewModels
{
    public class CtaArrivalsViewModel
    {
        public List<TrainArrival> TrainArrivals { get; set; }
        public DateTime TimeStamp { get; set; }
        public int ErrorCode { get; set; }

        public CtaArrivalsViewModel()
        {
            TrainArrivals = new List<TrainArrival>();
            TimeStamp = DateTime.Now;
            ErrorCode = 0;
        }

    }
}
