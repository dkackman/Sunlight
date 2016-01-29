using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunlight.Model
{
    sealed class ZipCode
    {
        public string Zip {get;set;}

        public string CityState { get; set; }

        public override string ToString()
        {
            return $"{Zip} ({CityState})";
        }
    }
}
