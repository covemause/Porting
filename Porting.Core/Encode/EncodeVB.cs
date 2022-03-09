using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Porting.Core.Data;

namespace Porting.Core.Encode
{
    /// <summary>
    /// VB6コンバート
    /// </summary>
    /// <remarks>
    /// 参考（VBA）：https://docs.microsoft.com/ja-jp/office/vba/language/reference/statements
    /// ※異なる構文もあるが参考にはなる
    /// 
    /// コンバートできない課題一覧
    /// ・On Error Resume Next : エラー続行
    /// </remarks>
    public class EncodeVB
    {
        /// <summary>
        /// エンコード種類
        /// </summary>
        private enum EncodePatturn
        {
            encCall, // 関数呼び出し
            encCode,
            encEnum,
            encError, // エラー発生とエラートラップがある
            encEvent,
            encFor,
            encFunc,
            encGoto,
            encIF,
            encPropaty,
            encSelect,
            encSystem,
            encType,
            encUnknown, // 判定不能または深く判定する前の状態
            encWhile,
            encWith,
        }

        public CtmBase? currentCtmBase = null;

        public void Execute(string[] codeLines)
        {
            currentCtmBase = null;
            foreach(var line in codeLines)
            {
                var ret = Execute(currentCtmBase, line);
                if (ret == null) throw new NullReferenceException();

                if (currentCtmBase == null)
                {
                    currentCtmBase = ret;
                }
                else
                {
                    ret.Parent = currentCtmBase;
                    currentCtmBase.InnerCtmList.Add(ret);
                }
            }
        }

        public CtmBase? Execute(CtmBase? current, string codeLine)
        {
            CtmBase? result = null;
            var baseContext = new CtmBaseContext(current, codeLine, GetIndent, GetComment);
            //if (ctmBuff == null) throw new Exception();

            // インデント、コメントは外している状態で判定
            //switch (getEncodePatturn(ctmBuff.Value))
            switch (getEncodePatturn(codeLine.Trim()))
            {
                case EncodePatturn.encCall:
                    break;
                case EncodePatturn.encCode:
                    break;
                case EncodePatturn.encEnum:
                    break;
                case EncodePatturn.encError:
                    break;
                case EncodePatturn.encEvent:
                    break;
                case EncodePatturn.encFor:
                    break;
                case EncodePatturn.encFunc:
                    result = new CtmFunction(baseContext,
                        new CtmFunctionContext(GetAccessModifier, GetFunctionKind, GetMethodName, GetFunctionArgs, GetFunctionResultValue));
                    break;
                case EncodePatturn.encGoto:
                    break;
                case EncodePatturn.encIF:
                    result = new CtmIf(baseContext,
                        new CtmIfContext(GetIfKind, GetConditions, GetInnerValue));
                    break;
                case EncodePatturn.encPropaty:
                    break;
                case EncodePatturn.encSelect:
                    break;
                case EncodePatturn.encSystem:
                    break;
                case EncodePatturn.encType:
                    break;
                case EncodePatturn.encUnknown:
                    break;
                case EncodePatturn.encWhile:
                    break;
                case EncodePatturn.encWith:
                    break;
                default:
                    throw new NotSupportedException("Not Patturn!");
            }

            return result;
        }

        private EncodePatturn getEncodePatturn(string codeLine)
        {
            var buff = codeLine.Trim().ToLower();

            // 先頭から数文字をチェックするため
            var buff2 = buff.Substring(0, 2);
            var buff3 = buff.Substring(0, 3);
            var buff4 = buff.Substring(0, 4);
            var buff5 = buff.Substring(0, 5);
            var buff6 = buff.Substring(0, 6);


            if ((buff2 == "do" && buff.Length == 2) ||
                (buff3 == "do "))
            {
                return EncodePatturn.encWhile; // DoやDo While objRs.EOF
            }

            if (buff3 == "end" && buff.Length == 3) 
            {
                return EncodePatturn.encSystem; // End
            }

            if (buff3 == "if ")
            {
                return EncodePatturn.encIF; // If xxx=10 Then
            }

            if (buff4 == "else")
            {
                return EncodePatturn.encSelect; // Else
            }

            if (buff4 == "for ")
            {
                return EncodePatturn.encFor; // For xxx=0 To 10 や For Each MyObject In MyCollection 
            }

            if (buff4 == "loop" && buff.Length == 4)
            {
                return EncodePatturn.encWhile; // Loop
            }

            if (buff4 == "wend" && buff.Length == 4)
            {
                return EncodePatturn.encWhile; // Wend
            }

            if ((buff4 == "next" && buff.Length == 4) ||
                (buff5 == "next "))
            {
                return EncodePatturn.encFor;  // Next or Next xxx
            }

            if (buff5 == "case ")
            {
                return EncodePatturn.encSelect; // case "10"
            }

            if (buff6 == "select ")
            {
                return EncodePatturn.encSelect; // select case Str(name)
            }
            if (buff6 == "const ")
            {
                return EncodePatturn.encCode; // Const MyVar = 459 
            }

            if (buff6 == "while ")
            {
                return EncodePatturn.encWhile; // While objRs.EOF
            }

            // スペース分割でのチェック
            return splitCheck(codeLine);
        }

        private EncodePatturn splitCheck(string codeLine)
        {
            var splitSpace = codeLine.ToLower().Split(' ');
            if (splitSpace.Length == -1)
            {
                // スペースなしは、再判定が必要
                // xxx=10といった通常コードのケース、Label:といったGotoラベルのケースがある
                return EncodePatturn.encUnknown;
            }


            if (splitSpace.Length == 2)
            {
                // Callステートメント
                if (splitSpace[0] == "Call") return EncodePatturn.encCall;

                // Endステートメント
                if (splitSpace[0] == "end")
                {
                    if (splitSpace[1] == "if") return EncodePatturn.encIF;
                    if (splitSpace[1] == "property") return EncodePatturn.encPropaty;
                    if (splitSpace[1] == "select") return EncodePatturn.encSelect;
                    if (splitSpace[1] == "function" || splitSpace[1] == "sub") return EncodePatturn.encFunc;
                    if (splitSpace[1] == "with") return EncodePatturn.encWith;
                    if (splitSpace[1] == "type") return EncodePatturn.encType;
                    // 上記以外は異常にする
                    throw new NotSupportedException("End Not Found! " + splitSpace[1]);
                }

                // Enumステートメント
                if (splitSpace[0] == "enum") return EncodePatturn.encEnum;

                // Error 11
                if (splitSpace[0] == "error") return EncodePatturn.encError;

                // Exitステートメント
                if (splitSpace[0] == "exit")
                {
                    if (splitSpace[1] == "function" || splitSpace[1] == "sub") return EncodePatturn.encFunc;
                    if (splitSpace[1] == "do") return EncodePatturn.encWhile;
                    if (splitSpace[1] == "property") return EncodePatturn.encPropaty;
                    if (splitSpace[1] == "for") return EncodePatturn.encFor;

                    // 上記以外は異常にする
                    throw new NotSupportedException("Exit Not Found! " + splitSpace[1]);
                }


                // Type Parson
                if (splitSpace[0] == "type") return EncodePatturn.encType;

                // Sub CheckData()、Function GetData()
                if (splitSpace[0] == "sub" || splitSpace[0] =="function") return EncodePatturn.encFunc;

                // Withステートメント
                if (splitSpace[0] == "with") return EncodePatturn.encWith;

            }

            if (splitSpace.Length == 3)
            {
                // Private Type Person
                if (splitSpace[1] == "type") return EncodePatturn.encType;
                // Enumステートメント
                if (splitSpace[1] == "enum") return EncodePatturn.encEnum;

            }

            if (splitSpace.Length > 2)
            {
                // Public Sub BinarySearch(. . .)
                // Public Function BinarySearch(. . .)
                // Public Function BinarySearch(. . .) As Boolean 
                if ((splitSpace[1] == "sub" || splitSpace[1] == "function"))
                {
                    return EncodePatturn.encFunc;
                }

                // Declare Sub First Lib "MyLib" ()
                // Private Declare Sub First Lib "MyLib" ()
                // Public Declare PtrSafe Function GetActiveWindow Lib "User32" () As LongPtr
                if (splitSpace[0] == "declare" || splitSpace[1] == "declare")
                {
                    return EncodePatturn.encSystem;
                }

                // On Error Goto ラベル
                // On Error Resume Next
                if (splitSpace[0] == "on" && splitSpace[1] == "error") return EncodePatturn.encError;

                // Property [Let|Get|Set] PenColor(ColorName As String)
                // Public Property [Let|Get|Set] PenColor(ColorName As String)
                if (splitSpace[0] == "property" || splitSpace[1] == "property") return EncodePatturn.encPropaty;


            }

            // 不明確なので再検証に回す
            return EncodePatturn.encUnknown;
        }


        #region CtmBaseContext

        public int GetIndent(string originalValue, string trimValue)
        {
            var indentLen = originalValue.IndexOf(trimValue);
            if (indentLen == -1)
            {
                throw new ArgumentException("異なる文字列のため、インデントを取得できません");
            }

            return indentLen;
        }

        public string GetComment(string originalValue)
        {
            var hasRead = true; // true : コメント読み込みとして可能
            var commentPos = 0;

            // テキスト内のシングルクォーテーション（"WHERE X = '5'"）はコメントとしてみなさないため
            // hasRead=falseにしておく。
            // たとえば、「strWhere = "WHERE X = '5'"   ' 5:固定を条件にする」 のような""外にある
            // シングルクォーテーションはコメントなので、hasRead=trueにする
            // つまりダブルクォーテーションに挟まれている間はコメントとしない。

            for (int i = 0; i < originalValue.Length; i++)
            {
                if (originalValue[i] == '"')
                {
                    hasRead = !hasRead;  // １回目Hitはfalse、２回目Hitはtrue・・・
                }

                if (hasRead && originalValue[i] == '\'')
                {
                    commentPos = i;
                    break;
                }
            }

            if (commentPos > 0)
            {
                return originalValue.Substring(commentPos);
            }

            return string.Empty;

        }
        public string GetConditions(string value)
        {
            // If から　Thenの間を取得する
            var splitSpace = value.Split(' ');
            var result = string.Empty;

            foreach(var item in splitSpace)
            {
                if (item.ToLower() == "then")
                {
                    break;
                }

                if (item.ToLower() == "if")
                {
                    continue;
                }
                result += item + " ";
            }

            return result.Trim();

        }
        #endregion

        #region  CtmIfContext
        public CtmIf.KindEnum GetIfKind(string value)
        {
            CtmIf.KindEnum? result = null;

            var trimVal = value.Trim().ToLower();
            if (trimVal == "else")
            {
                result = CtmIf.KindEnum.Else;
            }

            if (trimVal == "end if")
            {
                result = CtmIf.KindEnum.EndIf;
            }

            var splitSpace = trimVal.Split(' ');

            if (splitSpace.Length > 2)
            {
                // 最後がThenで終わっている場合は、ワンライナーではない
                if (splitSpace[splitSpace.Length - 1] == "then")
                {
                    if (splitSpace[0] == "else" && splitSpace[1] == "if")
                    {
                        result = CtmIf.KindEnum.ElseIfThen;
                    }
                    else if (splitSpace[0] == "if")
                    {
                        result = CtmIf.KindEnum.IfThen;
                    }
                }
                // 最後がThenで終わっていない場合は、ワンライナー
                else
                {
                    if (splitSpace[0] == "else" && splitSpace[1] == "if")
                    {
                        result = CtmIf.KindEnum.ElseIfTOnLiner;
                    }
                    else if (splitSpace[0] == "if")
                    {
                        result = CtmIf.KindEnum.IfOneLiner;
                    }
                }
            }

            // どれでもない場合は例外
            if (result == null) throw new NotSupportedException();

            return (CtmIf.KindEnum)result;
        }

        public string GetInnerValue(string value)
        {
            // Then以降を取得する
            var splitSpace = value.Split(' ');
            var isThen = false;
            var result = string.Empty;

            foreach (var item in splitSpace)
            {
                if (isThen) result += item + " ";
                if (item.ToLower() == "then") isThen = true;
            }

            return result.Trim();
        }
        #endregion

        #region CtmGotoContext

        public CtmGoto.KindEnum GetGotoKind(string value)
        {
            // 「Goto ラベル」ならGoto、それ以外はラベル
            return value.IndexOf(' ') > 0 ? CtmGoto.KindEnum.Goto : CtmGoto.KindEnum.Label;
        }

        #endregion

        #region CtmFunctionContext
        public CtmFunction.AccessModifierEnum GetAccessModifier(string value)
        {
            var buffHead = value.Substring(0, value.IndexOf(' ')).ToLower();

            switch (buffHead)
            {
                case "private":
                    return CtmFunction.AccessModifierEnum.Private;
                case "public":
                    return CtmFunction.AccessModifierEnum.Public;
                default:
                    // 省略と判断
                    return CtmFunction.AccessModifierEnum.Private;
            }

        }
        public string GetMethodName(string value)
        {
            // メソッド名後の（を起点に前方のスペースまで探す。スペースがなければ全部がメソッド名
            var buffHead = value.Substring(0, value.IndexOf('('));
            var buffSpaceList = buffHead.Split(' ');
            if (buffSpaceList.Length == 2) return buffSpaceList[1];
            if (buffSpaceList.Length == 3) return buffSpaceList[2];

            throw new NotSupportedException(); // どれにも該当しなければサポート外
        }

        public CtmFunction.KindEnum GetFunctionKind(string value)
        {
            // End
            if (value.Substring(0, 5).ToLower() == "end s") return CtmFunction.KindEnum.EndSub;
            if (value.Substring(0, 5).ToLower() == "end f") return CtmFunction.KindEnum.EndFunction;

            // Exit
            if (value.Substring(0, 6).ToLower() == "exit s") return CtmFunction.KindEnum.ExitSub;
            if (value.Substring(0, 6).ToLower() == "exit f") return CtmFunction.KindEnum.ExitFunction;


            var buffAccessPos = value.IndexOf(' ');
            var buffAccess = value.Substring(0, buffAccessPos);
            if (buffAccess.ToLower() == "public" || buffAccess.ToLower() == "private")
            {
                if (value.Substring(buffAccessPos + 1, 1).ToLower() == "s") return CtmFunction.KindEnum.StartSub;
                if (value.Substring(buffAccessPos + 1, 1).ToLower() == "f") return CtmFunction.KindEnum.StartFunction;
                throw new NotSupportedException(); // Function, Sub以外はサポート外
            }

            if (buffAccess == "Sub") return CtmFunction.KindEnum.StartSub;
            if (buffAccess == "Function") return CtmFunction.KindEnum.StartFunction;

            throw new NotSupportedException(); // どれにも該当しなければサポート外

        }

        public string[] GetFunctionArgs(string value)
        {
            var startPos = value.IndexOf('(') + 1;
            var endPos = value.IndexOf(')');

            if (endPos - startPos == 1) return new string[] { "" }; // ()の場合は空文字

            
            // ParseFullName(Path As String, FileName As String, Byref FullName As String) As Boolean => { "Path As String", " FileName As String", " Byref FullName As String" }
            var resultArgs = value.Substring(startPos, endPos - startPos).Split(',');

            // { "Path As String", " FileName As String", " Byref FullName As String" } =>{ "Path As String", "FileName As String","Byref FullName As String" }
            var buff = new List<string>();
            foreach(var arg in resultArgs)
            {
                buff.Add(arg.Trim());
            }
            return buff.ToArray();
            
            //return value.Substring(startPos, endPos - startPos).Split(", ").ToArray();
        }
        public string GetFunctionResultValue(string value)
        {
            var endPos = value.IndexOf(')');

            if (value.Length == endPos + 1) return "Varient";  // 戻り値の省略はVarient型

            var buff = value.Substring(endPos + 2).Split(' ');
            if (buff.Length == 2 && buff[0].ToLower() == "as") return buff[1];

            throw new NotSupportedException(); // どれにも該当しなければサポート外
        }

        #endregion

    }

}
