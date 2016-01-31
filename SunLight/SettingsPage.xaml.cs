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

using Sunlight.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Sunlight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
          //  if (args.ChosenSuggestion != null)
          //  {
          //      //User selected an item, take an action on it here
          //      var zipCode = args.ChosenSuggestion as ZipCode;
          //      sender.Text = zipCode.Zip;
          //  }
          //  else
          //  {
          //      //Do a fuzzy search on the query text
          //      var zipCode = _zipCodes.Find(args.QueryText).FirstOrDefault();

          //      if (zipCode != null)
          //      {
          //          //Choose the first match
          //          sender.Text = zipCode.Zip;
          //      }
          //      else
          //      {
          ////          NoResults.Visibility = Visibility.Visible;
          //      }
          //  }
        }

        private void MyMapControl_CenterChanged(Windows.UI.Xaml.Controls.Maps.MapControl sender, object args)
        {

        }
    }
}
