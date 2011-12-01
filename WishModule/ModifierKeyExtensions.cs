using System.Windows.Input;

namespace Wish
{
    public static class ModifierKeyExtensions
    {
        public static bool Any(this ModifierKeys keys)
        {
            return keys == ModifierKeys.Windows ||
                   keys == ModifierKeys.Shift ||
                   keys == ModifierKeys.Control ||
                   keys == ModifierKeys.Alt ||
                   keys == (ModifierKeys.Windows | ModifierKeys.Shift) ||
                   keys == (ModifierKeys.Windows | ModifierKeys.Control) ||
                   keys == (ModifierKeys.Windows | ModifierKeys.Alt) ||
                   keys == (ModifierKeys.Shift | ModifierKeys.Control) ||
                   keys == (ModifierKeys.Shift | ModifierKeys.Alt) ||
                   keys == (ModifierKeys.Control | ModifierKeys.Alt);
        }
    }
}
