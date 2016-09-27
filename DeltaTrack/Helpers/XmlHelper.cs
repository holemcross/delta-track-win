using System.Xml.Linq;

namespace DeltaTrack.Helpers
{
    public static class XmlHelper
    {
        public static int GetIntFromAttribute(XElement element, string attributeName)
        {
            return int.Parse(element.Attribute(attributeName)?.Value ?? "-1");
        }

        public static double GetDoubleFromAttribute(XElement element, string attributeName)
        {
            return double.Parse(element.Attribute(attributeName)?.Value ?? "-1");
        }

        public static bool GetBoolFromAttribute(XElement element, string attributeName)
        {
            return bool.Parse(element.Attribute(attributeName)?.Value ?? "false");
        }
    }
}
