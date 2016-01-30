using System.Collections.Generic;

using Sunlight.Model;
using Sunlight.Service;

namespace Sunlight.ViewModel
{
    public sealed class SettingsViewModel : ViewModel
    {
        private readonly ISettings _settings;
        private readonly ZipCodeSearchViewModel _zipSearchVm;

        public SettingsViewModel(ISettings settings, INavigationService2 navigationService)
            : base(navigationService)
        {
            _settings = settings;
            _zipSearchVm = new ZipCodeSearchViewModel(navigationService);
        }

        public ZipCodeSearchViewModel ZipCodeSearch => _zipSearchVm;

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
                if (!string.IsNullOrEmpty(value) && value.IndexOf(' ') >= 0)
                {
                    // this bit is for when the user selects the zip + city, state auto suggesiton
                    // it strips out the zip and just remembers that
                    value = value.Split(' ')[0];
                }

                _settings.ZipCode = value;
                ZipCodeSearch.SearchTerm = value;
            }
        }

        public IEnumerable<string> ThemeList => new List<string>() { "Light", "Dark" };
    }
}
