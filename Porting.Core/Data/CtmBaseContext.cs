using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Data
{
    /// <summary>
    /// 共通変換モデルのベースの生成に必要な要素
    /// </summary>
    public class CtmBaseContext
    {
        // 親
        public CtmBase? Parent { get; }

        // 元コード
        public string CodeLine { get; }

        // インデント取得関数
        public Func<string, string, int> FuncGetIndent { get; }

        // コメント取得関数
        public Func<string, string> FuncGetComment { get; }

        public CtmBaseContext(CtmBase? parent, string codeLine, Func<string, string, int> funcGetIndent, Func<string, string> funcGetComment)
        {
            Parent = parent;
            CodeLine = codeLine;
            FuncGetIndent = funcGetIndent;
            FuncGetComment = funcGetComment;
        }
    }
}
