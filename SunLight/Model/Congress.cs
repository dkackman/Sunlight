using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DynamicRestProxy.PortableHttpClient;

namespace Sunlight.Model
{
    class Congress : ICongress
    {
        private dynamic _client;

        public Congress(string apiKey)
        {
            var defaults = new DynamicRestClientDefaults();
            defaults.DefaultHeaders.Add("X-APIKEY", apiKey);
            _client = new DynamicRestClient("https://congress.api.sunlightfoundation.com/", defaults);
        }

        public async Task<dynamic> GetDistricts(string zipCode)
        {
            return await _client.districts.locate.get(zip: zipCode);
        }

        public async Task<dynamic> GetFirstDistrict(string zipCode)
        {
            dynamic list = await GetDistricts(zipCode);
            return list.count > 0 ? list.results[0] : null;
        }
    }
}
