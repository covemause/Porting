using Microsoft.VisualStudio.TestTools.UnitTesting;
using Porting.Core.Decode;
using Porting.Core.Data;
using System.Collections.Generic;

namespace Porting.Core.Test.Decode
{
    [TestClass]
    public class CtmToCS_9
    {
        private readonly DecodeCS_9 dec;

        public CtmToCS_9()
        {
            dec = new DecodeCS_9();
        }

        [TestMethod]
        public void TestDecode_Function_Start1()
        {
            var srcVal = "Public Function GetFileName(Byval strPath As String) As String";
            var args = new string[] { "Byval strPath As String" };
            var srcCtm = new CtmFunction(0, srcVal, "", srcVal, null, new List<CtmBase>(), CtmFunction.KindEnum.StartFunction,
                                     CtmFunction.AccessModifierEnum.Public, "GetFileName", args, "String");
            string dstVal = ""; 
            string refVal = "public string GetFileName(string strPath) {";

            dec.Execute(srcCtm);

            Assert.AreEqual(dstVal, refVal);

        }

    }
}