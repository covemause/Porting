using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Data
{
    public class CtmIfContext
    {
        public Func<string, CtmIf.KindEnum> FuncGetKind { get; }
        public Func<string, string> FuncGetConditionsValue { get; }
        public Func<string, string> FuncGetInnerValue { get; }

        public CtmIfContext(Func<string, CtmIf.KindEnum> funcGetKind,
                            Func<string, string> funcGetConditionsValue,
                            Func<string, string> funcGetInnerValue)
        {
            FuncGetKind = funcGetKind;
            FuncGetConditionsValue = funcGetConditionsValue;
            FuncGetInnerValue = funcGetInnerValue;
        }

    }
}
