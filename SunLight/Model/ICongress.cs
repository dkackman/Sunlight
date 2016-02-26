﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunlight.Model
{
    public interface ICongress
    {
        Task<dynamic> FindLegislators(double lat, double @long);
        Task<dynamic> GetLegislator(string bioguide_id);

        Task<dynamic> FindDistricts(string zipCode);
        Task<dynamic> FindFirstDistrict(string zipCode);

        Task<dynamic> FindDistricts(double lat, double @long);
        Task<dynamic> FindFirstDistrict(double lat, double @long);

        Task<dynamic> GetUpcomingBills();
        Task<dynamic> GetUpcomingBills(string chamber);

        Task<dynamic> GetHearings();
        Task<dynamic> GetHearings(string chamber);

        Task<dynamic> GetNextPage(dynamic currentPage);
    }
}