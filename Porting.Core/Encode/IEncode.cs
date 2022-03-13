using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Porting.Core.Data;

namespace Porting.Core.Encode
{
    public interface IEncode
    {
        CtmBase Execute(string[] lines);

        CtmBase? Execute(CtmBase? current, string line);
    }
}
