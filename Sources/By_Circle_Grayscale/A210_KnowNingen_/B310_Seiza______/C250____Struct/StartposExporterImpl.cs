﻿using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.P339_ConvKyokume.C500____Converter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.A210_KnowNingen_.B310_Seiza______.C250____Struct
{
    /// <summary>
    /// 将棋盤上の情報を数えます。
    /// </summary>
    public class StartposExporterImpl
    {

        /// <summary>
        /// 盤上の駒。
        /// </summary>
        public Dictionary<int, Busstop> BanObject201;



        /// <summary>
        /// 先後。
        /// </summary>
        public Playerside KaisiPside { get{return this.kaisiPside;} } private Playerside kaisiPside;

        /// <summary>
        /// ▲王
        /// </summary>
        public int Moti1K { get { return moti1K; } } private int moti1K;


        /// <summary>
        /// ▲飛
        /// </summary>
        public int Moti1R { get { return moti1R; } } private int moti1R;


        /// <summary>
        /// ▲角
        /// </summary>
        public int Moti1B { get { return moti1B; } } private int moti1B;


        /// <summary>
        /// ▲金
        /// </summary>
        public int Moti1G { get { return moti1G; } } private int moti1G;


        /// <summary>
        /// ▲銀
        /// </summary>
        public int Moti1S { get { return moti1S; } } private int moti1S;


        /// <summary>
        /// ▲桂
        /// </summary>
        public int Moti1N { get { return moti1N; } } private int moti1N;


        /// <summary>
        /// ▲香
        /// </summary>
        public int Moti1L { get { return moti1L; } } private int moti1L;


        /// <summary>
        /// ▲歩
        /// </summary>
        public int Moti1P { get { return moti1P; } } private int moti1P;


        /// <summary>
        /// △王
        /// </summary>
        public int Moti2k { get { return moti2k; } } private int moti2k;


        /// <summary>
        /// △飛
        /// </summary>
        public int Moti2r { get { return moti2r; } } private int moti2r;


        /// <summary>
        /// △角
        /// </summary>
        public int Moti2b { get { return moti2b; } } private int moti2b;


        /// <summary>
        /// △金
        /// </summary>
        public int Moti2g { get { return moti2g; } } private int moti2g;


        /// <summary>
        /// △銀
        /// </summary>
        public int Moti2s { get { return moti2s; } } private int moti2s;


        /// <summary>
        /// △桂
        /// </summary>
        public int Moti2n { get { return moti2n; } } private int moti2n;


        /// <summary>
        /// △香
        /// </summary>
        public int Moti2l { get { return moti2l; } } private int moti2l;


        /// <summary>
        /// △歩
        /// </summary>
        public int Moti2p { get { return moti2p; } } private int moti2p;


        /// <summary>
        /// 駒袋 王
        /// </summary>
        public int FukuroK { get { return fukuroK; } } private int fukuroK;

        /// <summary>
        /// 駒袋 飛
        /// </summary>
        public int FukuroR { get { return fukuroR; } } private int fukuroR;

        /// <summary>
        /// 駒袋 角
        /// </summary>
        public int FukuroB { get { return fukuroB; } } private int fukuroB;

        /// <summary>
        /// 駒袋 金
        /// </summary>
        public int FukuroG { get { return fukuroG; } } private int fukuroG;

        /// <summary>
        /// 駒袋 銀
        /// </summary>
        public int FukuroS { get { return fukuroS; } } private int fukuroS;

        /// <summary>
        /// 駒袋 桂
        /// </summary>
        public int FukuroN { get { return fukuroN; } } private int fukuroN;

        /// <summary>
        /// 駒袋 香
        /// </summary>
        public int FukuroL { get { return fukuroL; } } private int fukuroL;

        /// <summary>
        /// 駒袋 歩
        /// </summary>
        public int FukuroP { get { return fukuroP; } } private int fukuroP;



        public StartposExporterImpl(SkyConst src_Sky)
        {
            Debug.Assert(src_Sky.Count == 40, "sourceSky.Starlights.Count=[" + src_Sky.Count + "]");//将棋の駒の数

            this.BanObject201 = new Dictionary<int, Busstop>();

            this.ToBanObject201(src_Sky);
        }



        private void ToBanObject201(SkyConst src_Sky)
        {
            this.kaisiPside = src_Sky.KaisiPside;// TODO:

            //Util_Sky.Assert_Honshogi(src_Sky);


            // 将棋の駒４０個の場所を確認します。
            foreach (Finger finger in src_Sky.Fingers_All().Items)
            {
                src_Sky.AssertFinger(finger);
                Busstop komaKs = src_Sky.BusstopIndexOf(finger);

                Debug.Assert(Conv_MasuHandle.OnAll(Conv_SyElement.ToMasuNumber(Conv_Busstop.ToMasu( komaKs))), "(int)koma.Masu=[" + Conv_SyElement.ToMasuNumber(Conv_Busstop.ToMasu( komaKs)) + "]");//升番号

                this.AddKoma(
                    Conv_Busstop.ToMasu( komaKs),
                    komaKs
                );
            }
        }



        private void AddKoma(SyElement masu, Busstop busstop)
        {

            Debug.Assert(!this.BanObject201.ContainsKey(Conv_SyElement.ToMasuNumber(masu)), "既に駒がある枡に、駒を置こうとしています。[" + Conv_SyElement.ToMasuNumber(masu) + "]");


            this.BanObject201.Add(Conv_SyElement.ToMasuNumber(masu), busstop);

            if (Conv_MasuHandle.OnShogiban(Conv_SyElement.ToMasuNumber(masu)))
            {
                // 盤上

                // 特にカウントはなし
            }
            else if (Conv_MasuHandle.OnSenteKomadai(Conv_SyElement.ToMasuNumber(masu)))
            {
                // 先手駒台
                switch (Conv_Busstop.ToKomasyurui(busstop))
                {
                    case Komasyurui14.H01_Fu_____:
                        this.moti1P++;
                        break;
                    case Komasyurui14.H02_Kyo____:
                        this.moti1L++;
                        break;
                    case Komasyurui14.H03_Kei____:
                        this.moti1N++;
                        break;
                    case Komasyurui14.H04_Gin____:
                        this.moti1S++;
                        break;
                    case Komasyurui14.H05_Kin____:
                        this.moti1G++;
                        break;
                    case Komasyurui14.H06_Gyoku__:
                        this.moti1K++;
                        break;
                    case Komasyurui14.H07_Hisya__:
                        this.moti1R++;
                        break;
                    case Komasyurui14.H08_Kaku___:
                        this.moti1B++;
                        break;
                }
            }
            else if (Conv_MasuHandle.OnGoteKomadai(Conv_SyElement.ToMasuNumber(masu)))
            {
                // 後手駒台
                switch (Conv_Busstop.ToKomasyurui(busstop))
                {
                    case Komasyurui14.H01_Fu_____:
                        this.moti2p++;
                        break;
                    case Komasyurui14.H02_Kyo____:
                        this.moti2l++;
                        break;
                    case Komasyurui14.H03_Kei____:
                        this.moti2n++;
                        break;
                    case Komasyurui14.H04_Gin____:
                        this.moti2s++;
                        break;
                    case Komasyurui14.H05_Kin____:
                        this.moti2g++;
                        break;
                    case Komasyurui14.H06_Gyoku__:
                        this.moti2k++;
                        break;
                    case Komasyurui14.H07_Hisya__:
                        this.moti2r++;
                        break;
                    case Komasyurui14.H08_Kaku___:
                        this.moti2b++;
                        break;
                }
            }
            else
            {
                // 駒袋
                switch (Conv_Busstop.ToKomasyurui(busstop))
                {
                    case Komasyurui14.H01_Fu_____:
                        this.fukuroP++;
                        break;
                    case Komasyurui14.H02_Kyo____:
                        this.fukuroL++;
                        break;
                    case Komasyurui14.H03_Kei____:
                        this.fukuroN++;
                        break;
                    case Komasyurui14.H04_Gin____:
                        this.fukuroS++;
                        break;
                    case Komasyurui14.H05_Kin____:
                        this.fukuroG++;
                        break;
                    case Komasyurui14.H06_Gyoku__:
                        this.fukuroK++;
                        break;
                    case Komasyurui14.H07_Hisya__:
                        this.fukuroR++;
                        break;
                    case Komasyurui14.H08_Kaku___:
                        this.fukuroB++;
                        break;
                }
            }
        }


        public string CreateDanString(int leftestMasu)
        {
            StringBuilder sb = new StringBuilder();

            List<Busstop> list = new List<Busstop>();
            for (int masuNumber = leftestMasu; masuNumber >= 0; masuNumber -= 9)
            {
                if (this.BanObject201.ContainsKey(masuNumber))
                {
                    list.Add(this.BanObject201[masuNumber]);
                }
                else
                {
                    list.Add(Busstop.Empty);
                }
            }

            int spaceCount = 0;
            foreach (Busstop koma in list)
            {
                if (koma == Busstop.Empty)
                {
                    spaceCount++;
                }
                else
                {
                    if (0 < spaceCount)
                    {
                        sb.Append(spaceCount.ToString());
                        spaceCount = 0;
                    }

                    // 駒の種類だけだと先手ゴマになってしまう。先後も判定した。
                    switch(Conv_Busstop.ToPlayerside( koma))
                    {
                        case Playerside.P1:
                            sb.Append(Util_Komasyurui14.Sfen1P[(int)Conv_Busstop.ToKomasyurui( koma)]);
                            break;
                        case Playerside.P2:
                            sb.Append(Util_Komasyurui14.Sfen2P[(int)Conv_Busstop.ToKomasyurui(koma)]);
                            break;
                        default:
                            throw new Exception("ない手番");
                    }
                }
            }
            if (0 < spaceCount)
            {
                sb.Append(spaceCount.ToString());
                spaceCount = 0;
            }

            return sb.ToString();
        }


    }
}