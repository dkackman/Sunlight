using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Views;

using Sunlight.Model;

namespace Sunlight.ViewModel
{
    public sealed class SettingsViewModel : ViewModel
    {
        private readonly ISettings _settings;

        public SettingsViewModel(ISettings settings, INavigationService navigationService)
            : base(navigationService)
        {
            _settings = settings;
        }
    }
}
