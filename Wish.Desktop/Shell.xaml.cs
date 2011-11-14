using System.ComponentModel.Composition;

namespace Wish.Desktop
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    [Export("Shell")]
    public partial class Shell : IPartImportsSatisfiedNotification
    {
        public Shell()
        {
            InitializeComponent();
        }
        public void OnImportsSatisfied() { }
    }
}
