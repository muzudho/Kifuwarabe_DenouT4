using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B310_Shogiban___.C250____Struct;

using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B520_SeizaStartp.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B650_PnlTaikyoku.C___250_Struct;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser;
using Grayscale.A210_KnowNingen_.B820_KyokuParser.C___500_Parser;
using Grayscale.A210_KnowNingen_.B820_KyokuParser.C500____Parser;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B830_ConvStartpo.C500____Converter
{
    public abstract class Conv_StartposImporter
    {
                /// <summary>
        /// 
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="restText"></param>
        /// <param name="startposImporter"></param>
        /// <param name="logTag"></param>
        public static ParsedKyokumen ToParsedKyokumen(
            SkyWrapper_Gui model_Manual,// Gui局面を使用
            StartposImporter startposImporter,
            KifuParserA_Genjo genjo,
            KwLogger errH
            )
        {
            ParsedKyokumen parsedKyokumen = new ParsedKyokumenImpl();

            //------------------------------
            // 初期局面の先後
            //------------------------------
            if (startposImporter.RO_SfenStartpos.PsideIsBlack)
            {
                // 黒は先手。
                parsedKyokumen.FirstPside = Playerside.P1;
            }
            else
            {
                // 白は後手。
                parsedKyokumen.FirstPside = Playerside.P2;
            }

            //------------------------------
            // 駒の配置
            //------------------------------
            {
                Sky newSky = startposImporter.ToSky();
                newSky.SetKaisiPside(parsedKyokumen.FirstPside);
                newSky.SetTemezumi(startposImporter.RO_SfenStartpos.Temezumi);// FIXME: 将棋所だと常に 1 かも？？
                parsedKyokumen.NewMove = Move.Empty;// Conv_Move.GetErrorMove();//ルートなので
                parsedKyokumen.NewSky = newSky;
            }

            //------------------------------
            // 駒袋に表示されている駒を、駒台に表示されるように移します。
            //------------------------------
            {
                //------------------------------
                // 持ち駒 ▲王
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.K])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H06_Gyoku__, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.K], Playerside.P1));
                    //System.C onsole.WriteLine("mK=" + ro_SfenStartpos.Moti1K);
                }

                //------------------------------
                // 持ち駒 ▲飛
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.R])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H07_Hisya__, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.R], Playerside.P1));
                    //System.C onsole.WriteLine("mR=" + ro_SfenStartpos.Moti1R);
                }

                //------------------------------
                // 持ち駒 ▲角
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.B])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H08_Kaku___, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.B], Playerside.P1));
                    //System.C onsole.WriteLine("mB=" + ro_SfenStartpos.Moti1B);
                }

                //------------------------------
                // 持ち駒 ▲金
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.G])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H05_Kin____, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.G], Playerside.P1));
                    //System.C onsole.WriteLine("mG=" + ro_SfenStartpos.Moti1G);
                }

                //------------------------------
                // 持ち駒 ▲銀
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.S])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H04_Gin____, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.S], Playerside.P1));
                    //System.C onsole.WriteLine("mS=" + ro_SfenStartpos.Moti1S);
                }

                //------------------------------
                // 持ち駒 ▲桂
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.N])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H03_Kei____, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.N], Playerside.P1));
                    //System.C onsole.WriteLine("mN=" + ro_SfenStartpos.Moti1N);
                }

                //------------------------------
                // 持ち駒 ▲香
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.L])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H02_Kyo____, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.L], Playerside.P1));
                    //System.C onsole.WriteLine("mL=" + ro_SfenStartpos.Moti1L);
                }

                //------------------------------
                // 持ち駒 ▲歩
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.P])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H01_Fu_____, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.P], Playerside.P1));
                    //System.C onsole.WriteLine("mP=" + ro_SfenStartpos.Moti1P);
                }

                //------------------------------
                // 持ち駒 △王
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.k])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H06_Gyoku__, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.k], Playerside.P2));
                    //System.C onsole.WriteLine("mk=" + ro_SfenStartpos.Moti2k);
                }

                //------------------------------
                // 持ち駒 △飛
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.r])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H07_Hisya__, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.r], Playerside.P2));
                    //System.C onsole.WriteLine("mr=" + ro_SfenStartpos.Moti2r);
                }

                //------------------------------
                // 持ち駒 △角
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.b])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H08_Kaku___, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.b], Playerside.P2));
                    //System.C onsole.WriteLine("mb=" + ro_SfenStartpos.Moti2b);
                }

                //------------------------------
                // 持ち駒 △金
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.g])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H05_Kin____, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.g], Playerside.P2));
                    //System.C onsole.WriteLine("mg=" + ro_SfenStartpos.Moti2g);
                }

                //------------------------------
                // 持ち駒 △銀
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.s])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H04_Gin____, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.s], Playerside.P2));
                    //System.C onsole.WriteLine("ms=" + ro_SfenStartpos.Moti2s);
                }

                //------------------------------
                // 持ち駒 △桂
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.n])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H03_Kei____, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.n], Playerside.P2));
                    //System.C onsole.WriteLine("mn=" + ro_SfenStartpos.Moti2n);
                }

                //------------------------------
                // 持ち駒 △香
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.l])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H02_Kyo____, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.l], Playerside.P2));
                    //System.C onsole.WriteLine("ml=" + ro_SfenStartpos.Moti2l);
                }

                //------------------------------
                // 持ち駒 △歩
                //------------------------------
                if (0 < startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.p])
                {
                    parsedKyokumen.MotiList.Add(new MotiItemImpl(Komasyurui14.H01_Fu_____, startposImporter.RO_SfenStartpos.MotiSu[(int)Pieces.p], Playerside.P2));
                    //System.C onsole.WriteLine("mp=" + ro_SfenStartpos.Moti2p);
                }
            }

            //------------------------------------------------------------------------------------------------------------------------
            // 移動
            //------------------------------------------------------------------------------------------------------------------------
            for (int i = 0; i < parsedKyokumen.MotiList.Count; i++)
            {
                Playerside itaruPside;   //(至)先後
                Okiba itaruOkiba;   //(至)置き場

                if (Playerside.P2 == parsedKyokumen.MotiList[i].Playerside)
                {
                    // 宛：後手駒台
                    itaruPside = Playerside.P2;
                    itaruOkiba = Okiba.Gote_Komadai;
                }
                else
                {
                    // 宛：先手駒台
                    itaruPside = Playerside.P1;
                    itaruOkiba = Okiba.Sente_Komadai;
                }


                //------------------------------
                // 駒を、駒袋から駒台に移動させます。
                //------------------------------
                {
                    parsedKyokumen.Sky = model_Manual.GuiSky;


                    Fingers komas = Util_Sky_FingersQuery.InOkibaKomasyuruiNow(
                        parsedKyokumen.Sky,
                        Okiba.KomaBukuro,
                        parsedKyokumen.MotiList[i].Komasyurui
                    );
                    int moved = 1;
                    foreach (Finger koma in komas.Items)
                    {
                        // 駒台の空いている枡
                        SyElement akiMasu = Util_IttesasuRoutine.GetKomadaiKomabukuroSpace(
                            itaruOkiba,
                            parsedKyokumen.Sky
                        );

                        parsedKyokumen.Sky.PutOverwriteOrAdd_Busstop(
                            koma,
                            Conv_Busstop.ToBusstop(
                                itaruPside,
                                akiMasu,
                                parsedKyokumen.MotiList[i].Komasyurui
                            )
                        );

                        if (parsedKyokumen.MotiList[i].Maisu <= moved)
                        {
                            break;
                        }

                        moved++;
                    }
                }

            }//for


            return parsedKyokumen;
        }

    }
}
