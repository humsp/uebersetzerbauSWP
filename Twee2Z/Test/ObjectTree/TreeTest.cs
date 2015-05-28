﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Twee2Z.Analyzer;
using Twee2Z.ObjectTree;
using Twee2Z.Console;
using System.Collections.Generic;
using System.Linq;


namespace Test.ObjectTree
{
    [TestClass]
    public class TreeTest
    {
        const string untTestFolder = "../../../TestFiles/UnitTestFiles/";
        const string passageOnlyPath = untTestFolder + "passageOnly.tw";
        const string passageLinkPath = untTestFolder + "passageLink.tw";
        const string passageLinkValidationPath = untTestFolder + "passageLinkValidation.tw";

        [TestMethod]
        public void TestTreePassageOnly()
        {
            Tree tree = createTree(passageOnlyPath);
            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual(1, tree.Passages.Count);
            Assert.AreEqual(1, tree.StartPassage.PassageContentList.Count);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it\r\n\r\n",
                tree.StartPassage.PassageContentList[0].PassageText.Text);
        }

        [TestMethod]
        public void TestTreePassageLink()
        {
            Tree tree = createTree(passageLinkPath);
            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual(4, tree.Passages.Count);

            // 1 Passage
            Passage startPassage = tree.StartPassage;
            Assert.AreEqual(3, startPassage.PassageContentList.Count);
            Assert.AreEqual("Your story wilplay this passage first Edit it by double clicking it\r\n",
                startPassage.PassageContentList[0].PassageText.Text);

            PassageLink startPassageLink = startPassage.PassageContentList[1].PassageLink;
            Assert.AreEqual("HIERTEXT", startPassageLink.DisplayText);
            Assert.AreEqual("myPassage", startPassageLink.Target);
            Assert.AreEqual(tree.Passages["myPassage"], startPassageLink.TargetPassage);
            Assert.AreEqual(null, startPassageLink.Expression);
            Assert.AreEqual("\r\n", startPassage.PassageContentList[2].PassageText.Text);

            // 2 Passage
            Passage sndPassage = tree.Passages["StoryTitle"];
            Assert.AreEqual(1, sndPassage.PassageContentList.Count);
            Assert.AreEqual("Your story wil::l di[s]]play this passage first Edit it by double clicking it\r\nUntitled Story\r\n", 
                sndPassage.PassageContentList[0].PassageText.Text);


            // 3 Passage
            Passage thirdPassage = tree.Passages["myPassage"];
            Assert.AreEqual(3, thirdPassage.PassageContentList.Count);
            Assert.AreEqual("you are done\r\n", thirdPassage.PassageContentList[0].PassageText.Text);
            
            PassageLink thirdPassageLink = thirdPassage.PassageContentList[1].PassageLink;
            Assert.AreEqual("start", thirdPassageLink.Target);
            Assert.AreEqual(startPassage, thirdPassageLink.TargetPassage);
            Assert.AreEqual(null, thirdPassageLink.Expression);
            Assert.AreEqual(null, thirdPassageLink.DisplayText);
            Assert.AreEqual("\r\n", thirdPassage.PassageContentList[2].PassageText.Text);

            // 4 Passage
            Passage fourthPassage = tree.Passages["StoryAuthor"];
            Assert.AreEqual(1, fourthPassage.PassageContentList.Count);
            Assert.AreEqual("Anonymous x\r\n", fourthPassage.PassageContentList[0].PassageText.Text);
        }

        [TestMethod]
        public void TestTreePassageLinkValidation()
        {
            Tree tree = createTree(passageLinkValidationPath);
            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual(2, tree.Passages.Count);

            // 1 Passage
            Passage startPassage = tree.StartPassage;
            Assert.AreEqual(3, startPassage.PassageContentList.Count);
            Assert.AreEqual("Your story wilplay this passage first Edit it by double clicking it\r\n",
                startPassage.PassageContentList[0].PassageText.Text);

            PassageLink startPassageLink = startPassage.PassageContentList[1].PassageLink;
            Assert.IsNull(startPassageLink);
            Assert.AreEqual("HIERTEXT", startPassage.PassageContentList[1].PassageText.Text);
            Assert.AreEqual("\r\n\r\n", startPassage.PassageContentList[2].PassageText.Text);

            // 2 Passage
            Passage fourthPassage = tree.Passages["StoryAuthor"];
            Assert.AreEqual(2, fourthPassage.PassageContentList.Count);
            Assert.AreEqual("Anonymous ", fourthPassage.PassageContentList[0].PassageText.Text);
            Assert.AreEqual("x\r\n", fourthPassage.PassageContentList[1].PassageText.Text);
        
        }

        [TestMethod]
        public void TestTreePassageTag()
        {
            Tree tree = createTree(passageLinkPath);
            Assert.AreEqual("start", tree.StartPassage.Name);
            Assert.AreEqual(4, tree.Passages.Count);

            Assert.AreEqual(3, tree.StartPassage.PassageContentList.Count);

            HashSet<string> tags = tree.StartPassage.Tags;

            Assert.AreEqual(3, tags.Count());

            Assert.IsTrue(tags.Contains("tag1"));
            Assert.IsTrue(tags.Contains("tag2"));
            Assert.IsTrue(tags.Contains("tag3"));
        }


        private Tree createTree(string tweeFile)
        {
            FileStream tweeFileStream = new FileStream(tweeFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            Tree tree = Program.AnalyseFile(tweeFileStream);
            Program.ValidateTree(tree);
            return tree;
        }
    }
}
