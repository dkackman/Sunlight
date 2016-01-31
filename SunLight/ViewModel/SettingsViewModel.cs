using System.Collections.Generic;

using GalaSoft.MvvmLight.Command;

using Sunlight.Model;
using Sunlight.Service;

namespace Sunlight.ViewModel
{
    public sealed class SettingsViewModel : ViewModel
    {
        private readonly ISettings _settings;
        private readonly ZipCodeSearchViewModel _zipSearchVm;
        private readonly GeoLocationViewModel _geoVM;

        public SettingsViewModel(ISettings settings, Keys keys, INavigationService2 navigationService)
            : base(navigationService)
        {
            _settings = settings;
            _zipSearchVm = new ZipCodeSearchViewModel(navigationService);
            _geoVM = new GeoLocationViewModel(keys, navigationService);
        }

        public RelayCommand GoToSettingsCommand => new RelayCommand(() => NavigateTo("Settings"));

        public ZipCodeSearchViewModel ZipCodeSearch => _zipSearchVm;

        public GeoLocationViewModel GeoLocation => _geoVM;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        public RelayCommand GeoLocateCommand => new RelayCommand(() => _geoVM.GetLocation());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

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
                RaisePropertiesChanged("IsZipCodeValid");
            }
        }

        public bool IsZipCodeValid { get { return !string.IsNullOrEmpty(ZipCode) && ZipCode.Length == 5; } }

        public IEnumerable<string> ThemeList => new List<string>() { "Light", "Dark" };
    }
}
