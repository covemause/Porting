using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Porting.Core.Data;

namespace Porting.Core.Decode
{
    /// <summary>
    /// C# 9（NET5）のデコード
    /// </summary>
    /// <remarks>
    /// VB構文からのデコード対応 備忘録
    /// ・On Error Goto ラベルは、gotoが使えるので対応可能。読みにくいけど。
    /// ・ParamArrayはparamsで対応
    /// ・Variantはobjectで代替
    /// 
    /// 
    /// VB構文でデーコード課題一覧
    /// ・関数引数の参照戻りに対する規定値  VB6: Optional Byref Args As String = "" => ref string Args = ""はできない
    /// ・On Error Resume Nextを本気で対応しようとすると大変になる
    /// ・On Error Goto 0もtry~catchの仕組みとは考えが違う
    /// ・UIのコントロール配列はどうするか。。
    /// 
    /// </remarks>
    public class DecodeCS_9 : IDecodeCS
    {
        

        public void Execute(CtmBase ctmBase, ref string result)
        {

            // デコード処理
            switch (ctmBase.GetType().ToString())
            {
                case "CtmFunction":
                    result = parseFunc((CtmFunction)ctmBase);
                    break;
                case "CtmIf":
                    result = parseIf((CtmIf)ctmBase);
                    break;
            }



            // 子要素もあればデコード
            if (ctmBase.InnerCtmList != null && ctmBase.InnerCtmList.Count > 0)
            {
                foreach(var ctm in ctmBase.InnerCtmList)
                {
                    Execute(ctm, ref result);
                }
            }
        }

        private string parseFunc(CtmFunction ctm)
        {
            string result = "";
            string indentSpace = new string(' ', ctm.Indent);
            string comment = " //" + ctm.Comment;


            // アクセス修飾子
            string accessModifierString = "";
            switch (ctm.AccessModifier)
            {
                case CtmFunction.AccessModifierEnum.Public:
                    accessModifierString = "public";
                    break;
                case CtmFunction.AccessModifierEnum.Private:
                    accessModifierString = "private";
                    break;
                case CtmFunction.AccessModifierEnum.None:   // 省略時
                    accessModifierString = "private";
                    break;
                default:
                    throw new DecodeException("AccessModifier Not Case :" + ctm.AccessModifier.ToString());
            }

            // 引数
            var argsValue = ParseArgs(ctm.Args);

            // 戻り値
            var resultValue = ParseTypeValue(ctm.ResultTypeName);

            switch (ctm.Kind)
            {
                case CtmFunction.KindEnum.EndSub:
                    return indentSpace + "}" + comment;
                case CtmFunction.KindEnum.EndFunction:
                    result = indentSpace + "return result" + ctm.Name + ";" + comment + Environment.NewLine;
                    result += indentSpace + "}";
                    return result;
                case CtmFunction.KindEnum.ExitSub:
                    return indentSpace + "return;" + comment;
                case CtmFunction.KindEnum.ExitFunction:
                    return indentSpace + "return result" + ctm.Name + ";" + comment;

                case CtmFunction.KindEnum.StartSub:
                    result = indentSpace + accessModifierString + ' ' +
                             ctm.Name + '(' + argsValue + ") {";

                    return result;
            }

            return result;
        }

        private string parseIf(CtmIf ctm)
        {
            string result = "";
            string indentSpace = new string(' ', ctm.Indent);
            string comment = " //" + ctm.Comment;

            var condition = PrarseCondition(ctm.ConditionsValue);


            return indentSpace + result + comment;
        }

        public string ParseArgs(string[]? values)
        {

            if (values == null) return String.Empty;

            var resultArgs = new List<List<string>>();

            foreach (var item in values)
            {
                if (item == null || item.Length == 0) throw new DecodeException("CSDecode Error! Args Item Null");

                var resultArgItem = new List<string>();

                // Byref Args As String => {"Byref", "Args", "As", "String"}
                var argItem = item.Split(' ');

                if (argItem[0].ToLower() == "byref")
                {
                    resultArgItem.Add("ref");
                    resultArgItem.Add(ParseTypeValue(argItem[3]));
                    resultArgItem.Add(argItem[1]);
                    resultArgs.Add(resultArgItem);
                    continue;
                }

                if (argItem[0].ToLower() == "byval")
                {
                    resultArgItem.Add(ParseTypeValue(argItem[3]));
                    resultArgItem.Add(argItem[1]);
                    resultArgs.Add(resultArgItem);
                    continue;
                }

                // 省略時はByval同様
                if (argItem.Length == 3)
                {
                    resultArgItem.Add(ParseTypeValue(argItem[2]));
                    resultArgItem.Add(argItem[0]);
                    resultArgs.Add(resultArgItem);
                    continue;
                }
            }

            var result = "";
            foreach(var itemList in resultArgs)
            {
                foreach(var item in itemList )
                {
                    result += item + ' ';
                }
                result = result.Trim() + ",";
            }

            return result.Substring(0, result.Length - 1); // 最後のカンマを削除             
        }

        public string ParseTypeValue(string typeValue)
        {
            switch (typeValue.ToLower())
            {
                case "string":
                    return "string";
                case "long":
                    return "long";
            }

            throw new DecodeException("ResultType Not Case :" + typeValue);
        }

        public List<CtmCondition> PrarseCondition(string value)
        {
            return new List<CtmCondition>();
        }
        public string PrarseCondition(List<CtmCondition> values)
        {
            var result = "";
            foreach(var item in values)
            {
                if (item.JoinValue.ToLower() == "and")
                {
                    result += " && ";
                }
                if (item.JoinValue.ToLower() == "or")
                {
                    result += " || ";
                }

                // 条件式でなければそのまま
                if (item.OperatorValue == "")
                {
                    result += item.LeftSide;
                }
                else
                {
                    result += item.LeftSide + " " + item.OperatorValue + " " + item.RightSide;
                }
                
            }

            return result;
        }
    }
}
