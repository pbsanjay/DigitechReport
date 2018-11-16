using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitechReport
{
    public class DigitechData
    {
        public int Id { get; set; }

        public string Tile { get; set; }

        public int Count { get; set; }

        public DateTime ReportDateTime { get; set; }

        public string FlightNumber { get; set; }
    }
}
