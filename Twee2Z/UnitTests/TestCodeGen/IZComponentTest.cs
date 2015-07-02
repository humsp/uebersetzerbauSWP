using System;
using Twee2Z.CodeGen;
using Twee2Z.CodeGen.Instruction.Template;
using Twee2Z.CodeGen.Memory;
using NUnit.Framework;

namespace Twee2Z.UnitTests.TestCodeGen
{
    [TestFixture]
    public class IZComponentTest
    {
        [Test]
        public void TestPrintSize()
        {
            innerTest(new Print("Hello Wolrd"));
        }

        [Test]
        public void TestQuitSize()
        {
            innerTest(new Quit());
        }

        [Test]
        public void TestZDynamicMemoryize()
        {
            innerTest(new ZDynamicMemory());
        }

        [Test]
        public void TestZHeaderSize()
        {
            innerTest(new ZHeader(0, 0, 0, 0, 0, 0, 0));
        }

        [Test]
        public void TestZHeaderExtensionSize()
        {
            innerTest(new ZHeaderExtension());
        }

        [Test]
        public void TestZHighMemorySize()
        {
            innerTest(new ZHighMemory());
        }

        [Test]
        public void TestZMemorySize()
        {
            innerTest(new ZMemory());
        }

        [Test]
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
