using Wish.Common;

namespace Wish.Commands
{
    public class History : UniqueList<ICommand>
    {
        public int Index = -1;

        public ICommand Up()
        {
            if (Count == 0) return null;
            Index++;
            if(Index > Count - 1)
            {
                Index = -1;
            }
            return Index == -1 ? new Command(null, string.Empty) : this[Index];
        }

        public ICommand Down()
        {
            if (Count == 0) return null;
            Index--;
            if (Index == -1)
            {
                return new Command(null, string.Empty);
            }
            if (Index < -1)
            {
                Index = Count - 1;
            }
            return this[Index];
        }

        public void Reset()
        {
            Index = -1;
        }
    }
}
