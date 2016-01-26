using System;
using System.Windows.Input;

using Windows.System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

using Sunlight.Model;

namespace Sunlight.ViewModel
{
    public sealed class AboutViewModel : ViewModel
    {
        private readonly IAbout _about;

        public AboutViewModel(IAbout about, INavigationService navigationService)
            : base(navigationService)
        {
            _about = about;

            RateCommand = new RelayCommand(() => Launcher.LaunchUriAsync(new Uri($"ms-windows-store:REVIEW?PFN={_about.PackageName}")));

            //FeedbackCommand = new RelayCommand(() => Launcher.LaunchUriAsync(
            //        new Uri($"ms-windows-store:REVIEW?PFN={_about.PackageName}")));
        }

        public IAbout Model => _about;

        public ICommand RateCommand { get; private set; }

        public ICommand FeedbackCommand { get; private set; }
    }
}

