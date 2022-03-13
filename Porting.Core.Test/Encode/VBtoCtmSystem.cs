using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Porting.Core.Encode;
using Porting.Core.Data;

namespace Porting.Core.Test.Encode
{
    [TestClass]
    public class VBtoCtmSystem
    {
        private EncodeVB6 _enc;

        public VBtoCtmSystem()
        {
            _enc = new EncodeVB6();
        }


        [TestMethod]
        public void TestSystem1()
        {
            var srcVal = "End    '終了";

            var ctm = new CtmSystem(new CtmBaseContext(null, srcVal, _enc.GetIndent, _enc.GetComment),
                                      new CtmSystemContext(_enc.GetSystemKind));

            // CtmBaseContext
            Assert.AreEqual(ctm.OriginalCode, srcVal);
            Assert.AreEqual(ctm.Indent, 0);
            Assert.AreEqual(ctm.Comment, "'終了");
            Assert.IsNull(ctm.Parent);
            Assert.AreEqual(ctm.Value, "End");
            Assert.IsTrue(ctm.InnerCtmList.Count == 0);

            // CtmSystemContext
            Assert.AreEqual(ctm.Kind, CtmSystem.KindEnum.SysEnd);
        }


        [TestMethod]
        public void TestSystem2()
        {
            var srcVal = "    DoEvents    '制御を移譲";

            var ctm = new CtmSystem(new CtmBaseContext(null, srcVal, _enc.GetIndent, _enc.GetComment),
                                      new CtmSystemContext(_enc.GetSystemKind));

            // CtmBaseContext
            Assert.AreEqual(ctm.OriginalCode, srcVal);
            Assert.AreEqual(ctm.Indent, 4);
            Assert.AreEqual(ctm.Comment, "'制御を移譲");
            Assert.IsNull(ctm.Parent);
            Assert.AreEqual(ctm.Value, "DoEvents");
            Assert.IsTrue(ctm.InnerCtmList.Count == 0);

            // CtmSystemContext
            Assert.AreEqual(ctm.Kind, CtmSystem.KindEnum.SysDoEvent);
        }

    }
}
