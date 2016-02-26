using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Dynamic;

using DynamicRestProxy.PortableHttpClient;

using Newtonsoft.Json;

namespace Sunlight.Model
{
    public class Congress : ICongress
    {
        private dynamic _client;

        public Congress(string apiKey)
        {
            var defaults = new DynamicRestClientDefaults();
            defaults.DefaultHeaders.Add("X-APIKEY", apiKey);
            _client = new DynamicRestClient("https://congress.api.sunlightfoundation.com/", defaults);
        }

        public async Task<dynamic> GetNextPage(dynamic currentPage)
        {
            var page = currentPage?.page;
            if (page == null)
            {
                throw new InvalidOperationException("Can't determine the current page for this call");
            }

            long totalCount = currentPage.count;
            long current = page.page;
            long pageCount = page.per_page;

            if (pageCount * current >= totalCount)
            {
                return null; // no more pages
            }

            Uri uri = page.uri;
            IDictionary<string, object> args = uri.ParseQueryString();
            args.Add("page", current + 1);

            return await _client(uri.LocalPath).get(p: args);
        }

        public async Task<dynamic> FindLegislators(double lat, double @long)
        {
            return await _client.districts.legislators.locate.get(latitude: lat, longitude: @long);
        }
        public async Task<dynamic> GetLegislator(string bioguide_id)
        {
            return await _client.districts.legislators.get(bioguide_id: bioguide_id);
        }

        public async Task<dynamic> FindDistricts(string zipCode)
        {
            return await _client.districts.locate.get(zip: zipCode);
        }
        public async Task<dynamic> FindFirstDistrict(string zipCode)
        {
            dynamic list = await FindDistricts(zipCode);
            return list.count > 0 ? list.results[0] : null;
        }

        public async Task<dynamic> FindDistricts(double lat, double @long)
        {
            return await _client.districts.locate.get(latitude: lat, longitude: @long);
        }
        public async Task<dynamic> FindFirstDistrict(double lat, double @long)
        {
            dynamic list = await FindDistricts(lat, @long);
            return list.count > 0 ? list.results[0] : null;
        }

        public async Task<dynamic> GetUpcomingBills()
        {
            return await PackageResult(await _client.upcoming_bills.get(typeof(HttpResponseMessage)));
        }
        public async Task<dynamic> GetUpcomingBills(string chamber)
        {
            return await PackageResult(await _client.upcoming_bills.get(typeof(HttpResponseMessage), chamber: chamber));
        }

        public async Task<dynamic> GetHearings()
        {
            return await PackageResult(await _client.hearings.get(typeof(HttpResponseMessage)));
        }
        public async Task<dynamic> GetHearings(string chamber)
        {
            return await PackageResult(await _client.hearings.get(typeof(HttpResponseMessage), chamber: chamber));
        }

        private static async Task<dynamic> PackageResult(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new DynamicRestClientResponseException(response);
            }

            var content = await response.Content.ReadAsStringAsync();

            // we squirrel away the request uri so that we can get subsequent pages later
            dynamic d = JsonConvert.DeserializeObject<ExpandoObject>(content);
            if (d.page != null)
            {
                d.page.uri = response.RequestMessage.RequestUri;
            }
            response.Dispose();
            return d;
        }
    }
}
