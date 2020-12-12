using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A120KifuSfen;
using Grayscale.A210KnowNingen.B170WordShogi.C250Masu;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B190Komasyurui.C500Util;

namespace Grayscale.A210KnowNingen.B360_MasusWriter.C500Util
{
    public abstract class UtilMasus<T1>
        where T1 : SyElement
    {

        /// <summary>
        /// 筋も残し、全件網羅
        /// </summary>
        /// <returns></returns>
        public static string LogStringConcrete(
            SySet<SyElement> masus
            )
        {
            StringBuilder sb = new StringBuilder();

            if (masus is SySet_Default<SyElement>)
            {
                // まず自分の要素
                foreach (SyElement hMasu1 in ((SySet_Default<SyElement>)masus).Elements_)
                {
                    Okiba okiba = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(hMasu1));
                    if (okiba == Okiba.ShogiBan)
                    {
                        int suji;
                        int dan;
                        Conv_Masu.ToSuji_FromBanjoMasu(hMasu1, out suji);
                        Conv_Masu.ToDan_FromBanjoMasu(hMasu1, out dan);

                        sb.Append("["
                            + suji.ToString()
                            + dan.ToString()
                            + "]");
                    }
                    else
                    {
                        // TODO: まだ使えない☆（＾～＾）
                        Piece piece;
                        Conv_Masu.ToPiece_FromBangaiMasu(hMasu1, out piece);

                        sb.Append("["
                            +
                            Util_Komasyurui14.NimojiPieces[(int)piece]
                            + "]");
                    }
                }

                // 次に親集合
                foreach (SySet<SyElement> superset in ((SySet_Default<SyElement>)masus).Supersets)
                {
                    sb.Append(UtilMasus<SyElement>.LogStringConcrete(superset));
                }
            }
            else if (masus is SySet_Ordered<SyElement>)
            {
                // まず自分の要素
                foreach (SyElement hMasu1 in ((SySet_Ordered<SyElement>)masus).Elements_)
                {
                    int suji;
                    int dan;

                    Okiba okiba = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(hMasu1));
                    if (okiba == Okiba.ShogiBan)
                    {
                        Conv_Masu.ToSuji_FromBanjoMasu(hMasu1, out suji);
                        Conv_Masu.ToDan_FromBanjoMasu(hMasu1, out dan);

                        sb.Append("["
                            + suji.ToString()
                            + dan.ToString()
                            + "]");
                    }
                    else
                    {
                        // TODO: まだ使えない☆（＾～＾）
                        Piece piece;
                        Conv_Masu.ToPiece_FromBangaiMasu(hMasu1, out piece);

                        sb.Append("["
                            +
                            Util_Komasyurui14.NimojiPieces[(int)piece]
                            + "]");
                    }
                }

                // 次に親集合
                foreach (SySet<SyElement> superset in ((SySet_Ordered<SyElement>)masus).Supersets)
                {
                    sb.Append(UtilMasus<SyElement>.LogStringConcrete(superset));
                }
            }
            else if (masus is SySet_DirectedSegment<SyElement>)
            {
                sb.Append("[");

                foreach (T1 hMasu1 in ((SySet_DirectedSegment<SyElement>)masus).Elements)
                {
                    int suji;
                    int dan;

                    Okiba okiba = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(hMasu1));
                    if (okiba == Okiba.ShogiBan)
                    {
                        Conv_Masu.ToSuji_FromBanjoMasu(hMasu1, out suji);
                        Conv_Masu.ToDan_FromBanjoMasu(hMasu1, out dan);

                        sb.Append(
                            suji.ToString()
                            + dan.ToString()
                            + "→");
                    }
                    else
                    {
                        // TODO: まだ使えない☆（＾～＾）
                        Piece piece;
                        Conv_Masu.ToPiece_FromBangaiMasu(hMasu1, out piece);

                        sb.Append(
                            Util_Komasyurui14.NimojiPieces[(int)piece]
                            + "→");
                    }
                }

                // 最後の矢印は削除します。
                if ("[".Length < sb.Length)
                {
                    sb.Remove(sb.Length - 1, 1);
                }

                sb.Append("]");
            }
            else
            {
            }

            return sb.ToString();
        }


        /// <summary>
        /// 重複をなくした表現
        /// </summary>
        /// <returns></returns>
        public static string LogString_Set(
            SySet<SyElement> masus
            )
        {
            StringBuilder sb = new StringBuilder();

            if (masus is SySet_Default<SyElement>)
            {

                HashSet<T1> set = new HashSet<T1>();

                // まず自分の要素
                foreach (T1 hMasu1 in ((SySet_Default<SyElement>)masus).Elements_)
                {
                    set.Add(hMasu1);
                }

                // 次に親集合
                foreach (SySet<T1> superset in ((SySet_Default<SyElement>)masus).Supersets)
                {
                    foreach (T1 hMasu2 in superset.Elements)
                    {
                        set.Add(hMasu2);
                    }
                }

                T1[] array = set.ToArray();
                Array.Sort(array);

                int fieldCount = 0;
                foreach (T1 masuHandle in array)
                {
                    sb.Append("[");
                    sb.Append(masuHandle);
                    sb.Append("]");

                    fieldCount++;

                    if (fieldCount % 20 == 19)
                    {
                        sb.AppendLine();
                    }
                }
            }
            else if (masus is SySet_Ordered<SyElement>)
            {
                // まず自分の要素
                foreach (T1 hMasu1 in ((SySet_Ordered<SyElement>)masus).Elements_)
                {
                    int suji;
                    int dan;

                    Okiba okiba = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(hMasu1));
                    if (okiba == Okiba.ShogiBan)
                    {
                        Conv_Masu.ToSuji_FromBanjoMasu(hMasu1, out suji);
                        Conv_Masu.ToDan_FromBanjoMasu(hMasu1, out dan);

                        sb.Append("["
                            + suji.ToString()
                            + dan.ToString()
                            + "]");
                    }
                    else
                    {
                        // TODO: まだ使えない☆（＾～＾）
                        Piece piece;
                        Conv_Masu.ToPiece_FromBangaiMasu(hMasu1, out piece);

                        sb.Append("["
                            +
                            Util_Komasyurui14.NimojiPieces[(int)piece]
                            + "]");
                    }
                }

                // 次に親集合
                foreach (SySet<SyElement> superset in ((SySet_Ordered<SyElement>)masus).Supersets)
                {
                    sb.Append(UtilMasus<INewBasho>.LogStringConcrete(superset));
                }
            }
            else if (masus is SySet_DirectedSegment<SyElement>)
            {
                sb.Append("[");

                foreach (T1 hMasu1 in ((SySet_DirectedSegment<SyElement>)masus).Elements)
                {
                    int suji;
                    int dan;

                    Okiba okiba = Conv_Masu.ToOkiba(Conv_Masu.ToMasuHandle(hMasu1));
                    if (okiba == Okiba.ShogiBan)
                    {
                        Conv_Masu.ToSuji_FromBanjoMasu(hMasu1, out suji);
                        Conv_Masu.ToDan_FromBanjoMasu(hMasu1, out dan);

                        sb.Append(
                            suji.ToString()
                            + dan.ToString()
                            + "→");
                    }
                    else
                    {
                        // TODO: まだ使えない☆（＾～＾）
                        Piece piece;
                        Conv_Masu.ToPiece_FromBangaiMasu(hMasu1, out piece);

                        sb.Append(
                            Util_Komasyurui14.NimojiPieces[(int)piece]
                            + "→");
                    }
                }

                // 最後の矢印は削除します。
                if ("[".Length < sb.Length)
                {
                    sb.Remove(sb.Length - 1, 1);
                }

                sb.Append("]");
            }
            else
            {
            }

            return sb.ToString();
        }

    }
}
