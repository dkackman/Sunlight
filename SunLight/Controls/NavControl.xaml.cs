using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using GalaSoft.MvvmLight.Command;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SunLight.Controls
{
    public sealed partial class NavControl : UserControl
    {
        internal static Frame NavRoot { get; private set; }

        public NavControl()
        {
            this.InitializeComponent();
            NavRoot = this.MainFrame;
        }
    }
}
