using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Windows.Storage;

using Sunlight.Model;

namespace Sunlight.Model
{
    class Settings : ISettings
    {
        private readonly ApplicationDataContainer _container;
        public Settings(ApplicationDataContainer container)
        {
            _container = container;
        }

        public string Theme
        {
            get
            {
                return GetValue<string>("Theme", "Dark");
            }

            set
            {
                SetValue("Theme", value);
            }
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            try
            {
                if (_container.Values.ContainsKey(key))
                {
                    return (T)_container.Values[key];
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error deserializing setting {key} - {e.Message}");
            }
            return defaultValue;
        }

        public void SetValue(string key, object value)
        {
            _container.Values["key"] = value;
        }
    }
}
