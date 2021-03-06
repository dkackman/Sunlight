﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;
using Sunlight.Service;

namespace Sunlight.ViewModel
{
    public class MapItem
    {
        public Geopoint Location { get; set; }
    }
    public sealed class GeoLocationViewModel : ViewModel
    {
        private readonly Keys _keys;

        public GeoLocationViewModel(Keys keys, INavigationService2 navigationService)
            : base(navigationService)
        {
            var b = new BasicGeoposition()
            {
                Latitude = 0,
                Longitude = 0
            };
            _location = new Geopoint(b);

            _keys = keys;
        }

        private GeolocationAccessStatus _accessStatus = GeolocationAccessStatus.Unspecified;
        public GeolocationAccessStatus AccessStatus
        {
            get { return _accessStatus; }
            set
            {
                if (_accessStatus != value)
                {
                    _accessStatus = value;
                    RaisePropertyChangedOnUI("AccessStatus");
                    RaisePropertyChangedOnUI("CanGeoLocate");
                }
            }
        }

        public bool ShowLocation
        {
            get
            {
                return Location.Position.Latitude != 0 || Location.Position.Longitude != 0;
            }
        }

        public bool CanGeoLocate
        {
            get
            {
                return !string.IsNullOrEmpty(Token) && AccessStatus == GeolocationAccessStatus.Allowed;
            }
        }

        public IEnumerable<MapItem> MapItems
        {
            get
            {
                return new List<MapItem>()
                {
                    new MapItem() { Location = Location }
                };
            }
        }

        private Geopoint _location;
        public Geopoint Location
        {
            get { return _location; }
            set { } // prevent map scrolling from setting this
        }

        public void SetLocation(double lat, double @long)
        {
            var b = new BasicGeoposition()
            {
                Latitude = lat,
                Longitude = @long
            };
            _location = new Geopoint(b);
            RaisePropertiesChanged("Location");
            RaisePropertyChangedOnUI("ShowLocation");
        }

        public string Token { get { return _keys.Data.BingMaps; } }

        public async Task GetLocation()
        {
            AccessStatus = await Geolocator.RequestAccessAsync();
            if (AccessStatus == GeolocationAccessStatus.Allowed)
            {
                var geolocator = new Geolocator();
                var pos = await geolocator.GetGeopositionAsync();

                _location = pos.Coordinate.Point;

                RaisePropertyChanged("Location");
                RaisePropertyChangedOnUI("ShowLocation");
            }
        }
    }
}
