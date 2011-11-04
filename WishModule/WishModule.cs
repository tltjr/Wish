using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Wish.Views;

namespace Wish
{
    [ModuleExport(typeof(WishModule))]
    public class WishModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IRegion _mainRegion;

        [ImportingConstructor]
        public WishModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _mainRegion = _regionManager.Regions["MainRegion"];
        }

        public void Initialize()
        {
            var view = new WishView(_mainRegion, @"T:\src\dotnet");
            _mainRegion.Add(view);
        }
    }
}
