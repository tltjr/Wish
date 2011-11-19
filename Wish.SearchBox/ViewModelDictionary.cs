using Wish.SearchBox.ViewModels;
using VmDictionary = System.Collections.Generic.Dictionary<Wish.SearchBox.SearchType, Wish.SearchBox.ViewModels.SearchBoxViewModel>;

namespace Wish.SearchBox
{
    public enum SearchType
    {
        RecentDirectories,
        CommandHistory,
        RecentArguments
    };

    public class ViewModelDictionary
    {
        private readonly VmDictionary _dictionary = new VmDictionary();

        public ViewModelDictionary()
        {
            _dictionary.Add(SearchType.CommandHistory, new CommandHistoryViewModel());
            _dictionary.Add(SearchType.RecentDirectories, new RecentDirectoriesViewModel());
            _dictionary.Add(SearchType.RecentArguments, new RecentArgumentsViewModel());
        }

        public SearchBoxViewModel this[SearchType type]
        {
            get { return _dictionary[type]; }
        }
    }
}
