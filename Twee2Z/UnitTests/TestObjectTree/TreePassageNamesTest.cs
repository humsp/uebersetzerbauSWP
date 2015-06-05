using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twee2Z.ObjectTree;

namespace UnitTests.TestObjectTree
{
    [TestClass]
    public class TreePassageNamesTest
    {
        private const string _passageOnlyPath = Const.untTestFolder + "passageOnly.tw";
        private const string _passageNames = Const.untTestFolder + "passageNames.tw";
        private const string _passageNamesComment = Const.untTestFolder + "passageNamesComment.tw";
        private const string _passageNamesIgnoreAllBeforeStart = Const.untTestFolder + "passageNamesIgnoreAllBeforeStart.tw";
        private const string _passageNamesInvalid = Const.untTestFolder + "passageNamesInvalid.tw";
        private const string _passageNamesWS = Const.untTestFolder + "passageNamesWS.tw";

        [TestMethod]
        public void TestTreePassageOnly()
        {
            Tree tree = TreeBuilder.createTree(_passageOnlyPath);
            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual(1, tree.Passages.Count);
            Assert.AreEqual(1, tree.StartPassage.PassageContentList.Count);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it\r\n\r\n",
                tree.StartPassage.PassageContentList[0].PassageText.Text);
        }

        [TestMethod]
        public void TestTreePassageNameWithComment()
        {
            Tree tree = TreeBuilder.createTree(_passageNamesComment);
            Assert.AreEqual(2, tree.Passages.Count);
            
            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it", tree.StartPassage.PassageContentList[0].PassageText.Text);

            Assert.IsTrue(tree.Passages.ContainsKey("o/%Text%/ther"));
            Assert.AreEqual("o/%Text%/ther", tree.Passages["o/%Text%/ther"].Name);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it", tree.Passages["o/%Text%/ther"].PassageContentList[0].PassageText.Text);
        }


        [TestMethod]
        public void TestTreePassageNamesIgnoreAllBeforeStart()
        {
            Tree tree = TreeBuilder.createTree(_passageNamesIgnoreAllBeforeStart);
            Assert.AreEqual(1, tree.Passages.Count);

            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it", tree.StartPassage.PassageContentList[0].PassageText.Text);
        }

        [TestMethod]
        public void TestTreePassageNamesWS()
        {
            Tree tree = TreeBuilder.createTree(_passageNamesWS);
            Assert.AreEqual(4, tree.Passages.Count);

            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it", tree.StartPassage.PassageContentList[0].PassageText.Text);

            Assert.IsTrue(tree.Passages.ContainsKey("ohneWS"));
            Assert.AreEqual("ohneWS", tree.Passages["ohneWS"].Name);
            Assert.AreEqual("x", tree.Passages["ohneWS"].PassageContentList[0].PassageText.Text);

            Assert.IsTrue(tree.Passages.ContainsKey("fuehrenderWS"));
            Assert.AreEqual("führenderWS", tree.Passages["führenderWS"].Name);
            Assert.AreEqual("x", tree.Passages["führenderWS"].PassageContentList[0].PassageText.Text);

            Assert.IsTrue(tree.Passages.ContainsKey("Passage mit 	Tab	und WS am Ende"));
            Assert.AreEqual("Passage mit 	Tab	und WS am Ende", tree.Passages["Passage mit 	Tab	und WS am Ende"].Name);
            Assert.AreEqual("ende", tree.Passages["Passage mit 	Tab	und WS am Ende"].PassageContentList[0].PassageText.Text);
        }

        [TestMethod]
        public void TestTreePassageNamesInvalid()
        {
            Tree tree = TreeBuilder.createTree(_passageNamesInvalid);
            Assert.AreEqual(2, tree.Passages.Count);

            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual("x", tree.StartPassage.PassageContentList[0].PassageText.Text);
            
            Assert.IsTrue(tree.Passages.ContainsKey("__underline__"));
            Assert.AreEqual("__underline__", tree.Passages["__underline__"].Name);
            Assert.AreEqual("end", tree.Passages["__underline__"].PassageContentList[0].PassageText.Text);
        }
    }
}
