using System;
using NUnit.Framework;
using Twee2Z.ObjectTree;

namespace Twee2Z.UnitTests.TestObjectTree
{
    [TestFixture]
    public class TreePassageNamesTest
    {
        private const string _passageOnlyPath = Const.untTestFolder + "passageOnly.tw";
        private const string _passageNames = Const.untTestFolder + "passageNames.tw";
        private const string _passageNamesComment = Const.untTestFolder + "passageNamesComment.tw";
        private const string _passageNamesIgnoreAllBeforeStart = Const.untTestFolder + "passageNamesIgnoreAllBeforeStart.tw";
        private const string _passageNamesInvalidToken1 = Const.untTestFolder + "passageNamesInvalidToken1.tw";
        private const string _passageNamesInvalidToken2 = Const.untTestFolder + "passageNamesInvalidToken2.tw";
        private const string _passageNamesInvalidToken3 = Const.untTestFolder + "passageNamesInvalidToken3.tw";
        private const string _passageNamesInvalidToken4 = Const.untTestFolder + "passageNamesInvalidToken4.tw";
        private const string _passageNamesInvalidToken5 = Const.untTestFolder + "passageNamesInvalidToken5.tw";
        private const string _passageNamesInvalidToken6 = Const.untTestFolder + "passageNamesInvalidToken6.tw";
        private const string _passageNamesInvalidFormat = Const.untTestFolder + "passageNamesInvalidFormat.tw";
        private const string _passageNamesWS = Const.untTestFolder + "passageNamesWS.tw";

        [Test]
        public void TestTreePassageOnly()
        {
            Tree tree = TreeBuilder.createTree(_passageOnlyPath);
            Assert.AreEqual("Start", tree.StartPassage.Name);
            Assert.AreEqual(3, tree.Passages.Count);
            Assert.AreEqual(1, tree.StartPassage.PassageContentList.Count);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it\r\n\r\n",
                tree.StartPassage.PassageContentList[0].PassageText.Text);
        }

        [Test]
        public void TestTreePassageNameWithComment()
        {
            Tree tree = TreeBuilder.createTree(_passageNamesComment);
            Assert.AreEqual(2, tree.Passages.Count);
            
            Assert.AreEqual("Start", tree.StartPassage.Name);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it", tree.StartPassage.PassageContentList[0].PassageText.Text);

            Assert.IsTrue(tree.Passages.ContainsKey("o/%Text%/ther"));
            Assert.AreEqual("o/%Text%/ther", tree.Passages["o/%Text%/ther"].Name);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it", tree.Passages["o/%Text%/ther"].PassageContentList[0].PassageText.Text);
        }


        [Test]
        public void TestTreePassageNamesIgnoreAllBeforeStart()
        {
            Tree tree = TreeBuilder.createTree(_passageNamesIgnoreAllBeforeStart);
            Assert.AreEqual(3, tree.Passages.Count);

            Assert.AreEqual("Start", tree.StartPassage.Name);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it", tree.StartPassage.PassageContentList[0].PassageText.Text);
        }

        [Test]
        public void TestTreePassageNamesWS()
        {
            Tree tree = TreeBuilder.createTree(_passageNamesWS);
            Assert.AreEqual(6, tree.Passages.Count);

            Assert.AreEqual("Start", tree.StartPassage.Name);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it", tree.StartPassage.PassageContentList[0].PassageText.Text);

            Assert.IsTrue(tree.Passages.ContainsKey("ohneWS"));
            Assert.AreEqual("ohneWS", tree.Passages["ohneWS"].Name);
            Assert.AreEqual("x", tree.Passages["ohneWS"].PassageContentList[0].PassageText.Text);

            Assert.IsTrue(tree.Passages.ContainsKey("fuehrenderWS"));
            Assert.AreEqual("fuehrenderWS", tree.Passages["fuehrenderWS"].Name);
            Assert.AreEqual("x", tree.Passages["fuehrenderWS"].PassageContentList[0].PassageText.Text);

            Assert.IsTrue(tree.Passages.ContainsKey("Passage mit 	Tab	und WS am Ende"));
            Assert.AreEqual("Passage mit 	Tab	und WS am Ende", tree.Passages["Passage mit 	Tab	und WS am Ende"].Name);
            Assert.AreEqual("ende", tree.Passages["Passage mit 	Tab	und WS am Ende"].PassageContentList[0].PassageText.Text);
        }

        [Test]
        public void TestTreePassageNamesInvalid()
        {
            TestInvalidToken(_passageNamesInvalidFormat, "__underline__", "underline");
        }

        [Test]
        public void TestTreePassageNamesInvalidToken1()
        {
            TestInvalidToken(_passageNamesInvalidToken1, "toke|n", "token");
        }

        [Test]
        public void TestTreePassageNamesInvalidToken2()
        {
            TestInvalidToken(_passageNamesInvalidToken2, "toke$n", "token");
        }

        [Test]
        public void TestTreePassageNamesInvalidToken3()
        {
            TestInvalidToken(_passageNamesInvalidToken3, "toke<n", "token");
        }

        [Test]
        public void TestTreePassageNamesInvalidToken4()
        {
            TestInvalidToken(_passageNamesInvalidToken4, "toke>n", "token");
        }

        [Test]
        public void TestTreePassageNamesInvalidToken5()
        {
            TestInvalidToken(_passageNamesInvalidToken5, "toke[n", "token");
        }

        [Test]
        public void TestTreePassageNamesInvalidToken6()
        {
            TestInvalidToken(_passageNamesInvalidToken6, "toke]n", "token");
        }


        private void TestInvalidToken(string file, string falseName, string rightName)
        {
            Tree tree = TreeBuilder.createTree(file);
            Assert.AreEqual(4, tree.Passages.Count);

            Assert.AreEqual("Start", tree.StartPassage.Name);
            Assert.AreEqual("x", tree.StartPassage.PassageContentList[0].PassageText.Text);

            Assert.IsFalse(tree.Passages.ContainsKey(falseName));
            Assert.IsTrue(tree.Passages.ContainsKey(rightName));
            Assert.AreEqual(rightName, tree.Passages[rightName].Name);
            Assert.AreEqual("end", tree.Passages[rightName].PassageContentList[0].PassageText.Text);
        }
    }
}
