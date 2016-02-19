using System;
using System.Collections.Generic;

using Windows.System;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

using Sunlight.Model;
using Sunlight.Service;

namespace Sunlight.ViewModel
{
    public sealed class RecentActivityViewModel : ViewModel
    {
        private readonly ISettings _settings;
        private readonly ICongress _congress;
        private readonly ICurrentCongressionalSession _currentSession;

        public RecentActivityViewModel(ISettings settings, ICongress congress, ICurrentCongressionalSession currentSession, INavigationService2 navigationService)
            : base(navigationService)
        {
            _settings = settings;
            _congress = congress;
            _currentSession = currentSession;

            _upcomingBills = new RemoteResult<dynamic>(() => _congress.GetUpcomingBills(), () => RaisePropertiesChanged("UpcomingBills"), null);
        }

        private readonly RemoteResult<dynamic> _upcomingBills;
        public dynamic UpcomingBills
        {
            get
            {
                _upcomingBills.Execute();

                return _upcomingBills.Result;
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
