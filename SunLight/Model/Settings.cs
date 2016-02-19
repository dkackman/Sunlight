using System;
using System.Diagnostics;

using Windows.Storage;
using Windows.Foundation.Collections;

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
                return GetValue<string>("Theme", "Dark");
            }

            set
            {
                if (value != null && (value.Equals("Light", StringComparison.OrdinalIgnoreCase) || value.Equals("Dark", StringComparison.OrdinalIgnoreCase)))
                {
                    SetValue("Theme", value);
                }
                else
                {
                    Debug.Assert(false);
                }
            }
        }

        //public string ZipCode
        //{
        //    get
        //    {
        //        return GetValue("ZipCode", "");
        //    }

        //    set
        //    {
        //        if (string.IsNullOrEmpty(value) || value.Length != 5)
        //        {
        //            Remove("ZipCode");
        //        }
        //        else
        //        {
        //            SetValue("ZipCode", value);
        //        }
        //    }
        //}

        public T GetValue<T>(string key, T defaultValue)
        {
            try
            {
                if (_properties.ContainsKey(key))
                {
                    return (T)_properties[key];
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

        public void SetValue(string key, object value)
        {
            _properties[key] = value;
        }
    }
}
