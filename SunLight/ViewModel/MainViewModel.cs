using System;
using System.Collections.ObjectModel;

using Windows.System;

using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

namespace Sunlight.ViewModel
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            ToggleNavCommand = new RelayCommand(() => IsNavOpen = !IsNavOpen);

            MainNavItems = new ObservableCollection<NavItem>()
            {
                new NavItem()
                {
                    Text = "Home",
                    ButtonText = "\uE80F",
                    Command = new RelayCommand(() => NavigateTo("Home"))
                }
            };
            SecondaryNavItems = new ObservableCollection<NavItem>()
            {
                new NavItem()
                {
                    Text = "Visit the sunlight foundation",
                    ButtonText = "\uE909",
                    Command = new RelayCommand(() =>
                    {
                        var uri = new Uri("http://sunlightfoundation.com/", UriKind.Absolute);
                        Launcher.LaunchUriAsync(uri);
                    })
                },
                new NavItem()
                {
                    Text = "Settings",
                    ButtonText = "\uE713",
                    Command = new RelayCommand(() => NavigateTo("Settings"))
                }
            };

            // do this asynchronously on the dispatcher so that the UI is full instantiated
            // before we attempt to navigate (see comment in NavControl.xaml.cs)
            DispatcherHelper.RunAsync(() => base.NavigateTo("Home"));
        }

        private bool _isNavOpen;
        public bool IsNavOpen
        {
            get { return _isNavOpen; }
            set
            {
                Set<bool>(ref _isNavOpen, value);
            }
        }

        public RelayCommand ToggleNavCommand { get; private set; }

        public ObservableCollection<NavItem> MainNavItems { get; private set; }

        public ObservableCollection<NavItem> SecondaryNavItems { get; private set; }
    }
}
