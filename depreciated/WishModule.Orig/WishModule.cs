using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using WishModule.Views;

namespace WishModule
{
    [ModuleExport(typeof(WishModule))]
    public class WishModule : IModule
    {
        private readonly ILoggerFacade _logger;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegion _mainRegion;

        [ImportingConstructor]
        public WishModule(ILoggerFacade logger, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _logger = logger;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _mainRegion = _regionManager.Regions["MainRegion"];
        }

        public void Initialize()
        {
            var view = new WishView(_mainRegion, _eventAggregator);
            _mainRegion.Add(view);
        }
    }
}
