using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Twee2Z.Analyzer;
using Twee2Z.ObjectTree;
using Twee2Z.Console;


namespace Test.ObjectTree
{
    [TestClass]
    public class TreeTest
    {
        const string untTestFolder = "../../../TestFiles/UnitTestFiles/";
        const string passageOnlyPath = untTestFolder + "passageOnly.tw";

        [TestMethod]
        public void TestTreePassageOnly()
        {
            Tree passageOnly = createTree(passageOnlyPath);
            Assert.AreEqual("start", passageOnly.StartPassage.Name);
            Assert.AreEqual(1, passageOnly.Passages.Count);
            Assert.AreEqual(1, passageOnly.StartPassage.PassageContentList.Count);
            Assert.AreEqual("Your story will display this passage first Edit it by double clicking it\r\n\r\n",
                passageOnly.StartPassage.PassageContentList[0].PassageText.Text);
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
