using System.Linq;

namespace EFCore.Utilities
{
    public static class StringHelper
    {
        public static T Trim<T>(this T input)
        {
            var stringProperties = input.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue != null)
                {
                    stringProperty.SetValue(input, currentValue.Trim(), null);
                }
            }

            return input;
        }
    }
}
