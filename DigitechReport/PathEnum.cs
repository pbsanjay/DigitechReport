using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitechReport
{
    public static class PathEnum
    {
        public const string
            Cart = "/passengers/cart",
            Excursions = "/passengers/cruises/itinerary/{id}/excursions",
            Order = "/passengers/cart/order";
    }
}
