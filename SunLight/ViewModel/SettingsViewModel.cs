using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Command;

using Sunlight.Model;
using Sunlight.Service;

namespace Sunlight.ViewModel
{
    public sealed class SettingsViewModel : ViewModel
    {
        private readonly ISettings _settings;
        private readonly ICongress _congress;
        private readonly ZipCodeSearchViewModel _zipSearchVm;
        private readonly GeoLocationViewModel _geoVM;

        public SettingsViewModel(ISettings settings, Keys keys, ICongress congress, INavigationService2 navigationService)
            : base(navigationService)
        {
            _settings = settings;
            _congress = congress;
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
                _district = null;
                RaisePropertiesChanged("District");
            }
        }

        public bool IsZipCodeValid { get { return !string.IsNullOrEmpty(ZipCode) && ZipCode.Length == 5; } }

        public IEnumerable<string> ThemeList => new List<string>() { "Light", "Dark" };

        private RemoteResult<dynamic> _district = new RemoteResult<dynamic>();
        private bool _asking;
        private dynamic _district;
        public dynamic District
        {
            get
            {
                if (IsZipCodeValid && _district == null && !_asking)
                {
                    _asking = true;
                    Task.Run<dynamic>(async () =>
                    {
                        return await _congress.GetFirstDistrict(ZipCode);
                    }).ContinueWith((antecedent) =>
                    {
                        try
                        {
                            if (antecedent.Result != null)
                            {
                                _district = antecedent.Result;
                                RaisePropertiesChanged("District");
                            }
                        }
                        catch (Exception)
                        {
                        }
                        _asking = false;
                    });
                }

                return _district;
            }
        }
    }
}
