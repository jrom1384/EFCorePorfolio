using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace EFCore.Utilities
{
    public class DisplayNameHelper
    {
        public static string GetAttributeDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            if (atts.Length > 0)
            {
                return (atts[0] as DisplayNameAttribute).DisplayName;
            }

            return string.Empty;
        }
    }
}
