using DeltaTrack.Models;

namespace DeltaTrack.Helpers
{
    public static class RouteHelper
    {
        public static CtaRoutes GetRouteByAbbreviation(string abbrevation)
        {
            switch (abbrevation.ToLower())
            {
                case "blue":
                    return CtaRoutes.Blue;
                case "brn":
                    return CtaRoutes.Brown;
                case "g":
                    return CtaRoutes.Green;
                case "org":
                    return CtaRoutes.Orange;
                case "pink":
                    return CtaRoutes.Pink;
                case "red":
                    return CtaRoutes.Red;
                case "y":
                    return CtaRoutes.Yellow;
                default:
                    return CtaRoutes.Invalid;
            }
        }

        public static CtaRoutes GetRouteByName(string name)
        {
            switch (name.ToLower())
            {
                case "blue":
                    return CtaRoutes.Blue;
                case "brown":
                    return CtaRoutes.Brown;
                case "green":
                    return CtaRoutes.Green;
                case "orgnge":
                    return CtaRoutes.Orange;
                case "pink":
                    return CtaRoutes.Pink;
                case "red":
                    return CtaRoutes.Red;
                case "yellow":
                    return CtaRoutes.Yellow;
                default:
                    return CtaRoutes.Invalid;
            }
        }
    }
}
