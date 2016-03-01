using System;
using System.Collections.Generic;

using Windows.System;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

using Sunlight.Model;
using Sunlight.Service;

namespace Sunlight.ViewModel
{
    public class DynamicViewModel : ViewModel
    {
        public DynamicViewModel(INavigationService2 navigationService, dynamic model)
            : base(navigationService)
        {
            Model = model;
        }

        public dynamic Model { get; private set; }

        public string SomeText { get { return "dddd"; } }
    }
}
