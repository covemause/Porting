using Microsoft.VisualStudio.TestTools.UnitTesting;
using Porting.Core.Decode;
using Porting.Core.Data;
using System.Collections.Generic;

namespace Porting.Core.Test.Decode
{
    [TestClass]
    public class UnitTest_CS_9
    {
        private DecodeCS_9 dec;

        public UnitTest_CS_9()
        {
            dec = new DecodeCS_9();
        }

        [TestMethod]
        public void TestParseArgs1()
        {
            var srcVal = new string[] { "" };
            var refVal = "";
            try
            {
                var dstVal = dec.ParseArgs(srcVal);
                Assert.IsTrue(false);  // 到達することはない。到達したらエラー
            }
            catch (DecodeException decEx)
            {
                Assert.AreEqual(decEx.DecodeDescription,
                    "CSDecode Error! Args Item Null");

            }


        }
        [TestMethod]
        public void TestParseArgs2()
        {
            string[]? srcVal = null;
            string refVal = string.Empty;

            var dstVal = dec.ParseArgs(srcVal);

            Assert.AreEqual(dstVal, refVal);

        }
        [TestMethod]
        public void TestParseArgs3()
        {
            var srcVal = new string[] { "Byval Arg As String" };
            string refVal = "string Arg";

            var dstVal = dec.ParseArgs(srcVal);

            Assert.AreEqual(dstVal, refVal);

        }
        public void TestParseArgs4()
        {
            var srcVal = new string[] { "Byval Arg As String, Beref Count As Long" };
            string refVal = "string Arg, ref long Count";

            var dstVal = dec.ParseArgs(srcVal);

            Assert.AreEqual(dstVal, refVal);

        }
        public void TestParseArgs5()
        {
            var srcVal = new string[] { "Arg As String, Count As Long" };
            string refVal = "string Arg, long Count";

            var dstVal = dec.ParseArgs(srcVal);

            Assert.AreEqual(dstVal, refVal);

        }
        public void TestParseArgs6()
        {
            var srcVal = new string[] { "Optional Arg As String = \"TEST\", Optional Byval Count As Long = 0, Optional Byref Data As String = \"\"" };
            string refVal = "string Arg = \"TEST\", ref long Count = 0, string Data = \"\"";

            var dstVal = dec.ParseArgs(srcVal);

            Assert.AreEqual(dstVal, refVal);

        }



    }
}
