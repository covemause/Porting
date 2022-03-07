using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Data
{
    /// <summary>
    /// 関数変換モデルの生成に必要な追加要素
    /// </summary>
    public class CtmGotoContext
    {
        public Func<string, CtmGoto.AccessModifierEnum> FuncGetAccessModifier { get; }

        public Func<string, CtmGoto.KindEnum> FuncGetKind { get; }

        public CtmGotoContext(Func<string, CtmGoto.KindEnum> funcGetKind, Func<string, CtmGoto.AccessModifierEnum> funcGetAccessModifier)
        {
            FuncGetKind = funcGetKind;
            FuncGetAccessModifier = funcGetAccessModifier;
        }
    }
}
