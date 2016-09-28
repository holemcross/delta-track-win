using DeltaTrack.Models;
using Windows.UI;

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
                case "p":
                    return CtaRoutes.Purple;
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
                case "orange":
                    return CtaRoutes.Orange;
                case "purple":
                    return CtaRoutes.Pink;
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

        public static Color GetColorByRoute(CtaRoutes route)
        {
            switch (route)
            {
                case CtaRoutes.Blue: // 3F51B5
                    return Color.FromArgb(0xFF,0x3F,0x51,0xB5);
                case CtaRoutes.Brown: // 623607
                    return Color.FromArgb(0xFF,0x62,0x36,0x07);
                case CtaRoutes.Green: // 62a11b
                    return Color.FromArgb(0xFF,0x62,0xA1,0x1B);
                case CtaRoutes.Orange: // f9461c
                    return Color.FromArgb(0xFF,0xF9,0x46,0x1c);
                case CtaRoutes.Pink: // E27EA6
                    return Color.FromArgb(0xFF,0xE2,0x7E,0xA6);
                case CtaRoutes.Purple: // 522398
                    return Color.FromArgb(0xFF,0x52,0x23,0x98);
                case CtaRoutes.Red: // c60c30
                    return Color.FromArgb(0xFF,0xC6,0xC3,0x30);
                case CtaRoutes.Yellow: // f9e300
                    return Color.FromArgb(0xFF,0xF9,0xE3,0x00);
                default:
                    return Color.FromArgb(0xFF,0xFF,0xFF,0xFF);
            }
        }

        public static Color GetFontColorByRoute(CtaRoutes route)
        {
            switch (route)
            {
                case CtaRoutes.Yellow:
                    return Color.FromArgb(0xFF,0x00,0x00,0x00);
                default:
                    return Color.FromArgb(0xFF,0xFF,0xFF,0xFF);
            }
        }
    }
}
