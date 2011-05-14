using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Windows;
using AvalonDock;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using Werminal.RegionAdapters;

namespace Werminal
{
    class Bootstrapper : MefBootstrapper
    {
        private readonly CallbackLogger _callbackLogger = new CallbackLogger();

        protected override DependencyObject CreateShell()
        {
            return Container.GetExportedValue<Shell>("Shell");
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(WerminalModule.WerminalModule));
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {

            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();
            var regionBehaviourFactory = Container.GetExportedValue<IRegionBehaviorFactory>();
            mappings.RegisterMapping(typeof(DocumentPane), new AvalonDocumentRegionAdapter(regionBehaviourFactory));
            mappings.RegisterMapping(typeof(DockablePane), new AvalonDocableRegionAdapter(regionBehaviourFactory));

            return mappings;
        }

        protected override void ConfigureAggregateCatalog ( )
        {
            base.ConfigureAggregateCatalog ( );
			AggregateCatalog.Catalogs.Add ( new AssemblyCatalog ( typeof ( Shell ).Assembly ) );
            AggregateCatalog.Catalogs.Add ( new AssemblyCatalog ( typeof ( WerminalModule.WerminalModule ).Assembly));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.ComposeExportedValue(_callbackLogger);
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override ILoggerFacade CreateLogger()
        {
            return _callbackLogger;
        }
    }
}
