﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Dynamic;

using BingGeocoder;

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
        private readonly IGeoCoder _locator;
        private Location _location;

        public SettingsViewModel(ISettings settings, Keys keys, IGeoCoder locator, ICongress congress, INavigationService2 navigationService)
            : base(navigationService)
        {
            _settings = settings;
            _congress = congress;
            _zipSearchVm = new ZipCodeSearchViewModel(navigationService);
            _geoVM = new GeoLocationViewModel(keys, navigationService);
            _locator = locator;

            _location = _settings.Location;
            if (_location != null)
            {
                _zipcode = _location.ZipCode;
            }

            _district = new RemoteResult<dynamic>(() => _congress.FindFirstDistrict(_location.Lat, _location.Long), 
                () => RaisePropertiesChanged("District"), new ExpandoObject());
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

        private string _zipcode;
        public string ZipCode
        {
            get
            {
                return _zipcode;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.IndexOf(' ') >= 0)
                {
                    // this bit is for when the user selects the zip + city, state auto suggesiton
                    // it strips out the zip and just remembers that
                    value = value.Split(' ')[0];
                }
                _zipcode = value;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                SetLocationFromZip();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                ZipCodeSearch.SearchTerm = _zipcode;

                RaisePropertiesChanged("IsLocationValid");
            }
        }

        private async Task SetLocationFromZip()
        {
            if (_zipSearchVm.IsValidZip(_zipcode))
            {
                var coord = await _locator.GetCoordinate("", "", "", _zipcode, "US");

                _geoVM.SetLocation(coord.Item1, coord.Item2);
                _location = new Location()
                {
                    ZipCode = _zipcode,
                    Lat = coord.Item1,
                    Long = coord.Item2
                };

                _settings.Location = _location;
            }
            _district.Reset();
        }

        public bool SettingsValid => _settings.Location != null;

        public IEnumerable<string> ThemeList => new List<string>() { "Light", "Dark" };

        private readonly RemoteResult<dynamic> _district;

        public dynamic District
        {
            get
            {
                if (SettingsValid)
                {
                    _district.Execute();
                }

                return _district.Result;
            }
        }
    }
}
