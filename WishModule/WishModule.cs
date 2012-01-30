using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Wish.ViewModels;
using Wish.Views;

namespace Wish
{
    [ModuleExport(typeof(WishModule))]
    public class WishModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IRegion _mainRegion;
        private readonly WishViewModel _viewModel;

        [ImportingConstructor]
        public WishModule(IRegionManager regionManager, WishViewModel viewModel)
        {
            _regionManager = regionManager;
            _viewModel = viewModel;
            _mainRegion = _regionManager.Regions["MainRegion"];
        }

        public void Initialize()
        {
            var view = new WishView(_mainRegion, _viewModel);
            _mainRegion.Add(view);
        }
    }
}
