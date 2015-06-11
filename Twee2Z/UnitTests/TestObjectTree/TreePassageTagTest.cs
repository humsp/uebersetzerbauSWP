using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twee2Z.ObjectTree;

namespace UnitTests.TestObjectTree
{
    [TestClass]
    class TreePassageTagTest
    {
        private const string _passageLinkPath = Const.untTestFolder + "passageLink.tw";

        [TestMethod]
        public void TestTreePassageTag()
        {
            Tree tree = TreeBuilder.createTree(_passageLinkPath);
            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual(4, tree.Passages.Count);

            Assert.AreEqual(3, tree.StartPassage.PassageContentList.Count);

            HashSet<string> tags = tree.StartPassage.Tags;

            Assert.AreEqual(3, tags.Count());

            Assert.IsTrue(tags.Contains("tag1"));
            Assert.IsTrue(tags.Contains("tag2"));
            Assert.IsTrue(tags.Contains("tag3"));
        }
    }
}
