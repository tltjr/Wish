using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;

namespace Wish.Desktop
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    [Export("Shell")]
    public partial class Shell : IPartImportsSatisfiedNotification
    {
		[Import ( AllowRecomposition = false )]
		private CallbackLogger _logger;

		[Import]
		private IEventAggregator _eventAggregator;

        public Shell()
        {
            InitializeComponent();
        }

        public void Log(string message, Category category, Priority priority)
        {
        }

        public void OnImportsSatisfied()
        {
			_logger.Callback = Log;
			_logger.ReplaySavedLogs ( );
            //possible event aggregator subscriptions
        }
    }
}
