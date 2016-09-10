using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B150_KifuJsa____.C500____Word;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B210_KomanoKidou.C500____Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;

using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B550_JsaFugo____.C250____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B560_JsaFugoWrit.C500____Writer
{


    /// <summary>
    /// J符号作成１５個。
    /// 
    /// ５二金右引　のような文字を作ります。
    /// </summary>
    public abstract class Array_JsaFugoCreator15
    {
        public delegate JsaFugoImpl DELEGATE_CreateJFugo(Move move, Sky kWrap, KwLogger errH);

        public static DELEGATE_CreateJFugo[] ItemMethods
        {
            get
            {
                return Array_JsaFugoCreator15.itemMethods;
            }
        }
        private static DELEGATE_CreateJFugo[] itemMethods;

        static Array_JsaFugoCreator15()
        {
            // 駒種類ハンドルに対応
            Array_JsaFugoCreator15.itemMethods = new DELEGATE_CreateJFugo[]{
                Array_JsaFugoCreator15.CreateNullKoma,// null,//[0]
                Array_JsaFugoCreator15.CreateFu,//[1]
                Array_JsaFugoCreator15.CreateKyo,
                Array_JsaFugoCreator15.CreateKei,
                Array_JsaFugoCreator15.CreateGin,
                Array_JsaFugoCreator15.CreateKin,
                Array_JsaFugoCreator15.CreateOh,
                Array_JsaFugoCreator15.CreateHisya,
                Array_JsaFugoCreator15.CreateKaku,
                Array_JsaFugoCreator15.CreateRyu,
                Array_JsaFugoCreator15.CreateUma,//[10]
                Array_JsaFugoCreator15.CreateTokin,
                Array_JsaFugoCreator15.CreateNariKyo,
                Array_JsaFugoCreator15.CreateNariKei,
                Array_JsaFugoCreator15.CreateNariGin,
                Array_JsaFugoCreator15.CreateErrorKoma,//[15]
            };
        }


        public static JsaFugoImpl CreateNullKoma(Move move, Sky kWrap, KwLogger errH)
        {
            JsaFugoImpl result;

            //************************************************************
            // エラー
            //************************************************************
            MigiHidari migiHidari = MigiHidari.No_Print;
            AgaruHiku agaruHiku = AgaruHiku.No_Print;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);

            //----------
            // TODO: 移動前の駒が成る前かどうか
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;

            result = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return result;
        }

        /// <summary>
        /// 歩のJ符号を作ります。
        /// </summary>
        /// <param name="sasite">移動先、移動元、両方のマス番号</param>
        /// <returns></returns>
        public static JsaFugoImpl CreateFu(Move move, Sky src_Sky, KwLogger errH)
        {
            JsaFugoImpl result;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);
            Playerside pside = Conv_Move.ToPlayerside(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);
            bool drop = Conv_Move.ToDrop(move);


            //************************************************************
            // 歩
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //┌─┬─┬─┐
            //│  │  │  │
            //├─┼─┼─┤
            //│  │至│  │
            //├─┼─┼─┤
            //│  │Ｅ│  │
            //└─┴─┴─┘
            SySet<SyElement> srcE = KomanoKidou.SrcIppo_戻引(pside, dstMasu);

            //----------
            // 棋譜の現局面：競合駒
            //----------


            Fingers kmE; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmE, srcE, src_Sky, pside, errH);

            if (drop)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcE, src_Sky, errH))
            {
                // Ｅにいた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // 歩に右左、引上はありません。
                migiHidari = MigiHidari.No_Print;
                agaruHiku = AgaruHiku.No_Print;
            }
            else
            {
                // どこからか飛んできた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // 歩に右左、引上はありません。
                migiHidari = MigiHidari.No_Print;
                agaruHiku = AgaruHiku.No_Print;
            }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmE)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky_BoolQuery.IsNari(move) && !drop && Util_Sky_BoolQuery.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky_BoolQuery.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            result = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return result;
        }

        public static JsaFugoImpl CreateKyo(Move move, Sky src_Sky, KwLogger errH)
        {
            JsaFugoImpl fugo;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);
            Playerside pside = Conv_Move.ToPlayerside(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);
            bool drop = Conv_Move.ToDrop(move);


            //************************************************************
            // 香
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //  ┌─┬─┬─┐
            //  │  │至│  │
            //  ├─┼─┼─┤
            //  │  │E0│  │
            //  ├─┼─┼─┤
            //  │  │E1│  │
            //  ├─┼─┼─┤
            //  │  │E2│  │
            //  ├─┼─┼─┤
            //  │  │E3│  │
            //  ├─┼─┼─┤
            //  │  │E4│  │
            //  ├─┼─┼─┤
            //  │  │E5│  │
            //  ├─┼─┼─┤
            //  │  │E6│  │
            //  ├─┼─┼─┤
            //  │  │E7│  │
            //  └─┴─┴─┘
            SySet<SyElement> srcE = KomanoKidou.SrcKantu_戻引(pside, dstMasu);

            //----------
            // 競合駒
            //----------


            Fingers kmE; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmE, srcE, src_Sky, pside, errH);

            if (drop)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcE, src_Sky, errH))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： 香に右はありません。
            // 「左」解除： 香に左はありません。
            // 「上」解除： 香に上はありません。
            // 「引」解除： 香に引はありません。
            // 「寄」解除： 香に寄はありません。

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmE)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky_BoolQuery.IsNari(move) && !drop && Util_Sky_BoolQuery.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky_BoolQuery.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateKei(Move move, Sky src_Sky, KwLogger errH)
        {
            JsaFugoImpl fugo;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);
            Playerside pside = Conv_Move.ToPlayerside(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);
            bool drop = Conv_Move.ToDrop(move);


            //************************************************************
            // 桂
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス
            //----------
            //┌─┐　┌─┐
            //│  │　│  │
            //├─┼─┼─┤
            //│　│  │  │
            //├─┼─┼─┤
            //│　│至│  │先手から見た図
            //├─┼─┼─┤
            //│　│  │  │
            //├─┼─┼─┤
            //│Ｊ│　│Ｉ│
            //└─┘　└─┘
            SySet<SyElement> srcI = KomanoKidou.SrcKeimatobi_戻跳(pside, dstMasu);
            SySet<SyElement> srcJ = KomanoKidou.SrcKeimatobi_戻駆(pside, dstMasu);

            //----------
            // 競合駒
            //----------

            Fingers kmI; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmI, srcI, src_Sky, pside, errH);
            Fingers kmJ; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmJ, srcJ, src_Sky, pside, errH);

            if (drop)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcI, src_Sky, errH))
            {
                //----------
                // 移動前はＩだった
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcJ, src_Sky, errH))
            {
                //----------
                // 移動前はＪだった
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ｊに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmJ)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ｉに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmI)) { migiHidari = MigiHidari.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmI, kmJ)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky_BoolQuery.IsNari(move) && !drop && Util_Sky_BoolQuery.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky_BoolQuery.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateGin(Move move, Sky src_Sky, KwLogger errH)
        {
            JsaFugoImpl fugo;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);
            Playerside pside = Conv_Move.ToPlayerside(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);
            bool drop = Conv_Move.ToDrop(move);

            //************************************************************
            // 銀
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(range masu)
            //----------
            //┌─┬─┬─┐
            //│Ｈ│  │Ｂ│
            //├─┼─┼─┤
            //│　│至│  │先手から見た図
            //├─┼─┼─┤
            //│Ｆ│Ｅ│Ｄ│
            //└─┴─┴─┘
            SySet<SyElement> srcB = KomanoKidou.SrcIppo_戻昇(pside, dstMasu);
            SySet<SyElement> srcD = KomanoKidou.SrcIppo_戻沈(pside, dstMasu);
            SySet<SyElement> srcE = KomanoKidou.SrcIppo_戻引(pside, dstMasu);
            SySet<SyElement> srcF = KomanoKidou.SrcIppo_戻降(pside, dstMasu);
            SySet<SyElement> srcH = KomanoKidou.SrcIppo_戻浮(pside, dstMasu);

            //----------
            // 競合駒
            //----------

            Fingers kmB; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmB, srcB, src_Sky, pside, errH);
            Fingers kmD; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmD, srcD, src_Sky, pside, errH);
            Fingers kmE; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmE, srcE, src_Sky, pside, errH);
            Fingers kmF; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmF, srcF, src_Sky, pside, errH);
            Fingers kmH; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmH, srcH, src_Sky, pside, errH);

            if (drop)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcB, src_Sky, errH))
            {
                // 移動前はＢだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcD, src_Sky, errH))
            {
                // 移動前はＤだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcE, src_Sky, errH))
            {
                // 移動前はＥだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Sugu;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcF, src_Sky, errH))
            {
                // 移動前はＦだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcH, src_Sky, errH))
            {
                // 移動前はＨだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： ①Ｅ、Ｆ、Ｈのどこにも競合駒がなければ。
            //              ②Ｈに競合駒がなく引があるなら。
            //              ③Ｅ、Ｆに競合駒がなく上があるなら。
            if (migiHidari == MigiHidari.Migi && (
                Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmE, kmF, kmH)
                || (Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmH) && agaruHiku == AgaruHiku.Hiku)
                || (Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmE, kmF) && agaruHiku == AgaruHiku.Agaru)
                )) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： ①Ｂ、Ｄ、Ｅのどこにも競合駒がなければ。
            //              ②Ｂに競合駒がなく引があるなら。
            //              ③Ｄ、Ｅに競合駒がなく上があるなら。
            if (migiHidari == MigiHidari.Hidari && (
                Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmB, kmD, kmE)
                || (Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmB) && agaruHiku == AgaruHiku.Hiku)
                || (Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmD, kmE) && agaruHiku == AgaruHiku.Agaru)
                )) { migiHidari = MigiHidari.No_Print; }

            // 「直」解除： Ｄ、Ｆのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Sugu && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmD, kmF)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ｂ、Ｈのどこにも競合駒がなければ。また、直があるなら。
            if (agaruHiku == AgaruHiku.Agaru && (Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmB, kmH) || migiHidari == MigiHidari.Sugu)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： ①Ｂ、Ｄ、Ｅ、Ｆのどこにも競合駒がなければ。
            //              ②Ｄ、Ｅ、Ｆ、Ｈのどこにも競合駒がなければ。
            //              ③Ｄに競合駒がなく、右があるなら。
            //              ④Ｆに競合駒がなく、左があるなら。
            if (agaruHiku == AgaruHiku.Hiku &&
                (
                Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmB, kmD, kmE, kmF)
                || Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmD, kmE, kmF, kmH)
                || (Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmD) && migiHidari == MigiHidari.Migi)
                || (Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmF) && migiHidari == MigiHidari.Hidari)
                )) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： 銀は寄れません。

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmB, kmD, kmE, kmF, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky_BoolQuery.IsNari(move) && !drop && Util_Sky_BoolQuery.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky_BoolQuery.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateKin(Move move, Sky kWrap, KwLogger errH)
        {
            JsaFugoImpl fugo;

            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji;

            Array_JsaFugoCreator15.CreateKin_static(move, kWrap, out migiHidari, out agaruHiku, out nari, out daHyoji, errH);

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static void CreateKin_static(
            Move move,//移動先、移動元、両方のマス番号
            Sky src_Sky,
            out MigiHidari migiHidari, out AgaruHiku agaruHiku, out NariNarazu nari, out DaHyoji daHyoji,
            KwLogger errH
            )
        {
            Playerside pside = Conv_Move.ToPlayerside(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);
            Komasyurui14 dstKs = Conv_Move.ToDstKomasyurui(move);
            bool drop = Conv_Move.ToDrop(move);

            //************************************************************
            // △金、△と金、△成香、△成桂、△成銀
            //************************************************************
            daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //┌─┬─┬─┐
            //│  │Ａ│  │
            //├─┼─┼─┤
            //│Ｇ│至│Ｃ│先手から見た図
            //├─┼─┼─┤
            //│Ｆ│Ｅ│Ｄ│
            //└─┴─┴─┘
            SySet<SyElement> srcA = KomanoKidou.SrcIppo_戻上(pside, dstMasu);
            SySet<SyElement> srcC = KomanoKidou.SrcIppo_戻射(pside, dstMasu);
            SySet<SyElement> srcD = KomanoKidou.SrcIppo_戻沈(pside, dstMasu);
            SySet<SyElement> srcE = KomanoKidou.SrcIppo_戻引(pside, dstMasu);
            SySet<SyElement> srcF = KomanoKidou.SrcIppo_戻降(pside, dstMasu);
            SySet<SyElement> srcG = KomanoKidou.SrcIppo_戻滑(pside, dstMasu);

            //----------
            // 競合駒
            //----------

            Fingers kmA; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmA, srcA, src_Sky, pside, errH);
            Fingers kmC; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmC, srcC, src_Sky, pside, errH);
            Fingers kmD; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmD, srcD, src_Sky, pside, errH);
            Fingers kmE; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmE, srcE, src_Sky, pside, errH);
            Fingers kmF; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmF, srcF, src_Sky, pside, errH);
            Fingers kmG; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmG, srcG, src_Sky, pside, errH);


            if (drop)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcA, src_Sky, errH))
            {
                //----------
                // 移動前はＡだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcC, src_Sky, errH))
            {
                //----------
                // 移動前はＣだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcF, src_Sky, errH))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcE, src_Sky, errH))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Sugu;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcD, src_Sky, errH))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcG, src_Sky, errH))
            {
                //----------
                // 移動前はＧだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }


            // 「右」解除： ①Ｅ、Ｆ、Ｇのどちらにも競合駒がなければ。
            //              ②Ｇに競合駒がなく、寄があるなら。
            //              ③上があり、Ｅ、Ｆのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && (
                Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmE, kmF, kmG)
                || (Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmG) && agaruHiku == AgaruHiku.Yoru)
                || (AgaruHiku.Agaru == agaruHiku && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmE, kmF))
                )) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： ①Ｃ、Ｄ、Ｅのどちらにも競合駒がなければ。
            //              ②Ｃに競合駒がなく、寄があるなら。
            //              ③上があり、Ｄ、Ｅのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && (
                Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmC, kmD, kmE)
                || (Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmC) && agaruHiku == AgaruHiku.Yoru)
                || (AgaruHiku.Agaru == agaruHiku && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmD, kmE))
                )) { migiHidari = MigiHidari.No_Print; }

            // 「直」解除： Ｄ、Ｆのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Sugu && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmD, kmF)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： ①Ａ、Ｃ、Ｇのどこにも競合駒がなければ。
            //              ②直があるなら。
            //              ③Ｃに競合駒がなく、右があるなら。
            //              ④Ｇに競合駒がなく、左があるなら。
            if (agaruHiku == AgaruHiku.Agaru &&
                (
                Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmC, kmG)
                || migiHidari == MigiHidari.Sugu
                || Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmC) && migiHidari == MigiHidari.Migi
                || Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmG) && migiHidari == MigiHidari.Hidari
                )
                ) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｄ、Ｅ、Ｆ、Ｇのどこにも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmC, kmD, kmE, kmF, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： ①Ａ、Ｄ、Ｅ、Ｆのどこにも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmD, kmE, kmF)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmC, kmD, kmE, kmF, kmG)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成れません
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;
        }

        public static JsaFugoImpl CreateOh(Move move, Sky copy_Sky, KwLogger errH)
        {
            JsaFugoImpl fugo;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);


            //************************************************************
            // 王
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(range masu)
            //----------
            //┌─┬─┬─┐
            //│Ｈ│Ａ│Ｂ│
            //├─┼─┼─┤
            //│Ｇ│至│Ｃ│先手から見た図
            //├─┼─┼─┤
            //│Ｆ│Ｅ│Ｄ│
            //└─┴─┴─┘

            migiHidari = MigiHidari.No_Print;
            agaruHiku = AgaruHiku.No_Print;

            //----------
            // 王は成れません
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateHisya(Move move, Sky src_Sky, KwLogger errH)
        {
            JsaFugoImpl fugo;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);
            Playerside pside = Conv_Move.ToPlayerside(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);
            bool drop = Conv_Move.ToDrop(move);



            //************************************************************
            // 飛
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //  │  │  │  │  │  │  │  │  │A7│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A6│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A5│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A4│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │  │  │  │  │A3│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A2│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A1│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A0│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │G7│G6│G5│G4│G3│G2│G1│G0│至│C0│C1│C2│C3│C4│C5│C6│C7│
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E0│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E1│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E2│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E3│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │  │  │  │  │E4│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E5│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E6│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E7│  │  │  │  │  │  │  │  │
            //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
            SySet<SyElement> srcA = KomanoKidou.SrcKantu_戻上(pside, dstMasu);
            SySet<SyElement> srcC = KomanoKidou.SrcKantu_戻射(pside, dstMasu);
            SySet<SyElement> srcE = KomanoKidou.SrcKantu_戻引(pside, dstMasu);
            SySet<SyElement> srcG = KomanoKidou.SrcKantu_戻滑(pside, dstMasu);

            //----------
            // 棋譜の現局面：競合駒
            //----------

            Fingers kmA; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmA, srcA, src_Sky, pside, errH);
            Fingers kmC; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmC, srcC, src_Sky, pside, errH);
            Fingers kmE; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmE, srcE, src_Sky, pside, errH);
            Fingers kmG; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmG, srcG, src_Sky, pside, errH);

            if (drop)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcA, src_Sky, errH))
            {
                //----------
                // Ａにいた
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcC, src_Sky, errH))
            {
                //----------
                // Ｃにいた
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcE, src_Sky, errH))
            {
                //----------
                // Ｅにいた
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcG, src_Sky, errH))
            {
                //----------
                // Ｇにいた
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ｇに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmG)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ｃに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmC)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ａ、Ｃ、Ｇに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Agaru && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmC, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｅ、Ｇに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmC, kmE, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： Ａ、Ｅに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA) && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmE)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmC, kmE, kmG)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky_BoolQuery.IsNari(move) && !drop && Util_Sky_BoolQuery.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky_BoolQuery.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateKaku(Move move, Sky src_Sky, KwLogger errH)
        {
            JsaFugoImpl fugo;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);
            Playerside pside = Conv_Move.ToPlayerside(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);
            bool drop = Conv_Move.ToDrop(move);


            //************************************************************
            // 角
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(range masu)
            //----------
            //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //  │H7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │B7│
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │H6│  │  │  │  │  │  │  │  │  │  │  │  │  │B6│  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │H5│  │  │  │  │  │  │  │  │  │  │  │B5│  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │H4│  │  │  │  │  │  │  │  │  │B4│  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │H3│  │  │  │  │  │  │  │B3│  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │H2│  │  │  │  │  │B2│  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │H1│  │  │  │B1│  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │H0│  │B0│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │至│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │F0│  │D0│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │F1│  │  │  │D1│  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │F2│  │  │  │  │  │D2│  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │F3│  │  │  │  │  │  │  │D3│  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │F4│  │  │  │  │  │  │  │  │  │D4│  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │F5│  │  │  │  │  │  │  │  │  │  │  │D5│  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │F6│  │  │  │  │  │  │  │  │  │  │  │  │  │D6│  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │F7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │D7│
            //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
            SySet<SyElement> srcB = KomanoKidou.SrcKantu_戻昇(pside, dstMasu);
            SySet<SyElement> srcD = KomanoKidou.SrcKantu_戻沈(pside, dstMasu);
            SySet<SyElement> srcF = KomanoKidou.SrcKantu_戻降(pside, dstMasu);
            SySet<SyElement> srcH = KomanoKidou.SrcKantu_戻浮(pside, dstMasu);

            //----------
            // 競合駒
            //----------

            Fingers kmB; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmB, srcB, src_Sky, pside, errH);
            Fingers kmD; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmD, srcD, src_Sky, pside, errH);
            Fingers kmF; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmF, srcF, src_Sky, pside, errH);
            Fingers kmH; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmH, srcH, src_Sky, pside, errH);


            if (drop)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcB, src_Sky, errH))
            {
                //----------
                // 移動前はＢだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcD, src_Sky, errH))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcF, src_Sky, errH))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcH, src_Sky, errH))
            {
                //----------
                // 移動前はＨだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ｆ、Ｈに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmF, kmH)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ｂ、Ｄに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmB, kmD)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ｂ、Ｈに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Agaru && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmB, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｄ、Ｆに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmD, kmF)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： 角は寄れません。

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmB, kmD, kmF, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky_BoolQuery.IsNari(move) && !drop && Util_Sky_BoolQuery.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky_BoolQuery.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateRyu(Move move, Sky src_Sky, KwLogger errH)
        {
            JsaFugoImpl fugo;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);
            Playerside pside = Conv_Move.ToPlayerside(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);
            bool drop = Conv_Move.ToDrop(move);

            //************************************************************
            // 竜
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //  │  │  │  │  │  │  │  │  │A7│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A6│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A5│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A4│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │  │  │  │  │A3│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A2│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A1│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │Ｈ│A0│Ｂ│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │G7│G6│G5│G4│G3│G2│G1│G0│至│C0│C1│C2│C3│C4│C5│C6│C7│
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │Ｆ│E0│Ｄ│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E1│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E2│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E3│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │  │  │  │  │E4│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E5│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E6│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E7│  │  │  │  │  │  │  │  │
            //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
            SySet<SyElement> srcA = KomanoKidou.SrcKantu_戻上(pside, dstMasu);
            SySet<SyElement> srcB = KomanoKidou.SrcIppo_戻昇(pside, dstMasu);
            SySet<SyElement> srcC = KomanoKidou.SrcKantu_戻射(pside, dstMasu);
            SySet<SyElement> srcD = KomanoKidou.SrcIppo_戻沈(pside, dstMasu);
            SySet<SyElement> srcE = KomanoKidou.SrcKantu_戻引(pside, dstMasu);
            SySet<SyElement> srcF = KomanoKidou.SrcIppo_戻降(pside, dstMasu);
            SySet<SyElement> srcG = KomanoKidou.SrcKantu_戻滑(pside, dstMasu);
            SySet<SyElement> srcH = KomanoKidou.SrcIppo_戻浮(pside, dstMasu);

            //----------
            // 競合駒
            //----------

            Fingers kmA; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmA, srcA, src_Sky, pside, errH);
            Fingers kmB; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmB, srcB, src_Sky, pside, errH);
            Fingers kmC; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmC, srcC, src_Sky, pside, errH);
            Fingers kmD; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmD, srcD, src_Sky, pside, errH);
            Fingers kmE; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmE, srcE, src_Sky, pside, errH);
            Fingers kmF; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmF, srcF, src_Sky, pside, errH);
            Fingers kmG; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmG, srcG, src_Sky, pside, errH);
            Fingers kmH; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmH, srcH, src_Sky, pside, errH);


            if (drop)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcA, src_Sky, errH))
            {
                //----------
                // 移動前はＡだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcB, src_Sky, errH))
            {
                //----------
                // 移動前はＢだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcC, src_Sky, errH))
            {
                //----------
                // 移動前はＣだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcD, src_Sky, errH))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcE, src_Sky, errH))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcF, src_Sky, errH))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcG, src_Sky, errH))
            {
                //----------
                // 移動前はＧだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcH, src_Sky, errH))
            {
                //----------
                // 移動前はＨだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ａ、Ｅ、Ｆ、Ｇ、Ｈに１つも競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmE, kmF, kmG, kmH)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ａ、Ｂ、Ｃ、Ｄ、Ｅに１つも競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmD, kmE)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ａ、Ｂ、Ｃ、Ｇ、Ｈに１つも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Agaru && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmG, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｄ、Ｅ、Ｆ、Ｇに１つも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmC, kmD, kmE, kmF, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： Ａ、Ｂ、Ｄ、Ｅ、Ｆ、Ｈに１つも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmB, kmD, kmE, kmF, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmD, kmE, kmF, kmG, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成れません
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateUma(Move move, Sky src_Sky, KwLogger errH)
        {
            JsaFugoImpl fugo;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);
            Playerside pside = Conv_Move.ToPlayerside(move);
            SyElement dstMasu = Conv_Move.ToDstMasu(move);
            bool drop = Conv_Move.ToDrop(move);


            //************************************************************
            // 馬
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //  │H7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │B7│
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │H6│  │  │  │  │  │  │  │  │  │  │  │  │  │B6│  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │H5│  │  │  │  │  │  │  │  │  │  │  │B5│  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │H4│  │  │  │  │  │  │  │  │  │B4│  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │H3│  │  │  │  │  │  │  │B3│  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │H2│  │  │  │  │  │B2│  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │H1│  │  │  │B1│  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │H0│Ａ│B0│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │Ｇ│至│Ｃ│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │F0│Ｅ│D0│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │F1│  │  │  │D1│  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │F2│  │  │  │  │  │D2│  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │F3│  │  │  │  │  │  │  │D3│  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │F4│  │  │  │  │  │  │  │  │  │D4│  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │F5│  │  │  │  │  │  │  │  │  │  │  │D5│  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │F6│  │  │  │  │  │  │  │  │  │  │  │  │  │D6│  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │F7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │D7│
            //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
            SySet<SyElement> srcA = KomanoKidou.SrcIppo_戻上(pside, dstMasu);
            SySet<SyElement> srcB = KomanoKidou.SrcKantu_戻昇(pside, dstMasu);
            SySet<SyElement> srcC = KomanoKidou.SrcIppo_戻射(pside, dstMasu);
            SySet<SyElement> srcD = KomanoKidou.SrcKantu_戻沈(pside, dstMasu);
            SySet<SyElement> srcE = KomanoKidou.SrcIppo_戻引(pside, dstMasu);
            SySet<SyElement> srcF = KomanoKidou.SrcKantu_戻降(pside, dstMasu);
            SySet<SyElement> srcG = KomanoKidou.SrcIppo_戻滑(pside, dstMasu);
            SySet<SyElement> srcH = KomanoKidou.SrcKantu_戻浮(pside, dstMasu);

            //----------
            // 競合駒
            //----------


            Fingers kmA; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmA, srcA, src_Sky, pside, errH);
            Fingers kmB; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmB, srcB, src_Sky, pside, errH);
            Fingers kmC; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmC, srcC, src_Sky, pside, errH);
            Fingers kmD; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmD, srcD, src_Sky, pside, errH);
            Fingers kmE; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmE, srcE, src_Sky, pside, errH);
            Fingers kmF; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmF, srcF, src_Sky, pside, errH);
            Fingers kmG; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmG, srcG, src_Sky, pside, errH);
            Fingers kmH; Util_Sky_FingersQueryEx.Fingers_EachSrcNow(out kmH, srcH, src_Sky, pside, errH);

            if (drop)
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcB, src_Sky, errH))
            {
                //----------
                // 移動前はＢだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcD, src_Sky, errH))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcF, src_Sky, errH))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcH, src_Sky, errH))
            {
                //----------
                // 移動前はＨだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcA, src_Sky, errH))
            {
                //----------
                // 移動前はＡだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcC, src_Sky, errH))
            {
                //----------
                // 移動前はＣだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcE, src_Sky, errH))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky_BoolQuery.ExistsIn(move, srcG, src_Sky, errH))
            {
                //----------
                // 移動前はＧだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ａ、Ｅ、Ｆ、Ｇ、Ｈに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmE, kmF, kmG, kmH)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ａ、Ｂ、Ｃ、Ｄ、Ｅに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmD, kmE)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ａ、Ｂ、Ｃ、Ｇ、Ｈに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Agaru && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmG, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｄ、Ｅ、Ｆ、Ｇに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmC, kmD, kmE, kmF, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： Ａ、Ｂ、Ｄ、Ｅ、Ｆ、Ｈに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmB, kmD, kmE, kmF, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky_BoolQuery.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmD, kmE, kmF, kmG, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成れません
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateTokin(Move move, Sky kWrap, KwLogger errH)
        {
            JsaFugoImpl fugo;

            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji;

            Array_JsaFugoCreator15.CreateKin_static(move, kWrap, out migiHidari, out agaruHiku, out nari, out daHyoji, errH);

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateNariKyo(Move move, Sky kWrap, KwLogger errH)
        {
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji;

            Array_JsaFugoCreator15.CreateKin_static(move, kWrap, out migiHidari, out agaruHiku, out nari, out daHyoji, errH);

            JsaFugoImpl fugo;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateNariKei(Move move, Sky kWrap, KwLogger errH)
        {
            JsaFugoImpl fugo;

            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji;

            Array_JsaFugoCreator15.CreateKin_static(move, kWrap, out migiHidari, out agaruHiku, out nari, out daHyoji, errH);

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateNariGin(Move move, Sky kWrap, KwLogger errH)
        {
            JsaFugoImpl fugo;

            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji;

            Array_JsaFugoCreator15.CreateKin_static(move, kWrap, out migiHidari, out agaruHiku, out nari, out daHyoji, errH);

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static JsaFugoImpl CreateErrorKoma(Move move, Sky copy_Sky, KwLogger errH)
        {
            JsaFugoImpl fugo;

            Komasyurui14 srcKs = Conv_Move.ToSrcKomasyurui(move);


            //************************************************************
            // エラー
            //************************************************************
            MigiHidari migiHidari = MigiHidari.No_Print;
            AgaruHiku agaruHiku = AgaruHiku.No_Print;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。



            //----------
            // TODO: 移動前の駒が成る前かどうか
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;

            fugo = new JsaFugoImpl(
                srcKs,//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

    }


}
