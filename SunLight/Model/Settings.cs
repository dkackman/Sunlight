using System;
using System.Diagnostics;
using System.Reflection;

using Windows.Storage;
using Windows.Foundation.Collections;

using Newtonsoft.Json;

namespace Sunlight.Model
{
    sealed class Settings : ISettings
    {
        private readonly IPropertySet _properties;

        public Settings(ApplicationDataContainer container)
        {
            _properties = container.Values;
        }

        public Location Location
        {
            get
            {
                return GetValue<Location>("Location", null);
            }

            set
            {
                if (value?.IsValid == true)
                {
                    SetValue("Location", value);
                }
                else
                {
                    Remove("Location");
                }
            }
        }

        public string Theme
        {
            get
            {
                return GetValue<string>("Theme", "Light");
            }

            set
            {
                if ("Light".Equals(value, StringComparison.OrdinalIgnoreCase) || "Dark".Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    SetValue("Theme", value);
                }
                else
                {
                    Debug.Assert(false);
                }
            }
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            try
            {
                if (_properties.ContainsKey(key))
                {
                    var value = _properties[key];
                    if (typeof(T) != typeof(string) && !typeof(T).GetTypeInfo().IsPrimitive)
                    {
                        return JsonConvert.DeserializeObject<T>(value.ToString());
                    }

                    return (T)value;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error deserializing setting {key} - {e.Message}");
            }
            return defaultValue;
        }

        public void Remove(string key)
        {
            if (_properties.ContainsKey(key))
            {
                _properties.Remove(key);
            }
        }

        public void SetValue<T>(string key, T value)
        {
            if (typeof(T) != typeof(string) && !typeof(T).GetTypeInfo().IsPrimitive)
            {
                var json = JsonConvert.SerializeObject(value);
                SetValue<string>(key, json);
            }
            else
            {
                _properties[key] = value;
            }
        }
    }
}
