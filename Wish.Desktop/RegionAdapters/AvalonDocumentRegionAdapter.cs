using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AvalonDock;
using Microsoft.Practices.Prism.Regions;

namespace Wish.Desktop.RegionAdapters
{
    public sealed class AvalonDocumentRegionAdapter : RegionAdapterBase<DocumentPane>
    {
        public AvalonDocumentRegionAdapter(IRegionBehaviorFactory factory) : base(factory) { }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }

        protected override void Adapt(IRegion region, DocumentPane regionTarget)
        {
            region.Views.CollectionChanged += (sender, e) => OnViewsCollectionChanged(e, regionTarget);
        }

        private static void OnViewsCollectionChanged(NotifyCollectionChangedEventArgs e, ItemsControl regionTarget)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var view = item as FrameworkElement;

                        if (view == null) continue;
                        var newContentPane = new DocumentContent {Content = item};
                        var dc = (view).DataContext;
                        SetName(newContentPane, dc ?? view);
                        //When contentPane is closed remove the associated region 
                        newContentPane.IsCloseable = true;
                        newContentPane.Closed += (contentPaneSender, args) =>
                                                     {

                                                     };
                        regionTarget.Items.Add(newContentPane);

                        newContentPane.Activate();
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var o = item;
                        var cont = regionTarget.Items.OfType<DocumentContent>().FirstOrDefault(x => x.Content == o);
                        if (cont != null)
                            cont.Close();
                    }
                    break;
            }
        }

        private static void SetName(FrameworkElement cont, object item)
        {
            cont.SetBinding(ManagedContent.TitleProperty, new Binding("Title") { Source = item });
        }
    }
}
