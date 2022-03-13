using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Porting.Core.Data;

namespace Porting.Core.Decode
{
    /// <summary>
    /// C#デコードのインターフェース
    /// </summary>
    /// <remarks>
    /// バージョンにより記述が変わるため
    /// https://docs.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-version-history
    /// </remarks>
    public interface IDecode
    {
        string Value { get; set; }

        void Execute(CtmBase main);
    }
}
