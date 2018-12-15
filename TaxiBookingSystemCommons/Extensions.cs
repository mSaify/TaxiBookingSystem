using System;
using System.ComponentModel;
using System.Reflection;

namespace TaxiBookingSystemCommons
{
    public static class Extensions
    {
        public static string ToDescription(this Enum enumeration)
        {
            string value = enumeration.ToString();
            Type type = enumeration.GetType();
            //Use reflection to try and get the description attribute for the enumeration
            DescriptionAttribute[] descAttribute = (DescriptionAttribute[])type.GetField(value).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descAttribute.Length > 0 ? descAttribute[0].Description : value;
        }
    }
}
