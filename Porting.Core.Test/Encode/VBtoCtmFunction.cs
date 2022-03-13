using Microsoft.VisualStudio.TestTools.UnitTesting;
using Porting.Core.Encode;
using Porting.Core.Data;

namespace Porting.Core.Test.Encode
{
    [TestClass]
    public class VBtoCtmFunction
    {
        private readonly EncodeVB6 _enc;

        public VBtoCtmFunction()
        {
            _enc = new EncodeVB6();
        }

        [TestMethod]
        public void TestFunction_Normal_Start1()
        {
            var srcVal = "Public Function GetFileName(Byval strPath As String) As String";

            var ctm = new CtmFunction(new CtmBaseContext(null, srcVal, _enc.GetIndent, _enc.GetComment),
                                      new CtmFunctionContext(_enc.GetAccessModifier, _enc.GetFunctionKind, _enc.GetMethodName, _enc.GetFunctionArgs, _enc.GetFunctionResultValue));

            // CtmBaseContext
            Assert.AreEqual(ctm.OriginalCode, srcVal);
            Assert.AreEqual(ctm.Indent, 0);
            Assert.AreEqual(ctm.Comment, "");
            Assert.IsNull(ctm.Parent);
            Assert.AreEqual(ctm.Value, srcVal);
            Assert.IsTrue(ctm.InnerCtmList.Count == 0);

            // CtmFunctionContext
            Assert.AreEqual(ctm.AccessModifier, CtmFunction.AccessModifierEnum.Public);
            Assert.AreEqual(ctm.Kind, CtmFunction.KindEnum.StartFunction);
            Assert.IsNotNull(ctm.Args);
            if (ctm.Args != null) Assert.AreEqual(ctm.Args[0], "Byval strPath As String");
            Assert.AreEqual(ctm.Name, "GetFileName");
            Assert.AreEqual(ctm.ResultTypeName, "String");

        }

        [TestMethod]
        public void TestFunction_Normal_Start2()
        {
            var srcVal = "    Private Sub GetFileNameA(Byval strPath As String, Count As Long)  'ファイル取得";

            var ctm = new CtmFunction(new CtmBaseContext(null, srcVal, _enc.GetIndent, _enc.GetComment),
                                      new CtmFunctionContext(_enc.GetAccessModifier, _enc.GetFunctionKind, _enc.GetMethodName, _enc.GetFunctionArgs, _enc.GetFunctionResultValue));

            // CtmBaseContext
            Assert.AreEqual(ctm.OriginalCode, srcVal);
            Assert.AreEqual(ctm.Indent, 4);
            Assert.AreEqual(ctm.Comment, "'ファイル取得");
            Assert.IsNull(ctm.Parent);
            Assert.AreEqual(ctm.Value, "Private Sub GetFileNameA(Byval strPath As String, Count As Long)");
            Assert.IsTrue(ctm.InnerCtmList.Count == 0);

            // CtmFunctionContext
            Assert.AreEqual(ctm.AccessModifier, CtmFunction.AccessModifierEnum.Private);
            Assert.AreEqual(ctm.Kind, CtmFunction.KindEnum.StartSub);
            Assert.IsNotNull(ctm.Args);
            if (ctm.Args != null) Assert.AreEqual(ctm.Args[0], "Byval strPath As String");
            if (ctm.Args != null) Assert.AreEqual(ctm.Args[1], "Count As Long");
            Assert.AreEqual(ctm.Name, "GetFileNameA");
            Assert.AreEqual(ctm.ResultTypeName, "");

        }

        [TestMethod]
        public void TestFunction_Normal_Start3()
        {
            var srcVal = "   Function GetFileNameA(Byval strPath As String, Count As Long)  'ファイル取得";

            var ctm = new CtmFunction(new CtmBaseContext(null, srcVal, _enc.GetIndent, _enc.GetComment),
                                      new CtmFunctionContext(_enc.GetAccessModifier, _enc.GetFunctionKind, _enc.GetMethodName, _enc.GetFunctionArgs, _enc.GetFunctionResultValue));

            // CtmBaseContext
            Assert.AreEqual(ctm.OriginalCode, srcVal);
            Assert.AreEqual(ctm.Indent, 3);
            Assert.AreEqual(ctm.Comment, "'ファイル取得");
            Assert.IsNull(ctm.Parent);
            Assert.AreEqual(ctm.Value, "Function GetFileNameA(Byval strPath As String, Count As Long)");
            Assert.IsTrue(ctm.InnerCtmList.Count == 0);

            // CtmFunctionContext
            Assert.AreEqual(ctm.AccessModifier, CtmFunction.AccessModifierEnum.Private);
            Assert.AreEqual(ctm.Kind, CtmFunction.KindEnum.StartFunction);
            Assert.IsNotNull(ctm.Args);
            if (ctm.Args != null) Assert.AreEqual(ctm.Args[0], "Byval strPath As String");
            if (ctm.Args != null) Assert.AreEqual(ctm.Args[1], "Count As Long");
            Assert.AreEqual(ctm.Name, "GetFileNameA");
            Assert.AreEqual(ctm.ResultTypeName, "Varient");

        }


        [TestMethod]
        public void TestFunction_Normal_End1()
        {
            var srcVal = "       End Function   'ファイル取得";

            var ctm = new CtmFunction(new CtmBaseContext(null, srcVal, _enc.GetIndent, _enc.GetComment),
                                      new CtmFunctionContext(_enc.GetAccessModifier, _enc.GetFunctionKind, _enc.GetMethodName, _enc.GetFunctionArgs, _enc.GetFunctionResultValue));

            // CtmBaseContext
            Assert.AreEqual(ctm.OriginalCode, srcVal);
            Assert.AreEqual(ctm.Indent, "       ".Length);
            Assert.AreEqual(ctm.Comment, "'ファイル取得");
            Assert.IsNull(ctm.Parent);
            Assert.AreEqual(ctm.Value, "End Function");
            Assert.IsTrue(ctm.InnerCtmList.Count == 0);

            // CtmFunctionContext
            Assert.AreEqual(ctm.AccessModifier, CtmFunction.AccessModifierEnum.None);
            Assert.AreEqual(ctm.Kind, CtmFunction.KindEnum.EndFunction);
            Assert.IsNull(ctm.Args);
            Assert.AreEqual(ctm.Name, string.Empty);
            Assert.AreEqual(ctm.ResultTypeName, string.Empty);

        }
        [TestMethod]
        public void TestFunction_Normal_End2()
        {
            var srcVal = "End Sub   'ファイル取得";

            var ctm = new CtmFunction(new CtmBaseContext(null, srcVal, _enc.GetIndent, _enc.GetComment),
                                      new CtmFunctionContext(_enc.GetAccessModifier, _enc.GetFunctionKind, _enc.GetMethodName, _enc.GetFunctionArgs, _enc.GetFunctionResultValue));

            // CtmBaseContext
            Assert.AreEqual(ctm.OriginalCode, srcVal);
            Assert.AreEqual(ctm.Indent, 0);
            Assert.AreEqual(ctm.Comment, "'ファイル取得");
            Assert.IsNull(ctm.Parent);
            Assert.AreEqual(ctm.Value, "End Sub");
            Assert.IsTrue(ctm.InnerCtmList.Count == 0);

            // CtmFunctionContext
            Assert.AreEqual(ctm.AccessModifier, CtmFunction.AccessModifierEnum.None);
            Assert.AreEqual(ctm.Kind, CtmFunction.KindEnum.EndSub);
            Assert.IsNull(ctm.Args);
            Assert.AreEqual(ctm.Name, string.Empty);
            Assert.AreEqual(ctm.ResultTypeName, string.Empty);

        }

        [TestMethod]
        public void TestFunction_Normal_Exit1()
        {
            var srcVal = "Exit Sub   'aaファイル取得";

            var ctm = new CtmFunction(new CtmBaseContext(null, srcVal, _enc.GetIndent, _enc.GetComment),
                                      new CtmFunctionContext(_enc.GetAccessModifier, _enc.GetFunctionKind, _enc.GetMethodName, _enc.GetFunctionArgs, _enc.GetFunctionResultValue));

            // CtmBaseContext
            Assert.AreEqual(ctm.OriginalCode, srcVal);
            Assert.AreEqual(ctm.Indent, 0);
            Assert.AreEqual(ctm.Comment, "'aaファイル取得");
            Assert.IsNull(ctm.Parent);
            Assert.AreEqual(ctm.Value, "Exit Sub");
            Assert.IsTrue(ctm.InnerCtmList.Count == 0);

            // CtmFunctionContext
            Assert.AreEqual(ctm.AccessModifier, CtmFunction.AccessModifierEnum.None);
            Assert.AreEqual(ctm.Kind, CtmFunction.KindEnum.ExitSub);
            Assert.IsNull(ctm.Args);
            Assert.AreEqual(ctm.Name, string.Empty);
            Assert.AreEqual(ctm.ResultTypeName, string.Empty);

        }

        [TestMethod]
        public void TestFunction_Normal_Exit2()
        {
            var srcVal = " Exit Function   'aaファイル取得";

            var ctm = new CtmFunction(new CtmBaseContext(null, srcVal, _enc.GetIndent, _enc.GetComment),
                                      new CtmFunctionContext(_enc.GetAccessModifier, _enc.GetFunctionKind, _enc.GetMethodName, _enc.GetFunctionArgs, _enc.GetFunctionResultValue));

            // CtmBaseContext
            Assert.AreEqual(ctm.OriginalCode, srcVal);
            Assert.AreEqual(ctm.Indent, 1);
            Assert.AreEqual(ctm.Comment, "'aaファイル取得");
            Assert.IsNull(ctm.Parent);
            Assert.AreEqual(ctm.Value, "Exit Function");
            Assert.IsTrue(ctm.InnerCtmList.Count == 0);

            // CtmFunctionContext
            Assert.AreEqual(ctm.AccessModifier, CtmFunction.AccessModifierEnum.None);
            Assert.AreEqual(ctm.Kind, CtmFunction.KindEnum.ExitFunction);
            Assert.IsNull(ctm.Args);
            Assert.AreEqual(ctm.Name, string.Empty);
            Assert.AreEqual(ctm.ResultTypeName, string.Empty);

        }
    }
}