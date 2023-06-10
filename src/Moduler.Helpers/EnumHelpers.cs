using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Moduler.Helpers
{
    public static class EnumHelpers
    {
        public static string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()?
           .GetMember(enumValue.ToString())?[0]?
           .GetCustomAttribute<DisplayAttribute>()?
           .Name;
        }

        public static string GetDescription(this Enum enumm)
        {
            Type type = enumm.GetType();
            System.Reflection.MemberInfo[] memInfo = type.GetMember(enumm.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((System.ComponentModel.DataAnnotations.DisplayAttribute)attrs[0]).Name;
            }
            return enumm.ToString();
        }
    }
}
