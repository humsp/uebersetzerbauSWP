﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Twee2Z.CodeGen.Text;

namespace Test
{
    [TestClass]
    public class TextTest
    {
        [TestMethod]
        public void Test()
        {
            ushort[] referenceArray = { 4522, 17972, 916, 56873 };
            ushort[] helloWolrd = ZText.Convert("Hello world");
            Assert.AreEqual(referenceArray.Length, helloWolrd.Length);
            for (int i = 0; i < referenceArray.Length; i++)
            {
                Assert.AreEqual(referenceArray[i], helloWolrd[i]);
            }
        }
    }
}
