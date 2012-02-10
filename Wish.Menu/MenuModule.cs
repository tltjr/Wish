using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Wish.Menu.Views;
using Wish.ViewModels;

namespace Wish.Menu
{
    [ModuleExport(typeof(MenuModule))]
    public class MenuModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IRegion _menuRegion;
        private readonly WishViewModel _viewModel;

        [ImportingConstructor]
        public MenuModule(IRegionManager regionManager, WishViewModel viewModel)
        {
            _regionManager = regionManager;
            _viewModel = viewModel;
            _menuRegion = _regionManager.Regions["MenuRegion"];
        }

        public void Initialize()
        {
            var view = new MenuView(_viewModel);
            _menuRegion.Add(view);
        }
    }
}
