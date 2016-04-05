using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Sunlight.Model
{
    class ZipCodeDb
    {
        private readonly string _db;

        public ZipCodeDb()
        {
            _db = GetType().GetTypeInfo().Assembly.GetResourceText("Data.ZipCodes.txt");
        }

        public IEnumerable<ZipCode> Find(string match)
        {
            match = Regex.Escape(match);

            var list = new List<ZipCode>();
            foreach (Match m in Regex.Matches(_db, $"^{match}.*$", RegexOptions.Multiline))
            {
                if (!string.IsNullOrEmpty(m.Value))
                {
                    var pair = m.Value.Split('\t');
                    list.Add(new ZipCode()
                    {
                        Zip = pair[0],
                        CityState = pair[1].Trim()
                    });
                }
            }

            return list;
        }
    }
}


