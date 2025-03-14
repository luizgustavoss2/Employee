using System;
using System.ComponentModel;

namespace GS.Employee.Infra.CrossCutting.Extension
{
    public static class EnumExtension
    {
        private static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes == null || attributes.Length == 0)
                return value.ToString();

            return attributes[0].Description;
        }

        public static string GetDescription(this Enum value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!Enum.IsDefined(value.GetType(), value))
                throw new ArgumentNullException(nameof(value));

            return GetEnumDescription(value);
        }

        public static string TryGetDescription(this Enum value, string defaultValue = "")
        {
            try
            {
                if (value == null)
                    return defaultValue;

                if (!Enum.IsDefined(value.GetType(), value))
                    return defaultValue;

                return GetEnumDescription(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
