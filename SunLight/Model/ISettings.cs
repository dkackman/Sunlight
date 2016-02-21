using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunlight.Model
{
    public interface ISettings
    {
        string Theme { get; set; }

        Location Location { get; set; }

        T GetValue<T>(string key, T defaultValue);

        void SetValue<T>(string key, T value);

        void Remove(string key);
    }
}
