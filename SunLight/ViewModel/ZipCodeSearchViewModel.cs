using System.Collections.Generic;
using System.Linq;

using Sunlight.Service;
using Sunlight.Model;

namespace Sunlight.ViewModel
{
    public class ZipCodeSearchViewModel : ViewModel
    {
        private readonly ZipCodeDb _zipCodes = new ZipCodeDb();

        public ZipCodeSearchViewModel(INavigationService2 navigationService)
            : base(navigationService)
        {
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                Set<string>(ref _searchTerm, value);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Matches = _zipCodes.Find(_searchTerm);
                }
            }
        }

        private IEnumerable<ZipCode> _matches;
        public IEnumerable<ZipCode> Matches
        {
            get { return _matches; }
            private set
            {
                _matches = value;
                RaisePropertyChangedOnUI("Matches");
                RaisePropertyChangedOnUI("NoResults");
            }
        }

        public bool NoResults
        {
            get { return _matches != null && !_matches.Any(); }
        }
    }
}
