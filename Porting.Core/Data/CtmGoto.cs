using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Data
{
    /// <summary>
    /// Goto文
    /// </summary>
    public class CtmGoto : CtmBase
    {
        public enum AccessModifierEnum
        {
            None,  // 省略
            Public,
            Private,
            Protected,
        }

        public enum KindEnum
        {
            Goto,    // ラベル呼び出し
            Label,   // ラベル自体
        }
        public AccessModifierEnum AccessModifier { get; set; }

        public KindEnum Kind;

        public string GotoLabel { get; set; } = string.Empty;

        public CtmGoto(CtmBaseContext ctmBaseContext, CtmGotoContext ctmGotoContext) : 
            base(ctmBaseContext)
        {
            this.AccessModifier = ctmGotoContext.FuncGetAccessModifier(this.Value);

            this.Kind = ctmGotoContext.FuncGetKind(this.Value);
        }
    }
}
