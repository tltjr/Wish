using System.Linq;
using NSpec;

namespace Wish.Extensions.Tests
{
    class chunk_spec : nspec
    {
        private int[] _list;

        void describe_chunk()
        {
            context["given a list"] = () =>
                                          {
                                              before = () => _list = new[] {1, 2};
                                              it["can be chunked"] = () => _list.Chunk(1).First().Count().should_be(1);
                                          };
        }
    }
}
