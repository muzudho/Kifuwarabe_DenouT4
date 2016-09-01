using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C500____Util;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.B140_SfenStruct_.C___250_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

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
        /// 持ち駒の枚数☆（＾▽＾）
        /// </summary>
        public int[] MotiSu { get { return m_motiSu_; } }
        private int[] m_motiSu_;


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



        public StartposExporterImpl(Sky src_Sky)
        {
            Debug.Assert(src_Sky.Count == 40, "sourceSky.Starlights.Count=[" + src_Sky.Count + "]");//将棋の駒の数

            this.m_motiSu_ = new int[(int)Pieces.Num];
            this.BanObject201 = new Dictionary<int, Busstop>();

            this.ToBanObject201(src_Sky);
        }



        private void ToBanObject201(Sky src_Sky)
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
                        this.m_motiSu_[(int)Pieces.P]++;
                        break;
                    case Komasyurui14.H02_Kyo____:
                        this.m_motiSu_[(int)Pieces.L]++;
                        break;
                    case Komasyurui14.H03_Kei____:
                        this.m_motiSu_[(int)Pieces.N]++;
                        break;
                    case Komasyurui14.H04_Gin____:
                        this.m_motiSu_[(int)Pieces.S]++;
                        break;
                    case Komasyurui14.H05_Kin____:
                        this.m_motiSu_[(int)Pieces.G]++;
                        break;
                    case Komasyurui14.H06_Gyoku__:
                        this.m_motiSu_[(int)Pieces.K]++;
                        break;
                    case Komasyurui14.H07_Hisya__:
                        this.m_motiSu_[(int)Pieces.R]++;
                        break;
                    case Komasyurui14.H08_Kaku___:
                        this.m_motiSu_[(int)Pieces.B]++;
                        break;
                }
            }
            else if (Conv_MasuHandle.OnGoteKomadai(Conv_SyElement.ToMasuNumber(masu)))
            {
                // 後手駒台
                switch (Conv_Busstop.ToKomasyurui(busstop))
                {
                    case Komasyurui14.H01_Fu_____:
                        this.m_motiSu_[(int)Pieces.p]++;
                        break;
                    case Komasyurui14.H02_Kyo____:
                        this.m_motiSu_[(int)Pieces.l]++;
                        break;
                    case Komasyurui14.H03_Kei____:
                        this.m_motiSu_[(int)Pieces.n]++;
                        break;
                    case Komasyurui14.H04_Gin____:
                        this.m_motiSu_[(int)Pieces.s]++;
                        break;
                    case Komasyurui14.H05_Kin____:
                        this.m_motiSu_[(int)Pieces.g]++;
                        break;
                    case Komasyurui14.H06_Gyoku__:
                        this.m_motiSu_[(int)Pieces.k]++;
                        break;
                    case Komasyurui14.H07_Hisya__:
                        this.m_motiSu_[(int)Pieces.r]++;
                        break;
                    case Komasyurui14.H08_Kaku___:
                        this.m_motiSu_[(int)Pieces.b]++;
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
