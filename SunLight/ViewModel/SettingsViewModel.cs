using System.Collections.Generic;

using Sunlight.Model;
using Sunlight.Service;

namespace Sunlight.ViewModel
{
    public sealed class SettingsViewModel : ViewModel
    {
        private readonly ISettings _settings;

        public SettingsViewModel(ISettings settings, INavigationService2 navigationService)
            : base(navigationService)
        {
            _settings = settings;
        }

        public string Theme
        {
            get
            {
                return _settings.Theme;
            }
            set
            {
                _settings.Theme = value;
            }
        }

        public string ZipCode
        {
            get
            {
                return _settings.ZipCode;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.IndexOf(' ') >= 0)
                    {
                        value = value.Split(' ')[0];
                    }
                    _settings.ZipCode = value;
                }
            }
        }

        public IEnumerable<string> ThemeList => new List<string>() { "Light", "Dark" };
    }
}
