using System.Windows.Input;

namespace SunLight
{
    public sealed class NavItem
    {
        public string Text { get; set; }
        public string ButtonText { get; set; }

        public ICommand Command { get; set; }
    }
}
