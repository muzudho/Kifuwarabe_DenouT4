﻿using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B200_Masu_______.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter
{
    /// <summary>
    /// ＲＯ＿Ｓｔａｒ と Busstop の変換☆
    /// </summary>
    public abstract class Conv_Busstop
    {
        /*
        public static Busstop ToBusstop(RO_Star star)
        {
            int errorCheck = 0;

            // バリュー（ビット・フィールド）
            int v = 0;

            int suji;
            if (!Util_MasuNum.TryMasuToSuji(star.Masu, out suji))
            {
                errorCheck = 1;
            }

            int dan;
            if (!Util_MasuNum.TryMasuToDan(star.Masu, out dan))
            {
                errorCheck = 1;
            }

            int komasyurui = (int)star.Komasyurui;

            int komadai = Okiba.ShogiBan != Conv_SyElement.ToOkiba(star.Masu) ? 1 : 0;

            int playerside = Playerside.P1 == star.Pside ? 0 : 1;


            v |= suji << (int)BusstopShift.Suji;
            v |= dan << (int)BusstopShift.Dan;
            v |= komasyurui << (int)BusstopShift.Komasyurui;
            v |= komadai << (int)BusstopShift.Komadai;
            v |= playerside << (int)BusstopShift.Playerside;
            v |= errorCheck << (int)BusstopShift.ErrorCheck;

            return (Busstop)v;
        }
        */

        public static Busstop ToBusstop(Playerside pside, SyElement masu, Komasyurui14 komasyrui)
        {
            int errorCheck = 0;

            // バリュー（ビット・フィールド）
            int v = 0;

            int suji;
            if(!Util_MasuNum.TryMasuToSuji(masu, out suji))
            {
                errorCheck = 1;
            }

            int dan;
            if (!Util_MasuNum.TryMasuToDan(masu, out dan))
            {
                errorCheck = 1;
            }

            int komasyurui2 = (int)komasyrui;

            int komadai = Okiba.ShogiBan != Conv_SyElement.ToOkiba(masu) ? 1 : 0;

            int playerside = Playerside.P1 == pside ? 0 : 1;


            v |= suji << (int)BusstopShift.Suji;
            v |= dan << (int)BusstopShift.Dan;
            v |= komasyurui2 << (int)BusstopShift.Komasyurui;
            v |= komadai << (int)BusstopShift.Komadai;
            v |= playerside << (int)BusstopShift.Playerside;
            v |= errorCheck << (int)BusstopShift.ErrorCheck;

            return (Busstop)v;
        }

        public static Okiba ToOkiba(Busstop busstop)
        {
            if (Conv_Busstop.ToKomadai(busstop))
            {
                if (Playerside.P1 == Conv_Busstop.ToPlayerside(busstop))
                {
                    return Okiba.Sente_Komadai;
                }
                else if (Playerside.P2 == Conv_Busstop.ToPlayerside(busstop))
                {
                    return Okiba.Gote_Komadai;
                }
                else
                {
                    //TODO: エラーチェック
                    return Okiba.Empty;
                }
            }
            else
            {
                return Okiba.ShogiBan;
            }
        }

        /// <summary>
        /// 置き場の情報を補完するように注意すること☆（＾～＾）
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static SyElement ToMasu(Busstop busstop)
        {
            int v = (int)busstop;              // バリュー

            // TODO: エラーチェック
            if (Conv_Busstop.ToErrorCheck(busstop))
            {
                return Masu_Honshogi.Query_ErrorMasu();
            }

            // 打かどうか。
            Okiba okiba = Conv_Busstop.ToOkiba(busstop);
            if (Okiba.Empty == okiba)
            {
                // TODO: エラーチェック
                return Masu_Honshogi.Query_ErrorMasu();
            }


            // 筋
            int suji;
            {
                int m = (int)BusstopMask.Suji;  // マスク
                int s = (int)BusstopShift.Suji;   // シフト
                suji = (v & m) >> s;
            }

            // 段
            int dan;
            {
                int m = (int)BusstopMask.Dan;
                int s = (int)BusstopShift.Dan;
                dan = (v & m) >> s;
            }

            //────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────

            // 自
            return Util_Masu10.OkibaSujiDanToMasu(okiba, suji, dan);
        }

        public static bool ToKomadai(Busstop busstop)
        {
            int v = (int)busstop;              // バリュー

            // TODO: エラーチェック
            if (Conv_Busstop.ToErrorCheck(busstop))
            {
                return false;//FIXME:
            }

            // 打たない
            return 0 != (v & (int)BusstopMask.Komadai);
        }

        public static Komasyurui14 ToKomasyurui(Busstop busstop)
        {
            int v = (int)busstop;              // バリュー

            // TODO: エラーチェック
            if (Conv_Busstop.ToErrorCheck(busstop))
            {
                return Komasyurui14.H00_Null___;
            }

            // 移動した駒の種類
            int komasyurui;
            {
                int m = (int)BusstopMask.Komasyurui;
                int s = (int)BusstopShift.Komasyurui;
                komasyurui = (v & m) >> s;
            }

            //────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────

            // 移動した駒の種類
            return (Komasyurui14)komasyurui;
        }

        public static Playerside ToPlayerside(Busstop busstop)
        {
            int v = (int)busstop;              // バリュー

            // TODO: エラーチェック
            if (Conv_Busstop.ToErrorCheck(busstop))
            {
                return Playerside.Empty;
            }

            // 手番
            int playerside;
            {
                int m = (int)BusstopMask.Playerside;
                int s = (int)BusstopShift.Playerside;
                playerside = (v & m) >> s;
            }

            //────────────────────────────────────────────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────────────────────────────────────────────

            // 手番
            if (playerside == 1)
            {
                return Playerside.P2;
            }
            else
            {
                return Playerside.P1;
            }
        }

        public static bool ToErrorCheck(Busstop busstop)
        {
            int v = (int)busstop;              // バリュー

            // TODO: エラーチェック
            return 0 != (v & (int)BusstopMask.ErrorCheck);
        }

    }
}