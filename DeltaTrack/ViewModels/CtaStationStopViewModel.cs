using DeltaTrack.Models;

namespace DeltaTrack.ViewModels
{
    public class CtaStationStopViewModel
    {
        public int MapId { get; set; }
        public string StationName { get; set; }
        public string StationDescriptiveName { get; set; }
        public Stop Stop { get; set; }
    }
}
