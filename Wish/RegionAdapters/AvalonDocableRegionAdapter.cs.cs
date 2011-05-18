using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Data;
using AvalonDock;
using Microsoft.Practices.Prism.Regions;

namespace Wish.Desktop.RegionAdapters
{
    public class AvalonDocableRegionAdapter : RegionAdapterBase<DockablePane>
    {
        public AvalonDocableRegionAdapter(IRegionBehaviorFactory factory) : base(factory) { }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, DockablePane regionTarget)
        {
            region.Views.CollectionChanged += delegate(Object sender, NotifyCollectionChangedEventArgs e)
            {
                OnViewsCollectionChanged(sender, e, region, regionTarget);
            };
        }

        private void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, IRegion region, DockablePane regionTarget)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //what if not view
                foreach (object item in e.NewItems)
                {
                    var view = item as FrameworkElement;
                    if (view != null)
                    {
                        var newContentPane = new DockableContent {Content = item};
                        var dc = (view).DataContext;

                        if (dc == null)
                            newContentPane.SetBinding(ManagedContent.TitleProperty, new Binding("Name") { Source = view });
                        else
                            SetName(newContentPane, dc);

                        //When contentPane is closed remove the associated region 
                        newContentPane.IsCloseable = true;
                        newContentPane.Closed += (contentPaneSender, args) =>
                        {

                        };
                        regionTarget.Items.Add(newContentPane);
                        newContentPane.Activate();
                    }
                }
            }
            else
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {

                }
            }
        }

        private static void SetName(DockableContent cont, object item)
        {
            cont.SetBinding(ManagedContent.TitleProperty, new Binding("Title") { Source = item });

        }
    }
}
