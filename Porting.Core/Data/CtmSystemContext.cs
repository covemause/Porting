using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Data
{
    public class CtmSystemContext
    {
        public Func<string, CtmSystem.KindEnum> FuncGetKind { get; }

        public CtmSystemContext(Func<string, CtmSystem.KindEnum> funcGetKind)
        {
            FuncGetKind = funcGetKind;
        }
    }
}
