using System;
using NUnit.Framework;
using Twee2Z.ObjectTree;


namespace UnitTests.TestObjectTree
{
    [TestFixture]
    public class TreeTextTest
    {
        private const string _testUmlaut = Const.untTestFolder + "textUmlaut.tw";

        [Test]
        public void TestUmlaut()
        {
            Tree tree = TreeBuilder.createTree(_testUmlaut);
            Assert.AreEqual("Start", tree.StartPassage.Name);
            Assert.AreEqual(2, tree.Passages.Count);

            Passage startPassage = tree.StartPassage;
            Assert.AreEqual(1, startPassage.PassageContentList.Count);
            Assert.AreEqual("test ÜüÄäÖöèéêÉÈÊ\r\n", startPassage.PassageContentList[0].PassageText.Text);

            Assert.IsTrue(tree.Passages.ContainsKey("umlautÜüÄäÖöèéêÉÈÊ"));
            Assert.AreEqual("ende", tree.Passages["umlautÜüÄäÖöèéêÉÈÊ"].PassageContentList[0].PassageText.Text);
        }

        // TODO kommentare

        // invaliden passageninhalt prüfen
    }
}
