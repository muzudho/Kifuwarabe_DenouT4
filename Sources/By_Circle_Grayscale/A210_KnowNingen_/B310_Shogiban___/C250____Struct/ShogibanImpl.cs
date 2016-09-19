using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B310_Shogiban___.C250____Struct
{
    public class ShogibanImpl
    {
        public ShogibanImpl()
        {
            this.KaisiPside = Playerside.Empty;
            this.BanjoKomas = new Dictionary<int, Busstop>();
            this.ErrorMessage = new Dictionary<int, string>();
            this.m_motiSu_ = new int[(int)Pieces.Num];
            this.m_komabukuroSu_ = new int[(int)PieceTypes.Num];
        }


        /// <summary>
        /// 盤上の駒。
        /// </summary>
        public Dictionary<int, Busstop> BanjoKomas;
        public Dictionary<int, string> ErrorMessage { get; set; }

        /// <summary>
        /// 持ち駒の枚数☆（＾▽＾）
        /// </summary>
        public int[] MotiSu { get { return m_motiSu_; } }
        private int[] m_motiSu_;

        /// <summary>
        /// 駒袋の中の枚数☆（＾▽＾）
        /// </summary>
        public int[] KomabukuroSu { get { return this.m_komabukuroSu_; } }
        private int[] m_komabukuroSu_;

        /// <summary>
        /// 先後。
        /// </summary>
        public Playerside KaisiPside { get; set; }


        public void AddKoma(SyElement masu, Busstop koma, KwLogger errH)
        {
            Debug.Assert(!this.ContainsBanjoKoma(Conv_Masu.ToMasuHandle(masu)), "既に駒がある枡に、駒を置こうとしています。[" + Conv_Masu.ToMasuHandle(masu) + "]");

            try
            {
                int masuHandle = Conv_Masu.ToMasuHandle(masu);
                if (this.BanjoKomas.ContainsKey(masuHandle))
                {
                    // FIXME: エラー☆（＾▽＾）
                    string message = "[重複]masu=" + Conv_Masu.ToLog(masu)+ " busstop="+ Conv_Busstop.ToLog(koma);
                    if (this.ErrorMessage.ContainsKey(masuHandle))
                    {
                        this.ErrorMessage.Add(masuHandle,
                            this.ErrorMessage[masuHandle] + " " +
                            message);
                    }
                    else
                    {
                        this.ErrorMessage.Add(masuHandle, message);
                    }
                }
                else
                {
                    // まだ古い仕様なので、とりあえず駒台と区別せず盤上に追加
                    this.BanjoKomas.Add(masuHandle, koma);
                }
            }
            catch (Exception ex)
            {
                errH.DonimoNaranAkirameta(ex,
                    "将棋盤ログを作っているとき☆（＾▽＾）\n"+
                    " masu="+Conv_Masu.ToLog(masu) +"\n"+
                    " busstop=" + Conv_Busstop.ToLog(koma)
                    );
                throw ex;
            }


            if (Conv_Masu.OnShogiban(Conv_Masu.ToMasuHandle(masu)))
            {
                // 盤上
                // TODO:ほんとはここで追加したい this.BanjoKomas.Add(Conv_Masu.ToMasuHandle(masu), koma);

                // 特にカウントはなし
            }
            else if (Conv_Masu.OnSenteKomadai(Conv_Masu.ToMasuHandle(masu)))
            {
                // 先手駒台
                switch (Conv_Busstop.ToKomasyurui(koma))
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
            else if (Conv_Masu.OnGoteKomadai(Conv_Masu.ToMasuHandle(masu)))
            {
                // 後手駒台
                switch (Conv_Busstop.ToKomasyurui(koma))
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
                switch (Conv_Busstop.ToKomasyurui(koma))
                {
                    case Komasyurui14.H01_Fu_____:
                        this.m_komabukuroSu_[(int)PieceTypes.P]++;
                        break;
                    case Komasyurui14.H02_Kyo____:
                        this.m_komabukuroSu_[(int)PieceTypes.L]++;
                        break;
                    case Komasyurui14.H03_Kei____:
                        this.m_komabukuroSu_[(int)PieceTypes.N]++;
                        break;
                    case Komasyurui14.H04_Gin____:
                        this.m_komabukuroSu_[(int)PieceTypes.S]++;
                        break;
                    case Komasyurui14.H05_Kin____:
                        this.m_komabukuroSu_[(int)PieceTypes.G]++;
                        break;
                    case Komasyurui14.H06_Gyoku__:
                        this.m_komabukuroSu_[(int)PieceTypes.K]++;
                        break;
                    case Komasyurui14.H07_Hisya__:
                        this.m_komabukuroSu_[(int)PieceTypes.R]++;
                        break;
                    case Komasyurui14.H08_Kaku___:
                        this.m_komabukuroSu_[(int)PieceTypes.B]++;
                        break;
                }
            }
        }

        public bool ContainsBanjoKoma(int masuNumber)
        {
            return this.BanjoKomas.ContainsKey(masuNumber);
        }

        public Busstop GetBanjoKomaFromMasu(int masuNumber)
        {
            return this.BanjoKomas[masuNumber];
        }
        public string GetErrorMessage(int masuNumber)
        {
            if (this.ErrorMessage.ContainsKey(masuNumber))
            {
                return this.ErrorMessage[masuNumber];
            }
            return "";
        }

    }
}
