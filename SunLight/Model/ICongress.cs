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

        Task<dynamic> GetUpcomingBills();
        Task<dynamic> GetUpcomingBills(string chamber);

        Task<dynamic> GetHearings();
        Task<dynamic> GetHearings(string chamber);

        Task<dynamic> GetNextPage(dynamic currentPage);
    }
}