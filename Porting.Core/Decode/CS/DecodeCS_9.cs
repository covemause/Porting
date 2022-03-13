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
    public class DecodeCS_9 : IDecode
    {
        public string Value { get; set; } = string.Empty;

        public void Execute(CtmBase ctmBase)
        {
            ctmBase.Indent = getCtmIndent(ctmBase);


            // デコード処理
            switch (ctmBase.GetType().Name)
            {
                case "CtmFunction":
                    Value += parseFunc((CtmFunction)ctmBase);
                    break;
                case "CtmIf":
                    Value += parseIf((CtmIf)ctmBase);
                    break;
                case "CtmBase":
                    Value += parseBase(ctmBase);
                    break;
            }



            // 子要素もあればデコード
            if (ctmBase.InnerCtmList != null && ctmBase.InnerCtmList.Count > 0)
            {
                foreach(var ctm in ctmBase.InnerCtmList)
                {
                    Value += Environment.NewLine;
                    Execute(ctm);
                }
            }
        }

        private string parseFunc(CtmFunction ctm)
        {
            string result = "";
            string indentSpace = new string(' ', ctm.Indent);
            string indentSpaceUp = new string(' ', ctm.Indent + 4);
            string indentSpaceDown = ctm.Indent >= 4 ? new string(' ', ctm.Indent - 4) :  "";
            string comment = ctm.Comment != "" ? " //" + ctm.Comment : "";

            if (ctm.Kind == CtmFunction.KindEnum.EndSub)
            {
                return indentSpace + "}" + comment;
            }
            if (ctm.Kind == CtmFunction.KindEnum.EndFunction)
            {
                if (ctm.Parent != null && ctm is not CtmFunction) throw new DecodeException("parseFunc Error! Exit Function Parent Not Found.");
                if (ctm.Parent == null) throw new DecodeException("parseFunc Error! Exit Function Parent Null."); 

                return indentSpaceUp + "return result" + ((CtmFunction)ctm.Parent).Name + ";" + comment + Environment.NewLine +
                       indentSpace + "}";
            }


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
                //case CtmFunction.KindEnum.EndSub:
                //    return indentSpace + "}" + comment;
                //case CtmFunction.KindEnum.EndFunction:
                //    result = indentSpace + "return result" + ctm.Name + ";" + comment + Environment.NewLine;
                //    result += indentSpaceDown + "}";
                //    return result;
                case CtmFunction.KindEnum.ExitSub:
                    return indentSpace + "return;" + comment;
                case CtmFunction.KindEnum.ExitFunction:
                    return indentSpace + "return result" + ctm.Name + ";" + comment;

                case CtmFunction.KindEnum.StartSub:
                    result = indentSpace + accessModifierString + ' ' +
                             ctm.Name + '(' + argsValue + ") {";

                    return result;
                case CtmFunction.KindEnum.StartFunction:
                    result = indentSpace + accessModifierString + ' ' +
                             resultValue + ' ' +
                             ctm.Name + '(' + argsValue + ") {" + comment + Environment.NewLine;
                    result += indentSpaceUp + "var result" + ctm.Name + " = " + defaultValue(ctm.ResultTypeName);

                    return result;
            }

            return result;
        }

        private int getCtmIndent(CtmBase ctm)
        {
            // インデントは引き継がない。自動で４にする。
            if (ctm.Parent == null)
            {
                return 0;
            }
            else
            {
                if (ctm.Parent is CtmFunction)
                {
                    CtmFunction ctmFunc;
                    if (ctm is CtmFunction)
                    {
                        ctmFunc = (CtmFunction)ctm;
                        if (ctmFunc.Kind == CtmFunction.KindEnum.EndSub || ctmFunc.Kind == CtmFunction.KindEnum.EndFunction)
                        {
                            return ctmFunc.Parent == null ? 0 : ctmFunc.Parent.Indent;
                        }
                    }

                    ctmFunc = (CtmFunction)ctm.Parent;
                    if (ctmFunc.Kind == CtmFunction.KindEnum.StartSub || ctmFunc.Kind == CtmFunction.KindEnum.StartFunction)
                    {
                        return ctmFunc.Indent + 4; 
                    }



                }
                if (ctm.Parent is CtmIf)
                {
                    CtmIf ctmIf;

                    if (ctm is CtmIf)
                    {
                        ctmIf = (CtmIf)ctm;
                        if (ctmIf.Kind == CtmIf.KindEnum.EndIf)
                        {
                            return ctmIf.Parent == null ? 0 : ctmIf.Parent.Indent;
                        }
                    }
                    ctmIf = (CtmIf)ctm.Parent;
                    if (ctmIf.Kind == CtmIf.KindEnum.IfThen || ctmIf.Kind == CtmIf.KindEnum.ElseIfThen)
                    {
                        return ctmIf.Indent + 4;
                    }


                }
            }


            return ctm.Parent.Indent;
        }
        private string parseIf(CtmIf ctm)
        {
            string result = "if ({0}) ";
            string indentSpace = new string(' ', ctm.Indent);
            string comment = ctm.Comment != "" ? " //" + ctm.Comment : "";

            if (ctm.Kind == CtmIf.KindEnum.EndIf)
            {
                return indentSpace + "}" + comment;
            }

            // todo : 暫定版
            var condition = TryPrarseCondition(ctm.ConditionsValue);

            return indentSpace + string.Format(result, condition) + "{" + comment;
        }

        private string parseBase(CtmBase ctm)
        {
            string indentSpace = new string(' ', ctm.Indent);
            string comment = ctm.Comment != "" ? " //" + ctm.Comment : "";

            var spaceSplit = ctm.Value.Split(' ');
            if (spaceSplit.Length == 3 && spaceSplit[1] == "=")
            {
                // 親にFunctionがあれば、関数名を取得
                var funcName = getParentFuncName(ctm.Parent);

                // 関数名と同じであれば戻り値に変更
                var valueBuffLeft = ParseSide(spaceSplit[0]);
                var valueBuffRigth = ParseSide(spaceSplit[2]);
                if (funcName == valueBuffLeft)
                {
                    return indentSpace + "result" + funcName + " = " + valueBuffRigth + ";" + comment;
                }
                else
                {
                    return indentSpace + valueBuffLeft + " = " + valueBuffRigth + ";" + comment;
                }
            }

            return "";
        }

        private string getParentFuncName(CtmBase? ctm)
        {
            // なければ終了
            if (ctm == null) return "";
                 
            // ファンクションなら関数名を返す
            if (ctm.GetType() == typeof(CtmFunction))
            {
                return ((CtmFunction)ctm).Name;
            }

            // 親も遡って調べる
            return getParentFuncName(ctm.Parent);

        }

        // 左辺、右辺のパース
        public string ParseSide(string value)
        {
            if (value == "") return "";

            // True/Falseは小文字にする
            var valueBuff = value;
            if (valueBuff.ToLower() == "true" || valueBuff.ToLower() == "false")
            {
                valueBuff = valueBuff.ToLower();
            }

            // Nothingはnullにする
            if (valueBuff.ToLower() == "nothing") valueBuff = "null";

            // ![名前]の場合は.名前にする
            valueBuff = valueBuff.Replace("[", "");
            valueBuff = valueBuff.Replace("]", "");
            valueBuff = valueBuff.Replace('!', '.');

            return valueBuff;
        }

        public string ParseArgs(string[]? values)
        {

            if (values == null || (values.Length == 1 && values[0] == "")) throw new DecodeException("CSDecode Error! Args Item Null");

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

        private string defaultValue(string typeValue)
        {
            switch (typeValue.ToLower())
            {
                case "string":
                    return "string.Empty;";
                case "integer":
                case "double":
                case "long":
                    return "0;";
            }

            throw new DecodeException("defaultValue Not Case :" + typeValue);

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

            throw new DecodeException("ParseTypeValue Not Case :" + typeValue);
        }

        // 暫定
        public string TryPrarseCondition(string value)
        {
            var buff = value;

            if (buff.Length == 0) return value;

            if (buff.Substring(0,1) == "(")
            {
                // 優先のカッコあり
            }

            var notSign = false;
            if (buff.Substring(0, 4).ToLower() == "not ")
            {
                notSign = true;
                buff = buff.Substring(5);
            }

            // スペースなし。関数系なのでそのまま
            if (buff.IndexOf(' ') == -1)
            {
                if (notSign)
                {
                    return "!" + buff;
                }
                else
                {
                    return buff;
                }
            }

            // スペースありは条件式なので分解して確認
            var result = string.Empty;
            var spaceSplit = buff.Split(' ');
            if (spaceSplit.Length == 3)
            {

                var valueBuff = ParseSide(spaceSplit[2]);

                switch (spaceSplit[1])
                {
                    case "=":
                    case "is":
                        if (notSign)
                        {
                            result = spaceSplit[0] + " != " + valueBuff;
                        }
                        else
                        {
                            result = spaceSplit[0] + " == " + valueBuff;
                        }
                        break;
                }
            }


            return result;
        }

        public List<CtmCondition> PrarseCondition(string value)
        {
            if (value.Substring(0, 1) == "(")
            {
                // 優先のカッコあり
            }

            var notSign = false;
            if (value.Substring(0, 4).ToLower() == "not ")
            {
                notSign = true;
            }



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
