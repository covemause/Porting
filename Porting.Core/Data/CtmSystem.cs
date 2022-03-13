using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Data
{
    public sealed class CtmSystem : CtmBase
    {
        public enum KindEnum
        {
            SysEnd,
            SysDoEvent,
        }

        public KindEnum Kind;

        public CtmSystem(CtmBaseContext ctmBaseFunc, CtmSystemContext ctmSystem) : base(ctmBaseFunc)
        {
            this.Kind = ctmSystem.FuncGetKind(base.Value);
        }
    }
}
