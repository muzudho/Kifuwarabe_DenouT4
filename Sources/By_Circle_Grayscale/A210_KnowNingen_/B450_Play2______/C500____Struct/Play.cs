using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B200_Masu_______.C500____Util;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B310_Seiza______.C250____Struct;
using Grayscale.A210_KnowNingen_.B310_Seiza______.C500____Util;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C250____UtilFingers;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B440_Utifudume__.C500____Util;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C260____Operator;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.A210_KnowNingen_.B450_Play2______.C500____Struct
{


    public abstract class Play
    {


        /// <summary>
        /// 指定した持ち駒全てについて、基本的な駒の動きを返します。（金は「前、ななめ前、横、下に進める」のような）
        /// （ポテンシャル・ムーブ＝障害物がなかったときの動き）
        /// 
        /// １局面につき、１回実行される。
        /// </summary>
        /// <param name="src_Sky"></param>
        /// <param name="fingers_sirabetaiMOTIkoma"></param>
        /// <param name="motiOkenaiMasus">持ち駒を置けないマス（味方駒、敵駒が置いてあるマス）</param>
        /// <param name="errH_orNull"></param>
        /// <returns></returns>
        public static List_OneAndMulti<Finger, SySet<SyElement>> Translate_Motikoma_ToMove(
            Sky src_Sky,
            Fingers fingers_sirabetaiMOTIkoma,
            SySet<SyElement> masus_mikata_onBanjo,//打ち歩詰めチェック用
            SySet<SyElement> masus_aite_onBanjo,//打ち歩詰めチェック用
            SySet<SyElement> motiOkenaiMasus,
            KwErrorHandler errH_orNull
            )
        {
            // 駒種類別、置こうとする升
            SySet<SyElement>[] aMasus = new SySet<SyElement>[Array_Komasyurui.Items_AllElements.Length];
            // 駒種類別、置こうとする駒（持駒の代表）
            Busstop[] aDaihyo = new Busstop[Array_Komasyurui.Items_AllElements.Length];
            // 駒種類別、置こうとする駒番号
            Finger[] aFigKoma = new Finger[Array_Komasyurui.Items_AllElements.Length];


            Finger[] daihyoArray;// 持駒。駒の種類代表１個
            Util_Fingers_KomasyuruiQuery.Translate_Fingers_ToKomasyuruiBETUFirst(
                src_Sky,
                fingers_sirabetaiMOTIkoma,
                out daihyoArray
                );
            foreach (Finger figDaihyo in daihyoArray)
            {
                if (Fingers.Error_1 != figDaihyo)
                {
                    src_Sky.AssertFinger(figDaihyo);
                    Busstop daihyo = src_Sky.BusstopIndexOf(figDaihyo);
#if DEBUG
                    Debug.Assert(daihyo != Busstop.Empty, "持ち駒の代表がヌル");
#endif
                    // 駒種類別、置こうとする駒
                    aDaihyo[(int)Conv_Busstop.ToKomasyurui( daihyo)] = daihyo;
                    // 駒種類別、置こうとする升
                    aMasus[(int)Conv_Busstop.ToKomasyurui(daihyo)] = Util_Sky_SyugoQuery.KomaKidou_Potential(figDaihyo, src_Sky);
                    // 駒種類別、置こうとする駒番号
                    aFigKoma[(int)Conv_Busstop.ToKomasyurui(daihyo)] = figDaihyo;
                }
            }


            if (aDaihyo[(int)Komasyurui14.H01_Fu_____] != Busstop.Empty)// 攻め手が、歩を持っているなら
            {
                //----------------------------------------
                // 二歩チェック
                //----------------------------------------
                //
                // 打てない筋の升を除外します。
                //
#if DEBUG
                //if (null != errH_orNull)
                //{
                //    errH_orNull.Logger.WriteLine_AddMemo("----------------------------------------");
                //    errH_orNull.Logger.WriteLine_AddMemo("歩を置きたかった升＝" + Util_SySet.Dump_Elements(aMasus[(int)Komasyurui14.H01_Fu_____]));
                //    errH_orNull.Logger.WriteLine_AddMemo("----------------------------------------");
                //    errH_orNull.Logger.WriteLine_AddMemo("歩の置けない筋チェック（二歩チェック）開始");
                //}
#endif
                // 将棋盤上の自歩一覧。
                Fingers banjoJiFus = Util_Sky_FingersQuery.InOkibaPsideKomasyuruiNow(
                    src_Sky,//指定局面
                    Okiba.ShogiBan,//将棋盤上の
                    Conv_Busstop.ToPlayerside( aDaihyo[(int)Komasyurui14.H01_Fu_____]),//持駒を持っているプレイヤー側の
                    Komasyurui14.H01_Fu_____//歩
                    );
                    
#if DEBUG
                //if (null != errH_orNull)
                //{
                //    errH_orNull.Logger.WriteLine_AddMemo("banjoJiFus.Count=[" + banjoJiFus.Count + "]");
                //    foreach (Finger figKoma in banjoJiFus.Items)
                //    {
                //        errH_orNull.Logger.WriteLine_AddMemo("figKoma=[" + (int)figKoma + "]");
                //    }
                //    errH_orNull.Logger.WriteLine_AddMemo("----------------------------------------");
                //}
#endif

                // 筋別、歩のあるなしチェック
                bool[] existsFu_sujibetu = new bool[10];
                foreach (Finger figBanjoJiFu in banjoJiFus.Items)
                {
                    src_Sky.AssertFinger(figBanjoJiFu);
                    Busstop banjoJiFu = src_Sky.BusstopIndexOf(figBanjoJiFu);
                    int suji;//1～9
                    Util_MasuNum.TryBanjoMasuToSuji(Conv_Busstop.ToMasu( banjoJiFu), out suji);
                    existsFu_sujibetu[suji] = true;
                }

                // 9筋 確認します。
                for (int suji = 1; suji < 10; suji++)
                {
                    if (existsFu_sujibetu[suji])
                    {
                        // 二歩になる筋番号の発見☆
#if DEBUG
                        //System.Console.WriteLine("二歩チェック： " + suji + "筋は二歩。");
#endif

                        // 筋一列を、クリアーします。

                        // 駒種類別、置こうとする升
                        SySet<SyElement> arg1 = Masu_Honshogi.BAN_SUJIS[suji];

                        DLGT_SyElement_BynaryOperate arg2 = Util_SyElement_BinaryOperator.Dlgt_Equals_MasuNumber;

                        SySet<SyElement> obj1 = aMasus[(int)Komasyurui14.H01_Fu_____];//FIXME: ここでヌル☆

                        aMasus[(int)Komasyurui14.H01_Fu_____] = obj1.Minus_Closed(arg1, arg2);
                    }
                }
#if DEBUG
                //if (null != errH_orNull)
                //{
                //    errH_orNull.Logger.WriteLine_AddMemo("歩の置けない筋チェック（二歩チェック）終了");
                //    errH_orNull.Logger.WriteLine_AddMemo("----------------------------------------");
                //    errH_orNull.Logger.WriteLine_AddMemo("歩の置ける升＝" + Util_SySet.Dump_Elements(aMasus[(int)Komasyurui14.H01_Fu_____]));
                //    errH_orNull.Logger.WriteLine_AddMemo("----------------------------------------");
                //}
#endif

                //----------------------------------------
                // 打ち歩詰めチェック
                //----------------------------------------
                if (false)
                {
                    Util_Utifudume.Utifudume(
                        src_Sky,
                        masus_mikata_onBanjo,//打ち歩詰めチェック用
                        masus_aite_onBanjo,//打ち歩詰めチェック用
                        aMasus,//駒種類別、置こうとする升
                        errH_orNull
                    );
                }
            }



            //----------------------------------------
            // 集計
            //----------------------------------------
            // 「どの持ち駒を」「どこに置ける」のコレクション。
            List_OneAndMulti<Finger, SySet<SyElement>> result = new List_OneAndMulti<Finger, SySet<SyElement>>();
            foreach (Komasyurui14 ks in Array_Komasyurui.MotiKoma7Syurui)
            {
                // 置こうとする駒があれば
                if (Busstop.Empty != aDaihyo[(int)ks])
                {
                    // 置けない升を引きます。
                    aMasus[(int)ks] = aMasus[(int)ks].Minus_Closed(
                        motiOkenaiMasus, Util_SyElement_BinaryOperator.Dlgt_Equals_MasuNumber);

                    if (!aMasus[(int)ks].IsEmptySet())
                    {
                        // 置ける升があれば登録
                        result.AddNew(
                            aFigKoma[(int)ks],//置こうとする持駒番号
                            aMasus[(int)ks]//置ける升
                            );
                    }
                }
            }
            return result;
        }

    }


}
