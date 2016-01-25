using Windows.UI.Xaml.Controls;

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SunLight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // TODO - refactor this out of here
            var n = SimpleIoc.Default.GetInstance<INavigationService>() as SunLight.Service.NavigationService;
            if (n.Root == null)
            {
                n.Root = SunLight.Controls.NavControl.NavRoot;
                n.NavigateTo("Home");
            }
        }
    }
}
