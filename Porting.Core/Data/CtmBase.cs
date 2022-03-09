using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Data
{
    /// <summary>
    /// コード１行分のベースクラス
    /// </summary>
    public class CtmBase
    {

        #region プロパティ

        /// <summary>
        /// インデント
        /// </summary>
        public int Indent { get; set; } = 0;

        /// <summary>
        /// 元コード
        /// </summary>
        public string OriginalCode { get; set; } = string.Empty;

        /// <summary>
        /// コメント
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// インデントとコメントを除いた対象値
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 自クラスの親
        /// 例えば、Function内のCtmCodeが自クラスであれば、CtmFunctionクラスが親になる
        /// </summary>
        public CtmBase? Parent { get; set; } = null;

        /// <summary>
        /// 自クラスの子
        /// 例えば、CtmFunctionクラスが自クラスであれば、Function内の各処理クラスが子になる
        /// </summary>
        public List<CtmBase> InnerCtmList { get; set; } = new List<CtmBase>();

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public CtmBase(int indent, string originalCode, string comment, string value, CtmBase? parent, List<CtmBase> innerCtmList)
        {
            Indent = indent;
            OriginalCode = originalCode;
            Comment = comment;
            Value = value;
            Parent = parent;
            InnerCtmList = innerCtmList;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ctmBaseFunc">生成に必要な要素群</param>
        /// <remarks>
        /// 生成時にインデントと本文コードとコメントを分離する
        /// </remarks>
        public CtmBase(CtmBaseContext ctmBaseFunc)
        {

            this.Parent = ctmBaseFunc.Parent;

            this.OriginalCode = ctmBaseFunc.CodeLine;

            // インデントとコメントの分離
            this.Indent = ctmBaseFunc.FuncGetIndent(this.OriginalCode, this.OriginalCode.Trim());

            this.Comment = ctmBaseFunc.FuncGetComment(this.OriginalCode);

            if (this.Comment.Length > 0)
            {
                this.Value = this.OriginalCode.Substring(0, this.OriginalCode.IndexOf(this.Comment));
            }
            else
            {
                this.Value = this.OriginalCode;
            }

            // インデントとコメントを除外した値
            this.Value = this.Value.Trim();
        }

        /// <summary>
        /// 子要素に追加
        /// </summary>
        /// <param name="ctmBase">追加要素</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(CtmBase ctmBase)
        {
            if (ctmBase == null) throw new ArgumentNullException();
        
            this.InnerCtmList.Add(ctmBase);
        }

    }
}
