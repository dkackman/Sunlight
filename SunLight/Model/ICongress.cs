using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunlight.Model
{
    public interface ICongress
    {
        Task<dynamic> GetDistricts(string zipCode);
        Task<dynamic> GetFirstDistrict(string zipCode);
    }
}
