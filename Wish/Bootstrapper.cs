using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using AvalonDock;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Wish.Desktop.RegionAdapters;

namespace Wish.Desktop
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
            moduleCatalog.AddModule(typeof(WishModule));
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
            AggregateCatalog.Catalogs.Add ( new AssemblyCatalog ( typeof ( WishModule ).Assembly));
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
