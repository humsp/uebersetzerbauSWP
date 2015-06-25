using System;
using NUnit.Framework;
using System.IO;
using Twee2Z.Analyzer;
using Twee2Z.ObjectTree;
using Twee2Z.Console;
using System.Collections.Generic;
using System.Linq;


namespace UnitTests.TestObjectTree
{
    [TestFixture]
    public class TreeLinkTest
    {
        private const string _passageLinkPath = Const.untTestFolder + "passageLink.tw";
        private const string _passageLinkValidationPath = Const.untTestFolder + "passageLinkValidation.tw";


        [Test]
        public void TestTreePassageLink()
        {

            Tree tree = TreeBuilder.createTree(_passageLinkPath);
            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual(4, tree.Passages.Count);

            // 1 Passage
            Passage startPassage = tree.StartPassage;
            Assert.AreEqual(28, startPassage.PassageContentList.Count);
            String _passageText = "";  //Text Position 0-26
            for (int i = 0; i < 26; i++)
            {
                _passageText = _passageText + startPassage.PassageContentList[i].PassageText.Text;
            }
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it\r\n",
                _passageText);
            //Link Position 26
            PassageLink startPassageLink = startPassage.PassageContentList[26].PassageLink;
            Assert.AreEqual("HIERTEXT", startPassageLink.DisplayText);
            Assert.AreEqual("myPassage", startPassageLink.Target);
            Assert.AreEqual(tree.Passages["myPassage"], startPassageLink.TargetPassage);
            Assert.AreEqual(null, startPassageLink.Expression);
            Assert.AreEqual("\r\n", startPassage.PassageContentList[27].PassageText.Text);

            // 2 Passage
            Passage sndPassage = tree.StoryTitle;
            _passageText = "";//Text Position 0-37
            for (int i = 0; i < 38; i++)
            {
                _passageText = _passageText + sndPassage.PassageContentList[i].PassageText.Text;
            }
            Assert.AreEqual(38, sndPassage.PassageContentList.Count);
            Assert.AreEqual("Your story wil::l di[s]]play this passage first Edit it by double clicking it\r\nUntitled Story\r\n",
                _passageText);


            // 3 Passage
            Passage thirdPassage = tree.Passages["myPassage"];
            Assert.AreEqual(8, thirdPassage.PassageContentList.Count);
            _passageText = "";//Text Position 0-5
            for (int i = 0; i < 6; i++)
            {
                _passageText = _passageText + thirdPassage.PassageContentList[i].PassageText.Text;
            }
            Assert.AreEqual("you are done\r\n", _passageText);

            PassageLink thirdPassageLink = thirdPassage.PassageContentList[6].PassageLink;
            Assert.AreEqual("start", thirdPassageLink.Target);
            Assert.AreEqual(startPassage, thirdPassageLink.TargetPassage);
            Assert.AreEqual(null, thirdPassageLink.Expression);
            Assert.AreEqual(null, thirdPassageLink.DisplayText);
            Assert.AreEqual("\r\n", thirdPassage.PassageContentList[7].PassageText.Text);

            // 4 Passage
            Passage fourthPassage = tree.StoryAuthor;
            Assert.AreEqual(4, fourthPassage.PassageContentList.Count);
            _passageText = "";//Text Position 0-3
            for (int i = 0; i < 4; i++)
            {
                _passageText = _passageText + fourthPassage.PassageContentList[i].PassageText.Text;
            }
            Assert.AreEqual("Anonymous x\r\n", _passageText);
        }

        [Test]
        public void TestTreePassageLinkValidation()
        {
            Tree tree = TreeBuilder.createTree(_passageLinkValidationPath);
            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual(2, tree.Passages.Count);

            // 1 Passage
            Passage startPassage = tree.StartPassage;
            Assert.AreEqual(27, startPassage.PassageContentList.Count);
            String _passageText = "";  //Text Position 0-26
            for (int i = 0; i < 24; i++)
            {
                _passageText = _passageText + startPassage.PassageContentList[i].PassageText.Text;
            }
            Assert.AreEqual("Your story wilplay this passage first Edit it by double clicking it\r\n",
                _passageText);

            PassageLink startPassageLink = startPassage.PassageContentList[24].PassageLink;
            Assert.IsNull(startPassageLink);
            Assert.AreEqual("\r\n", startPassage.PassageContentList[25].PassageText.Text);
            Assert.AreEqual("\r\n", startPassage.PassageContentList[26].PassageText.Text);

            // 2 Passage
            Passage fourthPassage = tree.Passages["StoryAuthor"];
            Assert.AreEqual(4, fourthPassage.PassageContentList.Count);
            Assert.AreEqual("Anonymous", fourthPassage.PassageContentList[0].PassageText.Text);
            Assert.AreEqual(" ", fourthPassage.PassageContentList[1].PassageText.Text);
            Assert.AreEqual("x", fourthPassage.PassageContentList[2].PassageText.Text);
            Assert.AreEqual("\r\n", fourthPassage.PassageContentList[3].PassageText.Text);

        }
    }
}
