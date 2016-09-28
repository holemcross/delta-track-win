using DeltaTrack.Helpers;
using System;
using Windows.UI;

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

        public TimeSpan GetArrivalTimeSpan()
        {
            return ArrivalTime - DateTime.Now;
        }

        public string ArrivalDisplayText
        {
            get
            {
                string displayMessage;
                var arrivalTimeSpan = GetArrivalTimeSpan();

                if (arrivalTimeSpan.Minutes < 1 || IsApproaching)
                {
                    displayMessage = "Due";
                }
                else
                {
                    displayMessage = $"{arrivalTimeSpan.Minutes}.{(arrivalTimeSpan.Seconds/60f).ToString().Substring(2)} mins";
                }

                if (IsDelayed)
                {
                    displayMessage += " Dly";
                }

                return displayMessage;
            }
        }

        public Color RouteColor => RouteHelper.GetColorByRoute(Route);
    }
}
