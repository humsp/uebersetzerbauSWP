using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twee2Z.CodeGen;
using Twee2Z.CodeGen.Instructions.Templates;
using Twee2Z.CodeGen.Memory;

namespace UnitTests.TestCodeGen
{
    [TestClass]
    public class IZComponentTest
    {
        [TestMethod]
        public void TestPrintSize()
        {
            innerTest(new Print("Hello Wolrd"));
        }

        [TestMethod]
        public void TestQuitSize()
        {
            innerTest(new Quit());
        }

        [TestMethod]
        public void TestZDynamicMemoryize()
        {
            innerTest(new ZDynamicMemory());
        }

        [TestMethod]
        public void TestZHeaderSize()
        {
            innerTest(new ZHeader(0, 0, 0, 0, 0, 0, 0));
        }

        [TestMethod]
        public void TestZHeaderExtensionSize()
        {
            innerTest(new ZHeaderExtension());
        }

        [TestMethod]
        public void TestZHighMemorySize()
        {
            innerTest(new ZHighMemory());
        }

        [TestMethod]
        public void TestZMemorySize()
        {
            innerTest(new ZMemory());
        }

        [TestMethod]
        public void TestZStaticMemorySize()
        {
            innerTest(new ZStaticMemory());
        }

        private void innerTest(IZComponent component)
        {
            Assert.AreEqual(component.ToBytes().Length, component.Size);
        }
    }
}
