using System;
using NUnit.Framework;
using Twee2Z.CodeGen.Text;

namespace Twee2Z.UnitTests.TestCodeGen
{
    [TestFixture]
    public class ZTextTest
    {
        [Test]
        public void TestHelloWorldZText()
        {
            ushort[] referenceArray = { 4522, 17972, 916, 56873 };
            ushort[] helloWorld = TextHelper.Convert("Hello world");

            Assert.AreEqual(referenceArray.Length, helloWorld.Length);
        }
    }
}
