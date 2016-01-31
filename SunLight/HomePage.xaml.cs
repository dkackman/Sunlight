using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Documents;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Sunlight.ViewModel;

namespace Sunlight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void SettingHyperLink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            var vm = GotoSettings.DataContext as SettingsViewModel;
            if (vm != null)
            {
                vm.GoToSettingsCommand.Execute(null);
            }
        }
    }
}
