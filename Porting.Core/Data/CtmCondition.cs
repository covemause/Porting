using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porting.Core.Data
{
    /// <summary>
    /// 条件
    /// </summary>
    public class CtmCondition
    {

        /// <summary>
        /// 左辺
        /// </summary>
        public string LeftSide { get; set; } = string.Empty;

        // 比較値
        public string OperatorValue { get; set; } = string.Empty;

        /// <summary>
        /// 右辺
        /// </summary>
        public string RightSide { get; set; } = string.Empty;

        /// <summary>
        /// 連結値
        /// </summary>
        /// <remarks>
        /// args1 = 1 And args2 = 2の場合、1つ目のJoinValue="", 2つ目のJoinValue="And"
        /// </remarks>
        public string JoinValue { get; set; } = string.Empty;

    }
}
