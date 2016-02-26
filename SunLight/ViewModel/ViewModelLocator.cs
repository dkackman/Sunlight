using System.Diagnostics.CodeAnalysis;

using Windows.Storage;

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

using Microsoft.Practices.ServiceLocation;

using Sunlight.Model;
using Sunlight.Service;

using BingGeocoder;

namespace Sunlight.ViewModel
{
    public sealed class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // setup app services
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<Keys>();
            var keys = SimpleIoc.Default.GetInstance<Keys>();
            SimpleIoc.Default.Register<IGeoCoder>(() =>
            {
                return new GeoCoder((string)keys.Data.BingLocations);
            });
            SimpleIoc.Default.Register<INavigationService2>(() =>
            {
                var nav = new Sunlight.Service.NavigationService();
                nav.Configure("Home", typeof(HomePage));
                nav.Configure("Settings", typeof(SettingsPage));

                return nav;
            });

            // setup models
            SimpleIoc.Default.Register<IAbout, About>();
            SimpleIoc.Default.Register<ISettings>(() => new Settings(ApplicationData.Current.LocalSettings));
            SimpleIoc.Default.Register<ICurrentCongressionalSession, CurrentCongressionalSession>(true); // race condition in here and I don't feel like building synchronization
            SimpleIoc.Default.Register<ICongress>(() =>
            {
                return new Congress((string)keys.Data.Sunlight);
            });

            // setup view models
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AboutViewModel About => ServiceLocator.Current.GetInstance<AboutViewModel>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
    }
}
