using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moduler.Helpers
{
    public static class SerializeHelper
    {
        public static string ToJson<T>(this T model)
        {
            return JsonConvert.SerializeObject(model);
        }
        public static T FindIndex<T>(this string model)
        {
            return JsonConvert.DeserializeObject<T>(model);
        }
    }
}
