using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace WerminalModule
{
    [ModuleExport(typeof(WerminalModule))]
    public class WerminalModule : IModule
    {
        private readonly ILoggerFacade _logger;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegion _mainRegion;

        [ImportingConstructor]
        public WerminalModule(ILoggerFacade logger, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _logger = logger;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _mainRegion = _regionManager.Regions["MainRegion"];
        }

        public void Initialize()
        {
            var view = new Views.WerminalView();
            _mainRegion.Add(view);
        }
    }
}
