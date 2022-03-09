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
    public class CtmFunctionContext
    {

        public Func<string, CtmFunction.AccessModifierEnum> FuncGetAccessModifier { get; }
        public Func<string, CtmFunction.KindEnum> FuncGetKind { get; }
        public Func<string, string> FunGetName { get; }
        public Func<string, string[]?> FuncGetArgs { get; }
        public Func<string, string> FunGetResultTypeName { get; }

        public CtmFunctionContext(Func<string, CtmFunction.AccessModifierEnum> funcGetAccessModifier,
                                  Func<string, CtmFunction.KindEnum> funcGetKind,
                                  Func<string, string> funcGetName,
                                  Func<string, string[]?> funcGetArgs,
                                  Func<string, string> funGetResultTypeName)
        {
            FuncGetAccessModifier = funcGetAccessModifier;
            FuncGetKind = funcGetKind;
            FunGetName = funcGetName;
            FuncGetArgs = funcGetArgs;
            FunGetResultTypeName = funGetResultTypeName;

        }
    }
}
