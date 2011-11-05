using System.Collections.Generic;

namespace Wish.Commands
{
    public abstract class Completable
    {
        public abstract IEnumerable<string> Complete();
    }
}
