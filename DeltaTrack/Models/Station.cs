using System.Collections.Generic;
using System.Linq;

namespace DeltaTrack.Models
{
    public class Station
    {
        public string StationName { get; set; }
        public string DescriptiveName { get; set; }
        public int MapId { get; set; }
        public List<Stop> Stops { get; set; }

        public List<CtaRoutes> Routes
        {
            get
            {
                var fullList = Stops.SelectMany(st => st.Routes);
                return fullList.Distinct().ToList();
            }
        }

    }
}
