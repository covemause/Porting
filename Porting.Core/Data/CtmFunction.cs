using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Data
{
    public class CtmFunction : CtmBase
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
            StartSub,
            EndSub,
            ExitSub,
            StartFunction,
            EndFunction,
            ExitFunction,
            ResultCode, // 戻り値のコード
        }
        public KindEnum Kind;

        /// <summary>
        /// アクセス修飾子
        /// </summary>
        public AccessModifierEnum AccessModifier { get; set; } = AccessModifierEnum.None;

        public string Name { get; set; } = string.Empty;

        public string[]? Args { get; set; } = null;
        

        public string ResultTypeName { get; set; } = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="codeLine"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <remarks>
        /// 生成時に内部メンバーに格納する
        /// </remarks>
        public CtmFunction(CtmBaseContext ctmBaseFunc,
                           CtmFunctionContext ctmFunctionConText) : 
                           base(ctmBaseFunc)
        {


            this.Kind = ctmFunctionConText.FuncGetKind(this.Value);


            if (this.Kind == KindEnum.StartSub || this.Kind == KindEnum.StartFunction)
            {
                this.AccessModifier = ctmFunctionConText.FuncGetAccessModifier(this.Value);

                this.Name = ctmFunctionConText.FunGetName(this.Value);

                this.Args = ctmFunctionConText.FuncGetArgs(this.Value);
            }

            if (this.Kind == KindEnum.StartFunction)
            {
                this.ResultTypeName = ctmFunctionConText.FunGetResultTypeName(this.Value);
            }           
        }

        public CtmFunction(int indent, string originalCode, string comment, string value, CtmBase? parent, List<CtmBase> innerCtmList,
                           CtmFunction.KindEnum kind, CtmFunction.AccessModifierEnum accessModifier, string name, string[]? args, string returnTypeName ) 
                            : base(indent, originalCode, comment, value, parent, innerCtmList)
        {
            this.Kind = kind;
            this.AccessModifier = accessModifier;
            this.Name = name;
            this.Args = args;
            this.ResultTypeName = returnTypeName;
        }
    }
}
