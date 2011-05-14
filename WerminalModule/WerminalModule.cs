using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace WerminalModule
{
    public class WerminalModule : IModule
    {
        private readonly IRegionViewRegistry _regionViewRegistry;

        public WerminalModule(IRegionViewRegistry registry)
        {
            this._regionViewRegistry = registry;   
        }

        public void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion("MainRegion", typeof(Views.WerminalView));
        }
    }
}
