using System;
using System.Collections.Generic;
using System.Diagnostics;
using Grayscale.Kifuwaragyoku.Entities.Logging;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public class ShogibanImpl
    {
        public ShogibanImpl()
        {
            this.KaisiPside = Playerside.Empty;
            this.BanjoKomas = new Dictionary<int, Busstop>();
            this.ErrorMessage = new Dictionary<int, string>();
            this.m_motiSu_ = new int[(int)Piece.Num];
            this.m_komabukuroSu_ = new int[(int)PieceType.Num];
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


        public void AddKoma(SyElement masu, Busstop koma, ILogTag logTag)
        {
            Debug.Assert(!this.ContainsBanjoKoma(Conv_Masu.ToMasuHandle(masu)), "既に駒がある枡に、駒を置こうとしています。[" + Conv_Masu.ToMasuHandle(masu) + "]");

            int masuHandle = Conv_Masu.ToMasuHandle(masu);
            if (this.BanjoKomas.ContainsKey(masuHandle))
            {
                // FIXME: エラー☆（＾▽＾）
                string message = "[重複]masu=" + Conv_Masu.ToLog(masu) + " busstop=" + Conv_Busstop.ToLog(koma);
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
                        this.m_motiSu_[(int)Piece.P]++;
                        break;
                    case Komasyurui14.H02_Kyo____:
                        this.m_motiSu_[(int)Piece.L]++;
                        break;
                    case Komasyurui14.H03_Kei____:
                        this.m_motiSu_[(int)Piece.N]++;
                        break;
                    case Komasyurui14.H04_Gin____:
                        this.m_motiSu_[(int)Piece.S]++;
                        break;
                    case Komasyurui14.H05_Kin____:
                        this.m_motiSu_[(int)Piece.G]++;
                        break;
                    case Komasyurui14.H06_Gyoku__:
                        this.m_motiSu_[(int)Piece.K]++;
                        break;
                    case Komasyurui14.H07_Hisya__:
                        this.m_motiSu_[(int)Piece.R]++;
                        break;
                    case Komasyurui14.H08_Kaku___:
                        this.m_motiSu_[(int)Piece.B]++;
                        break;
                }
            }
            else if (Conv_Masu.OnGoteKomadai(Conv_Masu.ToMasuHandle(masu)))
            {
                // 後手駒台
                switch (Conv_Busstop.ToKomasyurui(koma))
                {
                    case Komasyurui14.H01_Fu_____:
                        this.m_motiSu_[(int)Piece.p]++;
                        break;
                    case Komasyurui14.H02_Kyo____:
                        this.m_motiSu_[(int)Piece.l]++;
                        break;
                    case Komasyurui14.H03_Kei____:
                        this.m_motiSu_[(int)Piece.n]++;
                        break;
                    case Komasyurui14.H04_Gin____:
                        this.m_motiSu_[(int)Piece.s]++;
                        break;
                    case Komasyurui14.H05_Kin____:
                        this.m_motiSu_[(int)Piece.g]++;
                        break;
                    case Komasyurui14.H06_Gyoku__:
                        this.m_motiSu_[(int)Piece.k]++;
                        break;
                    case Komasyurui14.H07_Hisya__:
                        this.m_motiSu_[(int)Piece.r]++;
                        break;
                    case Komasyurui14.H08_Kaku___:
                        this.m_motiSu_[(int)Piece.b]++;
                        break;
                }
            }
            else
            {
                // 駒袋
                switch (Conv_Busstop.ToKomasyurui(koma))
                {
                    case Komasyurui14.H01_Fu_____:
                        this.m_komabukuroSu_[(int)PieceType.P]++;
                        break;
                    case Komasyurui14.H02_Kyo____:
                        this.m_komabukuroSu_[(int)PieceType.L]++;
                        break;
                    case Komasyurui14.H03_Kei____:
                        this.m_komabukuroSu_[(int)PieceType.N]++;
                        break;
                    case Komasyurui14.H04_Gin____:
                        this.m_komabukuroSu_[(int)PieceType.S]++;
                        break;
                    case Komasyurui14.H05_Kin____:
                        this.m_komabukuroSu_[(int)PieceType.G]++;
                        break;
                    case Komasyurui14.H06_Gyoku__:
                        this.m_komabukuroSu_[(int)PieceType.K]++;
                        break;
                    case Komasyurui14.H07_Hisya__:
                        this.m_komabukuroSu_[(int)PieceType.R]++;
                        break;
                    case Komasyurui14.H08_Kaku___:
                        this.m_komabukuroSu_[(int)PieceType.B]++;
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
