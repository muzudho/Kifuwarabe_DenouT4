using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P035_Collection_.L500____Struct;
using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P211_WordShogi__.L250____Masu;
using Grayscale.P212_ConvPside__.L500____Converter;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P218_Starlight__.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P226_Tree_______.L___500_Struct;
using Grayscale.P238_Seiza______.L250____Struct;
using Grayscale.P238_Seiza______.L500____Util;
using Grayscale.P247_KyokumenWra.L500____Struct;
using Grayscale.P258_UtilSky258_.L500____UtilSky;
using Grayscale.P339_ConvKyokume.L500____Converter;
using Grayscale.P353_Conv_SasuEx.L500____Converter;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P219_Move_______.L___500_Struct;

namespace Grayscale.P360_Conv_Sasu__.L500____Converter
{
    public abstract class Conv_KomabetuMasus
    {
        /*
        /// <summary>
        /// FIXME: 使ってない？
        /// 
        /// 変換「自駒が動ける升」→「自駒が動ける手」
        /// </summary>
        /// <param name="kmDic_Self"></param>
        /// <returns></returns>
        public static Maps_OneAndMulti<Finger, Move> ToKomabetuMove(
            Maps_OneAndOne<Finger, SySet<SyElement>> kmDic_Self,
            Node<Move, KyokumenWrapper> siteiNode_genzai
            )
        {

            Maps_OneAndMulti<Finger, Move> komaMove = new Maps_OneAndMulti<Finger, Move>();

            //
            //
            kmDic_Self.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                foreach (SyElement masuHandle in value.Elements)
                {
                    siteiNode_genzai.Value.KyokumenConst.AssertFinger(key);
                    RO_Star koma = Util_Starlightable.AsKoma(siteiNode_genzai.Value.KyokumenConst.StarlightIndexOf(key).Now);

                    Move move = Conv_SasiteStr_Sfen.ToMove(
                        //key,
                        // 元
                            koma,
                        // 先
                            new RO_Star(
                                koma.Pside,
                                Masu_Honshogi.Masus_All[Conv_SyElement.ToMasuNumber(masuHandle)],
                                koma.Haiyaku//TODO:成るとか考えたい
                            ),

                            Komasyurui14.H00_Null___//取った駒不明
                        );

                    if (komaMove.ContainsKey(key))
                    {
                        // すでに登録されている駒
                        komaMove.AddExists(key, move);
                    }
                    else
                    {
                        // まだ登録されていない駒
                        komaMove.AddNew(key, move);
                    }

                }
            });

            return komaMove;
        }
        */



        public static Dictionary<string, SasuEntry> ToSasitebetuSky1(
            Maps_OneAndOne<Finger, SySet<SyElement>> komabetuSusumuMasus,
            SkyConst src_Sky,
            KwErrorHandler errH
        )
        {
            Dictionary<string, SasuEntry> result_komabetuEntry = new Dictionary<string, SasuEntry>();

            komabetuSusumuMasus.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                src_Sky.AssertFinger(key);
                Busstop koma = src_Sky.StarlightIndexOf(key).Now;

                foreach (SyElement dstMasu in value.Elements)
                {
                    Move move = Conv_Move.ToMove(
                        Conv_Busstop.ToMasu( koma),
                        dstMasu,
                        Conv_Busstop.ToKomasyurui( koma),
                        Komasyurui14.H00_Null___,
                        false,//成らない
                        false,//ドロップしない
                        src_Sky.KaisiPside,
                        false
                        );

                    string sasiteStr = Conv_Move.ToSfen(move);//重複防止用のキー

                    if (!result_komabetuEntry.ContainsKey(sasiteStr))
                    {
                        result_komabetuEntry.Add(
                            sasiteStr,
                            new SasuEntry(
                                move,
                                key,//動かす駒
                                dstMasu,//移動先升
                                false//成りません
                                )
                            );
                    }
                }
            });

            return result_komabetuEntry;
        }


        public static Dictionary<string, SasuEntry> KomabetuMasus_ToSasitebetuSky(
            List_OneAndMulti<Finger, SySet<SyElement>> sMs, SkyConst src_Sky, KwErrorHandler errH)
        {
            Dictionary<string, SasuEntry> sasitebetuEntry = new Dictionary<string, SasuEntry>();


            sMs.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                src_Sky.AssertFinger(key);
                Busstop koma = src_Sky.StarlightIndexOf(key).Now;


                foreach (SyElement dstMasu in value.Elements)
                {
                    Move move = Conv_Move.ToMove(
                        Conv_Busstop.ToMasu( koma),
                        dstMasu,
                        Conv_Busstop.ToKomasyurui( koma),
                        Komasyurui14.H00_Null___,
                        false,//成らない
                        false,//多分打たない
                        src_Sky.KaisiPside,
                        false
                        );

                    string sasiteStr = Conv_Move.ToSfen(move);//重複防止用のキー
                    SasuEntry sasuEntry = new SasuEntry(
                        move,
                        key,//動かす駒
                        dstMasu,//移動先升
                        false//成りません。
                        );
                    if (!sasitebetuEntry.ContainsKey(sasiteStr))
                    {
                        sasitebetuEntry.Add(sasiteStr, sasuEntry);
                    }
                }
            });

            return sasitebetuEntry;
        }

    }
}
