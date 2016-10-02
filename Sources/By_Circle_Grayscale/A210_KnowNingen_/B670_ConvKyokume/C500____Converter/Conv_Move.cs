using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A060_Application.B620_ConvText___.C500____Converter;
using Grayscale.A180_KifuCsa____.B120_KifuCsa____.C___250_Struct;
using Grayscale.A180_KifuCsa____.B120_KifuCsa____.C250____Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using System;
using System.Diagnostics;
using System.Text;

namespace Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter
{
    public abstract class Conv_Move
    {
        /// <summary>
        /// もともとは、自動で削除される、棋譜ツリー・ログのルートフォルダー名。
        /// </summary>
        public const string KIFU_TREE_LOG_ROOT_FOLDER = "temp_root";

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
                    sb.Append(Conv_Move.KIFU_TREE_LOG_ROOT_FOLDER);
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
                        komasyurui = (v & m) >> s;
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
                    if (Conv_Masu.YukoSuji(srcSuji))
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
                    if (Conv_Masu.YukoDan(srcDan2))
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
                if (Conv_Masu.YukoSuji(suji2))
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
                if (Conv_Masu.YukoDan(dan2))
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

        /// <summary>
        /// ************************************************************************************************************************
        /// SFEN符号表記。
        /// ************************************************************************************************************************
        /// 
        /// ファイル名にも使えるように、ファイル名に使えない文字を置換します。
        /// </summary>
        /// <returns></returns>
        public static string ToSfen_ForFilename(Move move)
        {
            string moveSfen = Conv_Move.ToSfen(move);
            return Conv_Filepath.ToEscape(moveSfen);
        }

        /// <summary>
        /// 置き場の情報を補完するように注意すること☆（＾～＾）
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static SyElement ToSrcMasu(Move move)
        {
            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            if (Conv_Move.ToErrorCheck(move))
            {
                return Masu_Honshogi.Query_ErrorMasu();
            }

            Okiba okiba;

            // 打かどうか。
            if (Conv_Move.ToDrop(move))
            {
                // 打なら
                if (Playerside.P1 == Conv_Move.ToPlayerside(move))
                {
                    okiba = Okiba.Sente_Komadai;
                }
                else if(Playerside.P2 == Conv_Move.ToPlayerside(move))
                {
                    okiba = Okiba.Gote_Komadai;
                }
                else
                {
                    //TODO: エラーチェック
                    return Masu_Honshogi.Query_ErrorMasu();
                }
            }
            else
            {
                okiba = Okiba.ShogiBan;
            }

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

            //────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────

            // 自
            if (okiba == Okiba.ShogiBan)
            {
                return Conv_Masu.ToMasu_FromBanjoSujiDan(srcSuji, srcDan);
            }

            // 打なら
            return Conv_Masu.ToMasu_FromBangaiSujiDan(okiba, srcSuji, srcDan);
        }
        /// <summary>
        /// 置き場の情報を補完するように注意すること☆（＾～＾）
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static SyElement ToSrcMasu(Move move, Sky positionA)
        {
            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            if (Conv_Move.ToErrorCheck(move))
            {
                return Masu_Honshogi.Query_ErrorMasu();
            }


            // 打かどうか。
            if (Conv_Move.ToDrop(move))
            {
                // 打なら
                Playerside pside = Conv_Move.ToPlayerside(move);
                if (Playerside.Empty == pside)
                {
                    //TODO: エラーチェック
                    return Masu_Honshogi.Query_ErrorMasu();
                }

                Komasyurui14 ks14 = Conv_Move.ToSrcKomasyurui(move);
                return Conv_Masu.ToMasu_FromKomadaiKomasyurui(pside, ks14, positionA);
            }
            else
            {
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

                return Conv_Masu.ToMasu_FromBanjoSujiDan(srcSuji, srcDan);
            }
        }


        public static int ToDstDan(Move move)
        {
            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            int errorCheck;
            {
                int m = (int)MoveMask.ErrorCheck;  // マスク
                int s = (int)MoveShift.ErrorCheck;   // シフト
                errorCheck = (v & m) >> s;
            }
            if (0 != errorCheck)
            {
                return Conv_Masu.ERROR_DAN;
            }

            // 至段
            int dstDan;
            {
                int m = (int)MoveMask.DstDan;
                int s = (int)MoveShift.DstDan;
                dstDan = (v & m) >> s;
            }

            return dstDan;
        }

        public static SyElement ToDstMasu(Move move)
        {
            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            int errorCheck;
            {
                int m = (int)MoveMask.ErrorCheck;  // マスク
                int s = (int)MoveShift.ErrorCheck;   // シフト
                errorCheck = (v & m) >> s;
            }
            if (0 != errorCheck)
            {
                return Masu_Honshogi.Query_ErrorMasu();
            }

            // 至筋
            int dstSuji;
            {
                int m = (int)MoveMask.DstSuji;
                int s = (int)MoveShift.DstSuji;
                dstSuji = (v & m) >> s;
            }

            //────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────

            // 至段
            int dstDan = Conv_Move.ToDstDan(move);

            // 至
            return Conv_Masu.ToMasu_FromBanjoSujiDan( dstSuji, dstDan);
        }

        public static bool ToPromotion(Move move)
        {
            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            int errorCheck;
            {
                int m = (int)MoveMask.ErrorCheck;  // マスク
                int s = (int)MoveShift.ErrorCheck;   // シフト
                errorCheck = (v & m) >> s;
            }
            if (0 != errorCheck)
            {
                return false;//FIXME:
            }

            // 成らない
            int promotion;
            {
                int m = (int)MoveMask.Promotion;
                int s = (int)MoveShift.Promotion;
                promotion = (v & m) >> s;
            }

            //────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────

            return 0 != promotion;
        }

        public static Komasyurui14 ToSrcKomasyurui(Move move)
        {
            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            if (Conv_Move.ToErrorCheck(move))
            {
                return Komasyurui14.H00_Null___;
            }

            // 移動した駒の種類
            int komasyurui;
            {
                int m = (int)MoveMask.Komasyurui;
                int s = (int)MoveShift.Komasyurui;
                komasyurui = (v & m) >> s;
            }

            //────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────

            // 移動した駒の種類
            return (Komasyurui14)komasyurui;
        }

        public static Komasyurui14 ToDstKomasyurui(Move move)
        {
            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            int errorCheck;
            {
                int m = (int)MoveMask.ErrorCheck;  // マスク
                int s = (int)MoveShift.ErrorCheck;   // シフト
                errorCheck = (v & m) >> s;
            }
            if (0 != errorCheck)
            {
                return Komasyurui14.H00_Null___;
            }

            // 移動した駒の種類
            int komasyurui;
            {
                int m = (int)MoveMask.Komasyurui;
                int s = (int)MoveShift.Komasyurui;
                komasyurui = (v & m) >> s;
            }

            bool promotion = Conv_Move.ToPromotion(move);

            //────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────

            if (promotion)
            {
                return Util_Komasyurui14.ToNariCase((Komasyurui14)komasyurui);
            }
            else
            {
                // 移動した駒の種類
                return (Komasyurui14)komasyurui;
            }
        }

        public static Move SetCaptured(Move baseMove, Komasyurui14 foodKoma)
        {
            return (Move)(
                //
                ((int)baseMove)
                // マスクを重ねる。
                &
                ~(((int)MoveMask.Captured) << ((int)MoveShift.Captured))
                // 駒を足す。
                |
                (((int)foodKoma) << ((int)MoveShift.Captured))
                //
                );
        }

        public static Komasyurui14 ToCaptured(Move move)
        {
            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            int errorCheck;
            {
                int m = (int)MoveMask.ErrorCheck;  // マスク
                int s = (int)MoveShift.ErrorCheck;   // シフト
                errorCheck = (v & m) >> s;
            }
            if (0 != errorCheck)
            {
                return Komasyurui14.H00_Null___;
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

            // 取った駒の種類
            return (Komasyurui14)captured;
        }

        public static Playerside ToPlayerside(Move move)
        {
            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            if (Conv_Move.ToErrorCheck(move))
            {
                return Playerside.Empty;
            }

            // 手番
            int playerside;
            {
                int m = (int)MoveMask.Playerside;
                int s = (int)MoveShift.Playerside;
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

        public static bool ToDrop(Move move)
        {
            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            if (Conv_Move.ToErrorCheck(move))
            {
                return false;//FIXME:
            }

            // 打たない
            return 0 != (v & (int)MoveMask.Drop);
        }

        public static bool ToErrorCheck(Move move)
        {
            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            return 0 != (v & (int)MoveMask.ErrorCheck);
        }

        /*
        public static Move ToMove_ByDrop(
            SyElement dstMasu,
            Komasyurui14 srcKs,
            Playerside pside
        )
        {
            // 打と分かっていれば、成りはないぜ☆（＾▽＾）

            return Conv_Move.ToMove(
                Masu_Honshogi.Query_ErrorMasu(),//元位置は指定なしだぜ☆（＾▽＾）
                dstMasu,
                srcKs,
                Komasyurui14.H00_Null___,//打で取れる駒は無いぜ☆（＾▽＾）
                false,//成りは無い☆
                true,//打と決まっているぜ☆（＾▽＾）
                pside,
                false//とりあえずエラーは無しということで☆（＾▽＾）
                );
        }
        */

        public static Move ToMove(
            SyElement srcMasu,
            SyElement dstMasu,
            Komasyurui14 srcKs,
            Komasyurui14 dstKs,//成り判定用
            Komasyurui14 captured,
            //bool promotion,
            //bool drop,
            Playerside pside,
            bool errorCheck
            )
        {
            bool promotion;
            {
                // 元種類が不成、現種類が成　の場合のみ真。

                // 成立しない条件を１つでも満たしていれば、偽　確定。
                if (
                    Komasyurui14.H00_Null___ == srcKs
                    ||
                    Komasyurui14.H00_Null___ == dstKs
                    ||
                    Util_Komasyurui14.FlagNari[(int)srcKs]
                    ||
                    !Util_Komasyurui14.FlagNari[(int)dstKs]
                    )
                {
                    promotion = false;
                }
                else
                {
                    promotion = true;
                }
            }

            bool drop;
            try
            {
                drop = Okiba.ShogiBan != Conv_Masu.ToOkiba(srcMasu)//駒台（駒袋）から打ったとき。
                    && Okiba.Empty != Conv_Masu.ToOkiba(srcMasu);//初期配置から移動しても、打にはしません。
            }
            catch (Exception ex)
            {
                drop = false;
                //Util_OwataMinister.ERROR.Logger.DonimoNaranAkirameta(ex, "IsDaAction:");// exceptionArea=" + exceptionArea
                throw ex;
            }

            return Conv_Move.ToMove(
                srcMasu,
                dstMasu,
                srcKs,
                captured,
                promotion,
                drop,
                pside,
                errorCheck
                );
        }

        public static Move ToMove(
            SyElement srcMasu,
            SyElement dstMasu,
            Komasyurui14 moved,
            Komasyurui14 captured,
            bool promotion,
            bool drop,
            Playerside pside,
            bool errorCheck
            )
        {
            int srcSuji;
            int srcDan;
            Okiba srcOkiba = Conv_Masu.ToOkiba(srcMasu);
            if (srcOkiba == Okiba.ShogiBan)
            {
                Conv_Masu.ToSuji_FromBanjoMasu(srcMasu, out srcSuji);
                Conv_Masu.ToDan_FromBanjoMasu(srcMasu, out srcDan);
            }
            else
            {
                Conv_Masu.ToSuji_FromBangaiMasu(srcMasu, out srcSuji);
                Conv_Masu.ToDan_FromBangaiMasu(srcMasu, out srcDan);
            }

            int dstSuji;
            int dstDan;
            Okiba dstOkiba = Conv_Masu.ToOkiba(dstMasu);
            if (dstOkiba == Okiba.ShogiBan)
            {
                Conv_Masu.ToSuji_FromBanjoMasu(dstMasu, out dstSuji);
                Conv_Masu.ToDan_FromBanjoMasu(dstMasu, out dstDan);
            }
            else
            {
                Conv_Masu.ToSuji_FromBangaiMasu(dstMasu, out dstSuji);
                Conv_Masu.ToDan_FromBangaiMasu(dstMasu, out dstDan);
            }

            // バリュー
            int v = 0;
            v |= srcSuji << (int)MoveShift.SrcSuji;
            v |= srcDan << (int)MoveShift.SrcDan;
            v |= dstSuji << (int)MoveShift.DstSuji;
            v |= dstDan << (int)MoveShift.DstDan;
            v |= (int)moved << (int)MoveShift.Komasyurui;
            v |= (int)captured << (int)MoveShift.Captured;
            if (promotion)
            {
                v |= 1 << (int)MoveShift.Promotion;
            }

            if (drop || 
                (Okiba.Sente_Komadai== srcOkiba
                |
                Okiba.Gote_Komadai== srcOkiba
                )
            ){
                v |= 1 << (int)MoveShift.Drop;
            }

            if (Playerside.P2==pside)
            {
                v |= 1 << (int)MoveShift.Playerside;
            }

            if (errorCheck)
            {
                v |= 1 << (int)MoveShift.ErrorCheck;
            }

            return (Move)v;
        }


        /// <summary>
        /// FIXME: 未テスト。
        /// 
        /// CSAの指し手を、Moveに変換します。
        /// </summary>
        /// <param name="csa"></param>
        /// <param name="ittemae_Sky">1手前の局面。ルート局面などの理由で１手前の局面がない場合はヌル。</param>
        /// <returns></returns>
        public static Move ToMove_ByCsa(CsaKifuSasite csa, Sky ittemae_Sky_orNull)
        {
            bool errorCheck = false;

            // 取った駒は分からないぜ☆（＾～＾）
            Komasyurui14 captured = Komasyurui14.H00_Null___;

            SyElement dstMasu;
            {
                int dstSuji;
                int.TryParse(csa.DestinationMasu[0].ToString(), out dstSuji);
                int dstDan;
                int.TryParse(csa.DestinationMasu[1].ToString(), out dstDan);
                dstMasu = Conv_Masu.ToMasu_FromBanjoSujiDan( dstSuji, dstDan);
            }

            SyElement srcMasu;
            {
                // 元位置の筋と段は、あとで必ず使う。（成りの判定）
                int srcSuji;
                int.TryParse(csa.SourceMasu[0].ToString(), out srcSuji);
                int srcDan;
                int.TryParse(csa.SourceMasu[1].ToString(), out srcDan);
                srcMasu = Conv_Masu.ToMasu_FromBanjoSujiDan( srcSuji, srcDan);
            }

            bool drop;
            if ("00" == csa.SourceMasu)
            {
                // 打
                drop = true;
            }
            else
            {
                drop = false;
            }

            Komasyurui14 komasyurui;
            switch (csa.Syurui)
            {
                case Word_Csa.FU_FU_____: komasyurui = Komasyurui14.H01_Fu_____; break;
                case Word_Csa.KY_KYO____: komasyurui = Komasyurui14.H02_Kyo____; break;
                case Word_Csa.KE_KEI____: komasyurui = Komasyurui14.H03_Kei____; break;
                case Word_Csa.GI_GIN____: komasyurui = Komasyurui14.H04_Gin____; break;
                case Word_Csa.KI_KIN____: komasyurui = Komasyurui14.H05_Kin____; break;
                case Word_Csa.KA_KAKU___: komasyurui = Komasyurui14.H08_Kaku___; break;
                case Word_Csa.HI_HISYA__: komasyurui = Komasyurui14.H07_Hisya__; break;
                case Word_Csa.OU_OU_____: komasyurui = Komasyurui14.H06_Gyoku__; break;//おまけ
                case Word_Csa.TO_TOKIN__: komasyurui = Komasyurui14.H11_Tokin__; break;
                case Word_Csa.NY_NARIKYO: komasyurui = Komasyurui14.H12_NariKyo; break;
                case Word_Csa.NK_NARIKEI: komasyurui = Komasyurui14.H13_NariKei; break;
                case Word_Csa.NG_NARIGIN: komasyurui = Komasyurui14.H14_NariGin; break;
                case Word_Csa.UM_UMA____: komasyurui = Komasyurui14.H10_Uma____; break;
                case Word_Csa.RY_RYU____: komasyurui = Komasyurui14.H09_Ryu____; break;
                default: komasyurui = Komasyurui14.H00_Null___; break;//エラー
            }

            //
            // 「成り」をしたのかどうかを、調べます。
            //
            bool promotion = false;
            {
                if (null != ittemae_Sky_orNull && "00" != csa.SourceMasu)
                {
                    // ルート局面ではなく、かつ、打ではないとき。

                    Busstop srcKoma = Util_Sky_KomaQuery.InMasuNow(ittemae_Sky_orNull, srcMasu);
                    Debug.Assert(Busstop.Empty != srcKoma, "元位置の駒を取得できなかった。1");

                    if (!Util_Komasyurui14.IsNari(Conv_Busstop.ToKomasyurui(srcKoma)) && drop)//移動元で「成り」でなかった駒が、移動後に「成駒」になっていた場合。
                    {
                        promotion = true;
                    }
                }
            }

            Playerside pside;
            if ("+"== csa.Sengo)
            {
                pside = Playerside.P1;
            }
            else if ("-"==csa.Sengo)
            {
                pside = Playerside.P2;
            }
            else
            {
                // エラー☆
                pside = Playerside.Empty;
                errorCheck = true;
            }
            

            return Conv_Move.ToMove(
                srcMasu,
                dstMasu,
                komasyurui,
                captured,
                promotion,
                drop,
                pside,
                errorCheck
                );
        }


        public static Move GetErrorMove()
        {
            return (Move)(1 << (int)MoveShift.ErrorCheck);//エラー
        }

        public static string ToLog(Move move)
        {
            if (Move.Empty==move)
            {
                return "EMPTY";
            }

            StringBuilder sb = new StringBuilder();

            int v = (int)move;              // バリュー

            // TODO: エラーチェック
            int errorCheck;
            {
                int m = (int)MoveMask.ErrorCheck;  // マスク
                int s = (int)MoveShift.ErrorCheck;   // シフト
                errorCheck = (v & m) >> s;
            }
            if (0 != errorCheck)
            {
                sb.Append("符号エラー");
                return sb.ToString();
            }


            //────────────────────────────────────────────────────────────────────────────────
            // 組み立てフェーズ
            //────────────────────────────────────────────────────────────────────────────────

            // 手番
            Playerside playersideB = Conv_Move.ToPlayerside(move);
            sb.Append("pside="+ Conv_Playerside.ToLog_Kanji(playersideB) );

            bool drop = Conv_Move.ToDrop(move);


            // 自
            SyElement srcMasuB;
            if (drop)
            {
                sb.Append(" 打");
                // 打のときは、とりあえず、元位置を将棋盤以外にしたい。
                if (Playerside.P1 == playersideB)
                {
                    srcMasuB = Masu_Honshogi.Masus_All[Masu_Honshogi.nsen01];
                }
                else
                {
                    srcMasuB = Masu_Honshogi.Masus_All[Masu_Honshogi.ngo01];
                }
            }
            else
            {
                srcMasuB = Conv_Move.ToSrcMasu(move);
                //srcMasuB = Conv_Masu.ToMasu((int)Conv_Move.ToSrcMasu(move));
                sb.Append(" src=");
                sb.Append(Conv_Masu.ToBanjoArabiaAndKanji_FromMasu(srcMasuB));
            }

            // 至
            SyElement dstMasuB = Conv_Move.ToDstMasu(move);
            sb.Append(" dst=");
            sb.Append(Conv_Masu.ToBanjoArabiaAndKanji_FromMasu(dstMasuB));

            // 移動した駒の種類
            Komasyurui14 srcKomasyuruiB = Conv_Move.ToSrcKomasyurui(move);
            sb.Append(" srcKs=");
            sb.Append(Util_Komasyurui14.KanjiIchimoji[(int)srcKomasyuruiB]);
            Komasyurui14 dstKomasyuruiB = Conv_Move.ToDstKomasyurui(move);
            sb.Append(" dstKs=");
            sb.Append(Util_Komasyurui14.KanjiIchimoji[(int)dstKomasyuruiB]);

            // 取った駒の種類
            Komasyurui14 capturedB = Conv_Move.ToCaptured(move);
            sb.Append(" captured=");
            sb.Append(Util_Komasyurui14.KanjiIchimoji[(int)capturedB]);


            return sb.ToString();
        }

    }
}
