using System.Collections.Generic;
using Windows.Foundation;

namespace DeltaTrack.Models
{
    public class Stop
    {
        public int StopId { get; set; }
        public string StopName { get; set; }
        public List<CtaRoutes> Routes { get; set; }
        public Point Location { get; set; }
        public bool IsHandicapAccesible { get; set; }
    }
}
