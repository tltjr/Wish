using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Wish.Menu.Views;

namespace Wish.Menu
{
    [ModuleExport(typeof(MenuModule))]
    public class MenuModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IRegion _menuRegion;

        [ImportingConstructor]
        public MenuModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _menuRegion = _regionManager.Regions["MenuRegion"];
        }

        public void Initialize()
        {
            var view = new MenuView(_menuRegion);
            _menuRegion.Add(view);
        }
    }
}
