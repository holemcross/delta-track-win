using System;

namespace DeltaTrack.Helpers
{
    public class Abbreviation : Attribute
    {
        public string Name;

        public Abbreviation(string abbrviation)
        {
            Name = abbrviation;
        }
    }
}
