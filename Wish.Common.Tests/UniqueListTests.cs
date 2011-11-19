using NUnit.Framework;

namespace Wish.Common.Tests
{
    [TestFixture]
    public class UniqueListTests
    {
        private UniqueList<string> _uniqueList;

        [SetUp]
        public void Init()
        {
            _uniqueList = new UniqueList<string> {"blah"};
        }

        [Test]
        public void RegularAdd()
        {
            Assert.AreEqual("blah", _uniqueList[0]);
        }

        [Test]
        public void RegularAddCount()
        {
            Assert.AreEqual(1, _uniqueList.Count);
        }

        [Test]
        public void NoDups()
        {
            _uniqueList.Add("blah");
            Assert.AreEqual(1, _uniqueList.Count);
        }

        [Test]
        public void NoNulls()
        {
            _uniqueList.Add(null);
            Assert.AreEqual(1, _uniqueList.Count);
        }

        [Test]
        public void AddRangeEnforcesSameRulesAsAdd()
        {
            var ins = new System.Collections.Generic.List<string> {"one", "one", null};
            _uniqueList = new UniqueList<string>();
            _uniqueList.AddRange(ins);
            Assert.AreEqual(1, _uniqueList.Count);
        }
    }
}
