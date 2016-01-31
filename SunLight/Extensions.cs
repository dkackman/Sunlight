using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using System.Reflection;

namespace Sunlight
{
    public static class Extensions
    {
        public static string GetResourceText(this Assembly assembly, string resource)
        {
            using (var stream = assembly.GetManifestResourceStream("Sunlight." + resource))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
