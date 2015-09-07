using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDi.Configuration
{
    public class AppFabricConfig
    {
        public string BaseUrl { get; set; }
        public WebDrivers Browser { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WebDrivers { Firefox, IE, Chrome, PhantomJS, Edge }
}
