using System;
using System.Linq;
using System.Reflection;

namespace DeltaTrack.Helpers.Extensions
{
    public static class EnumExtensions
    {
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo.First().GetCustomAttributes(typeof (T), false);

            return attributes.Any() ? (T)attributes.First() : null;
        }
    }
}
