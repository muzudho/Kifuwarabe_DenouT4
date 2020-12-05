using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A120KifuSfen.B140SfenStruct.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;
using Grayscale.A210KnowNingen.B190Komasyurui.C250Word;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B420UtilSky258.C500UtilSky;
using Grayscale.A210KnowNingen.B520_SeizaStartp.C500Struct;
using Grayscale.A210KnowNingen.B650PnlTaikyoku.C250Struct;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;
using Grayscale.A210KnowNingen.B690Ittesasu.C500UtilA;
using Grayscale.A210KnowNingen.B740KifuParserA.C500Parser;
using Grayscale.A210KnowNingen.B820KyokuParser.C500Parser;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.A210KnowNingen.B830ConvStartpo.C500Converter
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
            IKifuParserAGenjo genjo,
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
                ISky newSky = startposImporter.ToSky();
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
