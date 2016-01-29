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
        private readonly ZipCodeDb _zipCodes = new ZipCodeDb();

        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            NoResults.Visibility = Visibility.Collapsed;

            //We only want to get results when it was a user typing, 
            //otherwise we assume the value got filled in by TextMemberPath 
            //or the handler for SuggestionChosen
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var zipList = _zipCodes.Find(sender.Text);

                sender.ItemsSource = zipList.ToList();
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                //User selected an item, take an action on it here
                var zipCode = args.ChosenSuggestion as ZipCode;
                sender.Text = zipCode.Zip;
            }
            else
            {
                //Do a fuzzy search on the query text
                var zipCode = _zipCodes.Find(args.QueryText).FirstOrDefault();

                if (zipCode != null)
                {
                    //Choose the first match
                    sender.Text = zipCode.Zip;
                }
                else
                {
                    NoResults.Visibility = Visibility.Visible;
                }
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var zipCode = args.SelectedItem as ZipCode;
            sender.Text = zipCode.Zip;
        }
    }
}
