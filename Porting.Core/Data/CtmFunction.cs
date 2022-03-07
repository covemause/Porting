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
        }
        public KindEnum Kind;

        /// <summary>
        /// アクセス修飾子
        /// </summary>
        public AccessModifierEnum AccessModifier { get; set; }

        public string Name { get; set; } = string.Empty;

        public string[]? Args { get; set; } = null;
        

        public string ResultTypeName { get; set; } = string.Empty;

        public CtmBase[] CodeLines = new CtmBase[0];

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

            this.AccessModifier = ctmFunctionConText.FuncGetAccessModifier(this.Value);

            this.Kind = ctmFunctionConText.FuncGetKind(this.Value);

            if (this.Kind == KindEnum.StartSub || this.Kind == KindEnum.StartFunction)
            {
                this.Args = ctmFunctionConText.FuncGetArgs(this.Value);
            }

            if (this.Kind != KindEnum.StartFunction)
            {
                this.ResultTypeName = ctmFunctionConText.FunGetResultTypeName(this.Value);
            }           
        }
    }
}
