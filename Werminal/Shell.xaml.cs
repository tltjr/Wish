using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;

namespace Werminal
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
            //this.TraceTextBox.AppendText(
            //                                string.Format(
            //                                            CultureInfo.CurrentUICulture, 
            //                                            "[{0}][{1}] {2}\r\n", 
            //                                            category,
            //                                            priority, 
            //                                            message));
        }

        public void OnImportsSatisfied()
        {
			_logger.Callback = this.Log;
			_logger.ReplaySavedLogs ( );
            //possible event aggregator subscriptions
        }
    }
}
