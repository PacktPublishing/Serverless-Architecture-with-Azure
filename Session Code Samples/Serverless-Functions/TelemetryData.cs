using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessFunctions
{
    public class TelemetryData
    {
        public string devicename { get; set; }

        public string name { get; set; }

        public string value { get; set; }
    }

    public class TelemetryDataCollection
    {
        public TelemetryData[] TelemetryDataList { get; set; }
    }
}
