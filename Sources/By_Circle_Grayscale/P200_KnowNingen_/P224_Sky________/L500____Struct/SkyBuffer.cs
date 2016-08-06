using Grayscale.P211_WordShogi__.L500____Word;
using Grayscale.P218_Starlight__.L___500_Struct;
using Grayscale.P223_TedokuHisto.L___250_Struct;
using Grayscale.P223_TedokuHisto.L250____Struct;
using Grayscale.P224_Sky________.L___500_Struct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.P219_Move_______.L___500_Struct;

namespace Grayscale.P224_Sky________.L500____Struct
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 天空
    /// ************************************************************************************************************************
    /// 
    /// 局面のことです。
    /// 
    /// </summary>
    public class SkyBuffer : Sky
    {
        #region プロパティー

        public TedokuHistory TedokuHistory { get { return this.tedokuHistory; } }
        private TedokuHistory tedokuHistory;

        /// <summary>
        /// TODO:
        /// </summary>
        public Playerside KaisiPside { get { return this.kaisiPside; } }
        public void SetKaisiPside(Playerside pside)
        {
            this.kaisiPside = pside;
        }
        private Playerside kaisiPside;

        /// <summary>
        /// 何手目済みか。初期局面を 0 とする。
        /// </summary>
        public int Temezumi { get { return this.temezumi; } }
        public void SetTemezumi(int temezumi)
        {
            this.temezumi = temezumi;
        }
        private int temezumi;

        /// <summary>
        /// 盤面なので、動かないもの（駒）の位置のリストだぜ☆（＾～＾）駒しかないはずなので、４０個のはずだぜ☆（＾～＾）
        /// </summary>
        public List<Busstop> Busstops
        {
            get
            {
                return this.m_busstops_;
            }
        }
        private List<Busstop> m_busstops_;

        #endregion


        public Sky Clone()
        {
            return new SkyBuffer(this);
        }


        public int Count
        {
            get
            {
                return this.Busstops.Count;
            }
        }


        public void Clear()
        {
            this.m_busstops_.Clear();
        }

        /// <summary>
        /// 駒台に戻すとき
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="finger"></param>
        /// <param name="busstop"></param>
        public void PutOverwriteOrAdd_Busstop(Finger finger, Busstop busstop)
        {
            if(this.m_busstops_.Count==(int)finger)
            {
                // オブジェクトを追加します。
                this.Busstops.Add(busstop);
            }
            else if (this.m_busstops_.Count+1 <= (int)finger)
            {
                // エラー
                Debug.Assert((int)finger < this.Busstops.Count, "要素の個数より2大きいインデックスを指定しました。 インデックス[" + (int)finger + "]　要素の個数[" + this.Busstops.Count + "]");

                string message = this.GetType().Name + "#SetStarPos：　リストの要素より2多いインデックスを指定されましたので、追加しません。starIndex=[" + finger + "] / this.stars.Count=[" + this.m_busstops_.Count + "]";
                //LarabeLogger.GetInstance().WriteLineError(LarabeLoggerList.ERROR, message);
                throw new Exception(message);
            }
            else
            {
                this.Busstops[(int)finger] = busstop;
            }
        }





        /// <summary>
        /// 棋譜を新規作成するときに使うコンストラクター。
        /// </summary>
        public SkyBuffer(Playerside kaisiPside, int temezumi)
        {
            switch(kaisiPside)
            {
                case Playerside.P1: this.tedokuHistory = TedokuHistoryConst.New_HirateSyokikyokumen_1P(); break;
                case Playerside.P2: this.tedokuHistory = TedokuHistoryConst.New_HirateSyokikyokumen_2P(); break;
                default: break;
            }            

            this.kaisiPside = kaisiPside;
            this.temezumi = temezumi;
            this.m_busstops_ = new List<Busstop>();
        }

        /// <summary>
        /// クローンを作ります。
        /// </summary>
        /// <param name="src"></param>
        public SkyBuffer(Sky src)
        {
            // 手得ヒストリーのクローン
            this.tedokuHistory = TedokuHistoryConst.New_Clone(src.TedokuHistory);

            // 手番のクローン
            this.kaisiPside = src.KaisiPside;
            this.temezumi = src.Temezumi;

            // 星々のクローン
            this.m_busstops_ = new List<Busstop>();
            src.Foreach_Starlights((Finger finger, Busstop light, ref bool toBreak) =>
            {
                this.m_busstops_.Add(light);
            });
        }

        public void AssertFinger(
            Finger finger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            if ((int)finger < this.m_busstops_.Count)
            {

            }
            else
            {
                string message = this.GetType().Name + "#StarIndexOf：　スプライトの数より多いスプライト番号を指定されましたので、取得できません。スプライト番号=[" + finger + "] / スプライトの数=[" + this.m_busstops_.Count + "]\n memberName=" + memberName + "\n sourceFilePath=" + sourceFilePath + "\n sourceLineNumber=" + sourceLineNumber;
                Debug.Fail(message);
                throw new Exception(message);
            }

        }

        public Busstop StarlightIndexOf(
            Finger finger
            /*
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            */
        )
        {
            this.AssertFinger(finger);

            return this.m_busstops_[(int)finger];
        }


        public delegate void DELEGATE_Sky_Foreach(Finger finger, Busstop busstop, ref bool toBreak);
        public void Foreach_Starlights(DELEGATE_Sky_Foreach delegate_Sky_Foreach)
        {
            bool toBreak = false;

            Finger finger = 0;
            foreach (Busstop busstop in this.Busstops)
            {
                delegate_Sky_Foreach(finger, busstop, ref toBreak);

                finger = (int)finger + 1;
                if (toBreak)
                {
                    break;
                }
            }
        }

        
        /// <summary>
        /// ************************************************************************************************************************
        /// 天上のすべての星の光
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="okiba"></param>
        /// <returns></returns>
        public Fingers Fingers_All()
        {
            Fingers fingers = new Fingers();

            this.Foreach_Starlights((Finger finger, Busstop busstop, ref bool toBreak) =>
            {
                fingers.Add(finger);
            });

            return fingers;
        }
    }
}
