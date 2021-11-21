using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorContextMenu.DemoApp.Shared
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public bool Favorite { get; set; }
        public bool Highlight { get; set; }
    }
}
