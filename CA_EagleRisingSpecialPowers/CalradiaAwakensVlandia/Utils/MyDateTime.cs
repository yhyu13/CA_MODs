using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CA_EagleRisingSpecialPowers.Utils
{
    public static class MyDateTime
    {
        public static string GetLocalTime(string regionName = "en-US")
        {
            DateTime localDate = DateTime.Now;
            var region = new CultureInfo(regionName);
            return ($"{regionName}: {localDate.ToString(region)}");
        }
    }
}
