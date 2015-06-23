using System;
using NUnit.Framework;
using Twee2Z.CodeGen.Text;

namespace UnitTests.TestCodeGen
{
    [TestFixture]
    public class ZTextTest
    {
        [Test]
        public void TestHelloWorldZText()
        {
            ushort[] referenceArray = { 4522, 17972, 916, 56873 };
            ushort[] helloWolrd = TextHelper.Convert("Hello world");
            Assert.AreEqual(referenceArray.Length, helloWolrd.Length);
            for (int i = 0; i < referenceArray.Length; i++)
            {
                Assert.AreEqual(referenceArray[i], helloWolrd[i]);
            }
        }
    }
}
