﻿using System;
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

            _upcomingBills = new RemoteResult<dynamic>(async () => await _congress.GetUpcomingBills(), () => RaisePropertiesChanged("UpcomingBills"), null);
        }

        private readonly RemoteResult<dynamic> _upcomingBills;

        public dynamic UpcomingBills
        {
            get
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _upcomingBills.Execute();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

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
