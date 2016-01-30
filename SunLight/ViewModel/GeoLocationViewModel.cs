using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;

using Sunlight.Model;
using Sunlight.Service;

namespace Sunlight.ViewModel
{
    public sealed class GeoLocationViewModel : ViewModel
    {
        public GeoLocationViewModel(INavigationService2 navigationService)
            : base(navigationService)
        {
            var b = new BasicGeoposition();
            b.Latitude = 48.279301;
            b.Longitude = 11.582600;
            Location = new Geopoint(b);
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

        public bool CanGeoLocate
        {
            get
            {
                return !string.IsNullOrEmpty(Token) && AccessStatus == GeolocationAccessStatus.Allowed;
            }
        }

        public Geopoint Location { get; private set; }

        public string Token { get; private set; }

        public async Task GetLocation() 
        {
            AccessStatus = await Geolocator.RequestAccessAsync();
            if (AccessStatus == GeolocationAccessStatus.Allowed)
            {
                Geolocator geolocator = new Geolocator();
                Geoposition pos = await geolocator.GetGeopositionAsync();

                Location = pos.Coordinate.Point;

                RaisePropertyChangedOnUI("Location");
            }
        }
    }
}
