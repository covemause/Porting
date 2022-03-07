using Microsoft.VisualStudio.TestTools.UnitTesting;
using Porting.Core.Encode;
using Porting.Core.Data;

namespace Porting.Core.Test.Data
{
    [TestClass]
    public class VBtoCtmBase
    {
        private EncodeVB _enc;

        public VBtoCtmBase()
        {
            _enc = new EncodeVB();
        }

        [TestMethod]
        public void TestBase_Normal1()
        {
            var srcVal = "If ApState <> ApStateInsert Then boxVenderCode(Index) = Format(boxVenderCode(Index), \"000\")";

            var ctm = new CtmBase(new CtmBaseContext(null, srcVal, _enc.GetIndent, _enc.GetComment));

            Assert.AreEqual(ctm.OriginalCode, srcVal);
            Assert.AreEqual(ctm.Indent, 0);
            Assert.AreEqual(ctm.Comment, "");
            Assert.IsNull(ctm.Parent);
            Assert.AreEqual(ctm.Value, srcVal);
            Assert.IsTrue(ctm.InnerCtmList.Count == 0);
        }


        [TestMethod]
        public void TestBase_Normal2()
        {
            var srcVal = "    If ApState <> ApStateInsert Then Name = \"Where KANA Like 'テスト%'\"   'テストの前方一致";

            var ctm = new CtmBase(new CtmBaseContext(null, srcVal, _enc.GetIndent, _enc.GetComment));

            Assert.AreEqual(ctm.OriginalCode, srcVal);
            Assert.AreEqual(ctm.Indent, 4);
            Assert.AreEqual(ctm.Comment, "'テストの前方一致");
            Assert.IsNull(ctm.Parent);
            Assert.AreEqual(ctm.Value, "If ApState <> ApStateInsert Then Name = \"Where KANA Like 'テスト%'\"");
            Assert.IsTrue(ctm.InnerCtmList.Count == 0);


        }
    }
}