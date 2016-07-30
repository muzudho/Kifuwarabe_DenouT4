using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P062_ConvText___.L500____Converter;
using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P213_Komasyurui_.L500____Util;
using Grayscale.P214_Masu_______.L500____Util;
using Grayscale.P218_Starlight__.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P234_Komahaiyaku.L500____Util;
using Grayscale.P238_Seiza______.L250____Struct;
using Grayscale.P335_Move.L___500_Struct;
using System;
using System.Text;

namespace Grayscale.P339_ConvKyokume.L500____Converter
{
    public abstract class Conv_Move
    {
        /// <summary>
        /// ************************************************************************************************************************
        /// SFEN符号表記。
        /// ************************************************************************************************************************
        /// </summary>
        /// <returns></returns>
        public static string ToSfen(Move move)
        {
            StringBuilder sb = new StringBuilder();

            int v = (int)move;//バリュー（ビットフィールド）

            try
            {
                if (0 != ((v & (int)MoveMask.ErrorCheck)))
                {
                    sb.Append(Conv_SasiteStr_Sfen.KIFU_TREE_LOG_ROOT_FOLDER);
                    goto gt_EndMethod;
                }


                if (0 != ((v & (int)MoveMask.Drop)))
                {
                    // 打でした。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    // 移動した駒の種類
                    int komasyurui;
                    {
                        int m = (int)MoveMask.Komasyurui;   // マスク
                        int s = (int)MoveShift.Komasyurui;    // シフト
                        komasyurui = (v& m) >> s;
                    }

                    // (自)筋・(自)段は書かずに、「P*」といった表記で埋めます。
                    sb.Append(Util_Komasyurui14.SfenDa[(int)komasyurui]);
                    sb.Append("*");
                }
                else
                {
                    //------------------------------------------------------------
                    // (自)筋
                    //------------------------------------------------------------
                    // 自筋
                    string strSrcSuji;
                    int srcSuji;
                    {
                        int m = (int)MoveMask.SrcSuji;
                        int s = (int)MoveShift.SrcSuji;
                        srcSuji = (v & m) >> s;
                    }
                    if (Util_Masu10.YukoSuji(srcSuji))
                    {
                        strSrcSuji = srcSuji.ToString();
                    }
                    else
                    {
                        strSrcSuji = "Ｎ筋";//エラー表現
                    }
                    sb.Append(strSrcSuji);

                    //------------------------------------------------------------
                    // (自)段
                    //------------------------------------------------------------
                    string strSrcDan2;
                    int srcDan2;
                    {
                        int m = (int)MoveMask.SrcDan;
                        int s = (int)MoveShift.SrcDan;
                        srcDan2 = (v & m) >> s;
                    }
                    if (Util_Masu10.YukoDan(srcDan2))
                    {
                        strSrcDan2 = Conv_Int.ToAlphabet(srcDan2);
                    }
                    else
                    {
                        strSrcDan2 = "Ｎ段";//エラー表現
                    }
                    sb.Append(strSrcDan2);
                }

                //------------------------------------------------------------
                // (至)筋
                //------------------------------------------------------------
                string strSuji;
                int suji2;
                {
                    int m = (int)MoveMask.DstSuji;
                    int s = (int)MoveShift.DstSuji;
                    suji2 = (v & m) >> s;
                }
                if (Util_Masu10.YukoSuji(suji2))
                {
                    strSuji = suji2.ToString();
                }
                else
                {
                    strSuji = "Ｎ筋";//エラー表現
                }
                sb.Append(strSuji);


                //------------------------------------------------------------
                // (至)段
                //------------------------------------------------------------
                string strDan;
                int dan2;
                {
                    int m = (int)MoveMask.DstDan;
                    int s = (int)MoveShift.DstDan;
                    dan2 = (v & m) >> s;
                }
                if (Util_Masu10.YukoDan(dan2))
                {
                    strDan = Conv_Int.ToAlphabet(dan2);
                }
                else
                {
                    strDan = "Ｎ段";//エラー表現
                }
                sb.Append(strDan);


                //------------------------------------------------------------
                // 成
                //------------------------------------------------------------
                int promotion;
                {
                    int m = (int)MoveMask.Promotion;
                    int s = (int)MoveShift.Promotion;
                    promotion = (v & m) >> s;
                }
                if (1== promotion)
                {
                    sb.Append("+");
                }
            }
            catch (Exception e)
            {
                sb.Append(e.Message);//FIXME:
            }

            gt_EndMethod:
            ;
            return sb.ToString();
        }

        public static Starbeamable ToSasite(Move move)
        {
            // TODO: エラーチェック
            // 無視

            int v = (int)move;              // バリュー

            // 自筋
            int srcSuji;
            {
                int m = (int)MoveMask.SrcSuji;  // マスク
                int s = (int)MoveShift.SrcSuji;   // シフト
                srcSuji = (v & m) >> s;
            }

            // 自段
            int srcDan;
            {
                int m = (int)MoveMask.SrcDan;
                int s = (int)MoveShift.SrcDan;
                srcDan = (v & m) >> s;
            }

            // 至筋
            int dstSuji;
            {
                int m = (int)MoveMask.DstSuji;
                int s = (int)MoveShift.DstSuji;
                dstSuji = (v & m) >> s;
            }

            // 至段
            int dstDan;
            {
                int m = (int)MoveMask.DstDan;
                int s = (int)MoveShift.DstDan;
                dstDan = (v & m) >> s;
            }

            // 成らない
            int promotion;
            {
                int m = (int)MoveMask.Promotion;
                int s = (int)MoveShift.Promotion;
                promotion = (v & m) >> s;
            }

            // 打たない
            int drop;
            {
                int m = (int)MoveMask.Drop;
                int s = (int)MoveShift.Drop;
                drop = (v & m) >> s;
            }

            // 手番
            int playerside;
            {
                int m = (int)MoveMask.Playerside;
                int s = (int)MoveShift.Playerside;
                playerside = (v & m) >> s;
            }

            // 移動した駒の種類
            int komasyurui;
            {
                int m = (int)MoveMask.Komasyurui;
                int s = (int)MoveShift.Komasyurui;
                komasyurui = (v & m) >> s;
            }

            // 取った駒の種類
            int captured;
            {
                int m = (int)MoveMask.Captured;
                int s = (int)MoveShift.Captured;
                captured = (v & m) >> s;
            }

            //────────────────────────────────────────────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────────────────────────────────────────────

            // 自
            SyElement srcMasuB;
            {
                srcMasuB = Util_Masu10.OkibaSujiDanToMasu(Okiba.ShogiBan, srcSuji, srcDan);
            }

            // 至
            SyElement dstMasuB;
            {
                dstMasuB = Util_Masu10.OkibaSujiDanToMasu(Okiba.ShogiBan, dstSuji, dstDan);
            }

            // 手番
            Playerside playersideB;
            {
                if (playerside == 1)
                {
                    playersideB = Playerside.P2;
                }
                else
                {
                    playersideB = Playerside.P1;
                }
            }

            // 移動した駒の種類
            Komasyurui14 komasyuruiB;
            {
                komasyuruiB = (Komasyurui14)komasyurui;
            }

            // 取った駒の種類
            Komasyurui14 capturedB;
            {
                capturedB = (Komasyurui14)captured;
            }


            return new RO_Starbeam(
                new RO_Star(playersideB, srcMasuB, komasyuruiB),
                new RO_Star(playersideB, dstMasuB, komasyuruiB),
                capturedB
                );
        }
    }
}
