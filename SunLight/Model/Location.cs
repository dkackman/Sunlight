using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunlight.Model
{
    public sealed class Location
    {
        public string ZipCode { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(ZipCode) && ZipCode.Length == 5) || (Lat != 0 && Long != 0);
            }
        }

        public static bool IsValidZip(string zip)
        {
            return !string.IsNullOrWhiteSpace(zip) && zip.Length == 5;
        }
    }
}
