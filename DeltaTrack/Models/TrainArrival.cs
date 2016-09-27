using System;

namespace DeltaTrack.Models
{
    public class TrainArrival
    {
        public int MapId { get; set; }
        public int StopId { get; set; }
        public int RunNumber { get; set; }
        public int DestinationStopId { get; set; }
        public string DestinationName { get; set; }
        public CtaRoutes Route { get; set; }
        public int TrainDirection { get; set; }
        public DateTime PredictionTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public bool IsApproaching { get; set; }
        public bool IsNotLiveData { get; set; }
        public bool IsDelayed { get; set; }
    }
}
