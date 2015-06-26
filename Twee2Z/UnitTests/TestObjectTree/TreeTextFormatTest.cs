using System;
using NUnit.Framework;
using Twee2Z.ObjectTree;

namespace UnitTests.TestObjectTree
{
    [TestFixture]
    public class TreeTextFormatTest
    {
        private const string _passageTag = Const.untTestFolder + "passageFormatText.tw";

        [Test]
        public void TestTreePassageFormatTextSimple()
        {
            Tree tree = TreeBuilder.createTree(_passageTag);
            Assert.AreEqual("Start", tree.StartPassage.Name);
            Assert.AreEqual(4, tree.Passages.Count);

            Assert.AreEqual(3, tree.StartPassage.PassageContentList.Count);
            Assert.IsFalse(tree.StartPassage.PassageContentList[0].ContentFormat.Italic);
            Assert.IsTrue(tree.StartPassage.PassageContentList[1].ContentFormat.Italic);
            Assert.IsFalse(tree.StartPassage.PassageContentList[2].ContentFormat.Italic);
        }
    }
}
