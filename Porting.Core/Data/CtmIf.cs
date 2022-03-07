using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Data
{
    public sealed class CtmIf : CtmBase
    {
        public enum KindEnum
        {
            IfOneLiner,
            IfThen,
            Else,
            ElseIfTOnLiner,
            ElseIfThen,
            EndIf,
        }

        public KindEnum Kind;

        /// <summary>
        /// IF条件値
        /// </summary>
        public string ConditionsValue = string.Empty;

        public string InnerValue = string.Empty;


        public CtmIf(CtmBaseContext ctmBaseContext, CtmIfContext ctmIfContext) :
            base(ctmBaseContext)
        {
            this.Kind = ctmIfContext.FuncGetKind(this.Value);


            if (this.Kind == KindEnum.IfOneLiner || this.Kind == KindEnum.ElseIfTOnLiner ||
                this.Kind == KindEnum.IfThen || this.Kind == KindEnum.ElseIfThen)
            {
                this.ConditionsValue = ctmIfContext.FuncGetConditionsValue(this.Value);
            }

            if (this.Kind == KindEnum.IfOneLiner || this.Kind == KindEnum.ElseIfTOnLiner)
            {
                this.InnerValue = ctmIfContext.FuncGetInnerValue(this.Value);
            }
        }


    }
}
