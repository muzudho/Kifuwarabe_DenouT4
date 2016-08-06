using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P035_Collection_.L500____Struct;
using Grayscale.P056_Syugoron___.L___250_Struct;
using Grayscale.P213_Komasyurui_.L250____Word;
using Grayscale.P219_Move_______.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P339_ConvKyokume.L500____Converter;
using Grayscale.P353_Conv_SasuEx.L500____Converter;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P360_Conv_Sasu__.L500____Converter
{
    public abstract class Conv_KomabetuMasus
    {
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
                Busstop koma = src_Sky.StarlightIndexOf(key);

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
                Busstop koma = src_Sky.StarlightIndexOf(key);


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
