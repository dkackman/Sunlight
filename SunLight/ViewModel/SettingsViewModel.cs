using System.Collections.Generic;

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

        public IEnumerable<string> ThemeList => new List<string>() { "Light", "Dark" };
    }
}
