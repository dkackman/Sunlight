using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Command;

using Sunlight.Model;
using Sunlight.Service;

namespace Sunlight.ViewModel
{
    public sealed class HomeViewModel : ViewModel
    {
        private readonly ISettings _settings;
        private readonly ICongress _congress;
        private readonly ICurrentCongressionalSession _currentSession;

        public HomeViewModel(ISettings settings, ICongress congress, ICurrentCongressionalSession currentSession, INavigationService2 navigationService)
            : base(navigationService)
        {
            _settings = settings;
            _congress = congress;
            _currentSession = currentSession;

            _upcomingBills = new RemoteResult<dynamic>(() => _congress.GetUpcomingBills(), () => RaisePropertiesChanged("UpcomingBills"), null);
            _legislators = new RemoteResult<dynamic>(() => _congress.FindLegislators(_settings.Location.Lat, _settings.Location.Long), () => RaisePropertiesChanged("Legislators"), null);

            SelectCommand = new RelayCommand<dynamic>(o =>
            {
                var vm = new DynamicViewModel(_navigationService, o);
                string id = o.bioguide_id as string;

                vm.AppendModel(new Dictionary<string, Func<Task<dynamic>>>()
                    {
                        { "committees", () => _congress.GetCommittees(id) },
                        { "sponsored_bills", () => _congress.GetBills(id) }
                    });

                NavigateTo("LegislatorDetail", vm);
            });
        }

        public RelayCommand<dynamic> SelectCommand { get; private set; }

        private readonly RemoteResult<dynamic> _upcomingBills;
        public dynamic UpcomingBills
        {
            get
            {
                _upcomingBills.Execute();

                return _upcomingBills.Result;
            }
        }

        private readonly RemoteResult<dynamic> _legislators;
        public dynamic Legislators
        {
            get
            {
                if (_settings.Location.IsValid)
                {
                    _legislators.Execute();
                }

                return _legislators.Result;
            }
        }

        public string CongressionalSession
        {
            get
            {
                return _currentSession.Current.ToString();
            }
        }
    }
}
