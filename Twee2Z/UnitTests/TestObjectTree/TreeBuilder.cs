using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Twee2Z.Analyzer;
using Twee2Z.ObjectTree;
using Twee2Z.Console;
using NUnit.Framework;

namespace Twee2Z.UnitTests.TestObjectTree
{
    class TreeBuilder
    {
        public static Tree createTree(string tweeFile)
        {
            FileStream tweeFileStream = new FileStream(tweeFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            Tree tree = Program.AnalyseFile(tweeFileStream);
            Assert.IsTrue(new TreeValidator(tree).ValidateTree());
            return tree;
        }
    }
}
