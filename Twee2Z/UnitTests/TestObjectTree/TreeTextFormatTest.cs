using System;
using NUnit.Framework;
using Twee2Z.ObjectTree;
using Twee2Z.ObjectTree.PassageContents;

namespace UnitTests.TestObjectTree
{
    [TestFixture]
    public class TreeTextFormatTest
    {
        private const string _passageTag = Const.untTestFolder + "passageFormatText.tw";
        private const string _passageTagComplex = Const.untTestFolder + "passageFormatTextComplex.tw";

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

        [Test]
        public void TestTreePassageFormatTextComplex()
        {
            Tree tree = TreeBuilder.createTree(_passageTagComplex);
            Assert.AreEqual("Start", tree.StartPassage.Name);
            Assert.AreEqual(4, tree.Passages.Count);

            Assert.AreEqual(3, tree.StartPassage.PassageContentList.Count);
            Assert.IsFalse(tree.StartPassage.PassageContentList[0].ContentFormat.Italic);
            Assert.IsTrue(tree.StartPassage.PassageContentList[1].ContentFormat.Italic);
            Assert.IsFalse(tree.StartPassage.PassageContentList[2].ContentFormat.Italic);

            Assert.NotNull(tree.Passages["StoryTitle"]);
            Passage passage1 = tree.Passages["StoryTitle"];

            Assert.AreEqual(5, passage1.PassageContentList.Count);
            PassageContentFormat format = new PassageContentFormat();

            format.Italic = true;
            Assert.AreEqual("\r\nYour story wil::l di[s]]play ", passage1.PassageContentList[0].PassageText.Text);
            Assert.IsTrue(format.euquals(passage1.PassageContentList[0].ContentFormat));

            format.Bold = true;
            Assert.AreEqual("this passage", passage1.PassageContentList[1].PassageText.Text);
            Assert.IsTrue(format.euquals(passage1.PassageContentList[1].ContentFormat));

            format.Bold = false;
            Assert.AreEqual(" first Edit it by double clicking it\r\n", passage1.PassageContentList[2].PassageText.Text);
            Assert.IsTrue(format.euquals(passage1.PassageContentList[2].ContentFormat));

            Assert.AreEqual("HIERTEXT", passage1.PassageContentList[3].PassageLink.DisplayText);
            Assert.IsTrue(format.euquals(passage1.PassageContentList[3].ContentFormat));

            format.Italic = false;
            Assert.AreEqual("\r\nUntitled Story\r\n", passage1.PassageContentList[4].PassageText.Text);
            Assert.IsTrue(format.euquals(passage1.PassageContentList[4].ContentFormat));


            Assert.NotNull(tree.Passages["myPassage"]);
            Passage passage2 = tree.Passages["myPassage"];

            Assert.AreEqual(4, passage2.PassageContentList.Count);
            format = new PassageContentFormat();

            Assert.AreEqual("you are ", passage2.PassageContentList[0].PassageText.Text);
            Assert.IsTrue(format.euquals(passage2.PassageContentList[0].ContentFormat));

            format.Italic = true;
            Assert.AreEqual("done\r\n", passage2.PassageContentList[1].PassageText.Text);
            Assert.IsTrue(format.euquals(passage2.PassageContentList[1].ContentFormat));

            Assert.AreEqual("start", passage2.PassageContentList[2].PassageText.Text);
            Assert.IsTrue(format.euquals(passage2.PassageContentList[2].ContentFormat));

            Assert.AreEqual("\r\n", passage2.PassageContentList[3].PassageText.Text);
            Assert.IsTrue(format.euquals(passage2.PassageContentList[3].ContentFormat));


            Assert.NotNull(tree.Passages["StoryAuthor"]);
            Passage passage3 = tree.Passages["StoryAuthor"];

            Assert.AreEqual(1, passage3.PassageContentList.Count);
            format = new PassageContentFormat();

            Assert.AreEqual("Anonymous x\r\n", passage3.PassageContentList[0].PassageText.Text);
            Assert.IsTrue(format.euquals(passage3.PassageContentList[0].ContentFormat));
        }
    }
}
