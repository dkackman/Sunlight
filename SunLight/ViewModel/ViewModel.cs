using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;

namespace Sunlight.ViewModel
{
    public abstract class ViewModel : ViewModelBase
    {
        static ViewModel()
        {
            DispatcherHelper.Initialize();
        }

        private readonly INavigationService _navigationService;

        protected ViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        protected void NavigateTo(string page)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => _navigationService.NavigateTo(page));
        }
        protected void NavigateTo(string page, object state)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => _navigationService.NavigateTo(page, state));
        }

        protected void RaisePropertyChangedOnUI(string propertyName)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => RaisePropertyChanged(propertyName));
        }

        protected void RaisePropertiesChanged(params string[] properties)
        {
            foreach (string p in properties)
            {
                RaisePropertyChangedOnUI(p);
            }
        }
    }
}
