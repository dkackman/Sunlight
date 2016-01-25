using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

using GalaSoft.MvvmLight.Views;

namespace SunLight.Service
{
    sealed class NavigationService : INavigationService
    {
        private readonly IDictionary<string, Type> _pages = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private Frame _root;

        public NavigationService()
        {
        }

        public Frame Root
        {
            get { return _root; }
            internal set
            {
                _root = value;
                _root.Navigated += _root_Navigated;
                SystemNavigationManager.GetForCurrentView().BackRequested += (o, e) => GoBack();
            }
        }

        private void _root_Navigated(object sender, NavigationEventArgs e)
        {
            if (Root.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        public void Configure(string key, Type type)
        {
            _pages.Add(key, type);
        }

        public string CurrentPageKey { get; private set; }

        public void GoBack()
        {
            if (Root.CanGoBack)
            {
                Root.GoBack();
            }
        }

        public void NavigateTo(string pageKey)
        {
            bool b = Root.Navigate(_pages[pageKey]);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            Root.Navigate(_pages[pageKey], parameter);
        }
    }
}
