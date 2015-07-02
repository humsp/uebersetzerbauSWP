using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Twee2Z.ObjectTree;

namespace Twee2Z.UnitTests.TestObjectTree
{
    [TestFixture]
    class TreePassageTagTest
    {
        private const string _passageTag = Const.untTestFolder + "passageTag.tw";

        [Test]
        public void TestTreePassageTag()
        {
            Tree tree = TreeBuilder.createTree(_passageTag);
            Assert.AreEqual("Start", tree.StartPassage.Name);
            Assert.AreEqual(4, tree.Passages.Count);

            Assert.AreEqual(3, tree.StartPassage.PassageContentList.Count);

            HashSet<string> tags = tree.StartPassage.Tags;

            Assert.AreEqual(3, tags.Count());

            Assert.IsTrue(tags.Contains("a"));
            Assert.IsTrue(tags.Contains("b"));
            Assert.IsTrue(tags.Contains("c"));
        }
    }
}
