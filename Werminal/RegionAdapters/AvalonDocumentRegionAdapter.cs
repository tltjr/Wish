using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using AvalonDock;
using Microsoft.Practices.Prism.Regions;

namespace Werminal.RegionAdapters
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
            region.Views.CollectionChanged += delegate(Object sender, NotifyCollectionChangedEventArgs e)
            {
                OnViewsCollectionChanged(sender, e, region, regionTarget);
            };
        }
        private void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, IRegion region, DocumentPane regionTarget)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //Add content panes for each associated view. 
                foreach (object item in e.NewItems)
                {
                    var view = item as FrameworkElement;

                    if (view != null)
                    {
                        var newContentPane = new DocumentContent();
                        newContentPane.Content = item;
                        var dc = (view).DataContext;
                        if (dc != null)
                            SetName(newContentPane, dc);
                        else
                            SetName(newContentPane, view);
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
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    var o = item;
                    var cont = regionTarget.Items.OfType<DocumentContent>().FirstOrDefault(x => x.Content == o);
                    if (cont != null)
                        cont.Close();
                }

            }
        }

        private static void SetName(DocumentContent cont, object item)
        {
            cont.SetBinding(ManagedContent.TitleProperty, new Binding("Title") { Source = item });
        }
    }
}
