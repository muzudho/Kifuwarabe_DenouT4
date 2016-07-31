using Grayscale.P062_ConvText___.L500____Converter;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.L500____Util;
using Grayscale.P214_Masu_______.L500____Util;
using Grayscale.P218_Starlight__.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P234_Komahaiyaku.L500____Util;
using Grayscale.P238_Seiza______.L250____Struct;
using Grayscale.P238_Seiza______.L500____Util;
using Grayscale.P258_UtilSky258_.L500____UtilSky;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using Grayscale.P335_Move_______.L___500_Struct;

namespace Grayscale.P339_ConvKyokume.L500____Converter
{

    public abstract class Conv_SasiteStr_Sfen
    {
        /// <summary>
        /// 自動で削除される、棋譜ツリー・ログのルートフォルダー名。
        /// </summary>
        public const string KIFU_TREE_LOG_ROOT_FOLDER = "temp_root";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcNow">選択している駒の元の場所</param>
        /// <param name="dstNow">選択している駒の移動先</param>
        /// <param name="captured">あれば取った駒</param>
        /// <returns></returns>
        public static Move ToMove(
            Starlightable srcNow,
            Starlightable dstNow,
            Komasyurui14 captured
            )
        {
            Starbeamable sasite = new RO_Starbeam(srcNow, dstNow, captured);

            int v = 0;//バリュー（ビットフィールド）
            //System.Console.WriteLine("(1) move=" + Convert.ToString(v, 2));

            try
            {
                if (null == sasite || Util_Sky258A.NULL_OBJECT_SASITE == sasite)
                {
                    v |= 1 << (int)MoveShift.ErrorCheck;//エラー
                    //System.Console.WriteLine("(2) move=" + Convert.ToString(v, 2));

                    goto gt_EndMethod;
                }

                RO_Star srcKoma = Util_Starlightable.AsKoma(sasite.LongTimeAgo);
                RO_Star dstKoma = Util_Starlightable.AsKoma(sasite.Now);

                if (Util_Sky_BoolQuery.IsDaAction(sasite))
                {
                    // 打でした。
                    v |= 1 << (int)MoveShift.Drop;
                    //System.Console.WriteLine("(3) move=" + Convert.ToString(v, 2));
                }
                else
                {
                    //------------------------------------------------------------
                    // (自)筋
                    //------------------------------------------------------------
                    int srcSuji;
                    if (Util_MasuNum.TryMasuToSuji(srcKoma.Masu, out srcSuji))
                    {
                        v |= srcSuji << (int)MoveShift.SrcSuji;
                        //System.Console.WriteLine("(4) move=" + Convert.ToString(v, 2)+ " srcSuji="+ srcSuji);
                    }
                    else
                    {
                        v |= 1 << (int)MoveShift.ErrorCheck;//エラー
                        //System.Console.WriteLine("(5) move=" + Convert.ToString(v, 2));
                    }

                    //------------------------------------------------------------
                    // (自)段
                    //------------------------------------------------------------
                    int srcDan;
                    if (Util_MasuNum.TryMasuToDan(srcKoma.Masu, out srcDan))
                    {
                        v |= srcDan << (int)MoveShift.SrcDan;
                        //System.Console.WriteLine("(6) move=" + Convert.ToString(v, 2)+ " srcDan2="+ srcDan);
                    }
                    else
                    {
                        v |= 1 << (int)MoveShift.ErrorCheck;//エラー
                        //System.Console.WriteLine("(7) move=" + Convert.ToString(v, 2));
                    }
                }

                //------------------------------------------------------------
                // (至)筋
                //------------------------------------------------------------
                int dstSuji;
                if (Util_MasuNum.TryMasuToSuji(dstKoma.Masu, out dstSuji))
                {
                    v |= dstSuji << (int)MoveShift.DstSuji;
                    //System.Console.WriteLine("(8) move=" + Convert.ToString(v, 2)+ " dstSuji="+ dstSuji);
                }
                else
                {
                    v |= 1 << (int)MoveShift.ErrorCheck;//エラー
                    //System.Console.WriteLine("(9) move=" + Convert.ToString(v, 2));
                }


                //------------------------------------------------------------
                // (至)段
                //------------------------------------------------------------
                int dstDan;
                if (Util_MasuNum.TryMasuToDan(dstKoma.Masu, out dstDan))
                {
                    v |= dstDan << (int)MoveShift.DstDan;
                    //System.Console.WriteLine("(10) move=" + Convert.ToString(v, 2)+ " dstDan="+ dstDan);
                }
                else
                {
                    v |= 1 << (int)MoveShift.ErrorCheck;//エラー
                    //System.Console.WriteLine("(11) move=" + Convert.ToString(v, 2));
                }


                //------------------------------------------------------------
                // 成
                //------------------------------------------------------------
                if (Util_Sky_BoolQuery.IsNatta_Sasite(sasite))
                {
                    v |= 1 << (int)MoveShift.Promotion;
                    //System.Console.WriteLine("(12) move=" + Convert.ToString(v, 2));
                }


                //------------------------------------------------------------
                // 手番
                //------------------------------------------------------------
                if (Util_Sky_BoolQuery.IsSente(sasite))
                {
                    //立てない。
                    //System.Console.WriteLine("(13) move=" + Convert.ToString(v, 2));
                }
                else
                {
                    // 後手
                    v |= 1 << (int)MoveShift.Playerside;
                    //System.Console.WriteLine("(14) move=" + Convert.ToString(v, 2));
                }


                //------------------------------------------------------------
                // 移動した駒の種類
                //------------------------------------------------------------
                Komasyurui14 komasyurui = Util_Starlightable.AsKoma(sasite.Now).Komasyurui;
                if (Komasyurui14.H00_Null___ != komasyurui)
                {
                    v |= (int)komasyurui << (int)MoveShift.Komasyurui;
                    //System.Console.WriteLine("(15) move=" + Convert.ToString(v, 2)+ " komasyurui="+ (int)komasyurui);
                }


                //------------------------------------------------------------
                // 取った駒の種類
                //------------------------------------------------------------
                if (Komasyurui14.H00_Null___ != (Komasyurui14)sasite.FoodKomaSyurui)
                {
                    int food = (int)sasite.FoodKomaSyurui;
                    //System.Console.WriteLine("(--) food=" + Convert.ToString((int)food, 2));
                    v |= food << (int)MoveShift.Captured;
                    //System.Console.WriteLine("(16) move=" + Convert.ToString(v, 2));
                }
            }
            catch (Exception)
            {
                v |= 1 << (int)MoveShift.ErrorCheck;//エラー
                //System.Console.WriteLine("(17) move=" + Convert.ToString(v, 2));
            }

            gt_EndMethod:

            return (Move)v;
        }
    }
}
