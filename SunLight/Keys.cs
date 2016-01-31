using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Newtonsoft.Json;

namespace Sunlight
{
    public sealed class Keys
    {
        public Keys()
        {
            var json = GetType().GetTypeInfo().Assembly.GetResourceText("keys.txt");
            Data = JsonConvert.DeserializeObject(json);
        }

        public dynamic Data { get; private set; }
    }
}
