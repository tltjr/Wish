using System;
using System.Collections.Generic;
using Wish.Views;

namespace Wish
{
    public class TabManager
    {
        private static TabManager _instance;
        private readonly IList<IndexedTab> _list = new List<IndexedTab>();
        private int _currentTab;
        private static int _index;

        public static TabManager Instance()
        {
            return _instance ?? (_instance = new TabManager());
        }

        public void Add(WishView view)
        {
            _list.Add(new IndexedTab
                          {
                              WishView = view,
                              Index = ++_index
                          });
            _currentTab = 0;
        }

        public WishView Next()
        {
            if(_index < _list.Count - 1)
            {
                return _list[++_index].WishView;
            }
            _currentTab = 0;
            return _list[0].WishView;
        }

        public WishView Previous()
        {
            if(_currentTab > 0)
            {
                return _list[--_currentTab].WishView;
            }
            _currentTab = _list.Count - 1;
            return _list[_currentTab].WishView;
        }
    }

    internal class IndexedTab
    {
        public WishView WishView { get; set; }
        public int Index { get; set; }
    }
}
