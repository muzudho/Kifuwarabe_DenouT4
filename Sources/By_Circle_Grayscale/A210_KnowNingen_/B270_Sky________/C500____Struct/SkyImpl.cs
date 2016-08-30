using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C250____Masu;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B190_Komasyurui_.C250____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 天空
    /// ************************************************************************************************************************
    /// 
    /// 局面データです。
    /// 
    /// 使い方。
    /// 
    /// SkyConst(Sky src) を使って生成してください。
    /// Skyは他に、SkyBuffer があります。
    /// 
    /// </summary>
    public class SkyImpl : Sky
    {
        /// <summary>
        /// 棋譜を新規作成するときに使うコンストラクター。
        /// </summary>
        public SkyImpl(Playerside kaisiPside, int temezumi)
        {
            this.kaisiPside = kaisiPside;
            this.temezumi = temezumi;
            this.m_busstops_ = new List<Busstop>();
        }

        /// <summary>
        /// クローンを作ります。
        /// </summary>
        /// <param name="src"></param>
        public SkyImpl(Sky src)
        {
            // 手番のクローン
            this.kaisiPside = src.KaisiPside;
            this.temezumi = src.Temezumi;

            // 星々のクローン
            this.m_busstops_ = new List<Busstop>();
            src.Foreach_Busstops((Finger finger, Busstop busstop, ref bool toBreak) =>
            {
                this.m_busstops_.Add(busstop);
            });
        }

        /// <summary>
        /// クローンを作ります。
        /// </summary>
        /// <param name="src"></param>
        private SkyImpl(Sky src, bool toReversePlayerside, int update_temezumi_orMinus1, Finger[] finger1, Busstop[] busstops1)
        {
            Debug.Assert(src.Count == 40, "本将棋とみなしてテスト中。sky.Starlights.Count=[" + src.Count + "]");//将棋の駒の数

            // 手番のクローン
            if (toReversePlayerside)
            {
                this.kaisiPside = Conv_Playerside.Reverse(src.KaisiPside);
            }
            else
            {
                this.kaisiPside = src.KaisiPside;
            }

            // 手目済み
            if (-1 == update_temezumi_orMinus1)
            {
                // そのまま
                this.temezumi = src.Temezumi;
            }
            else
            {
                // 上書き更新
                this.temezumi = update_temezumi_orMinus1;
            }

            // 星々のクローン
            this.m_busstops_ = new List<Busstop>();
            src.Foreach_Busstops((Finger finger2, Busstop busstop2, ref bool toBreak2) =>
            {
                this.m_busstops_.Add(busstop2);
            });

            //
            // 追加分があれば。
            //
            for (int i = 0; i < finger1.Length; i++)
            {
                if (finger1[i] != Fingers.Error_1)
                {
                    if (this.m_busstops_.Count == (int)finger1[i])
                    {
                        // オブジェクトを追加します。
                        this.m_busstops_.Add(busstops1[i]);
                    }
                    else if (this.m_busstops_.Count + 1 <= (int)finger1[i])
                    {
                        // エラー
                        Debug.Assert((int)finger1[i] < this.m_busstops_.Count, "要素の個数より2大きいインデックスを指定しました。 インデックス[" + (int)finger1[i] + "]　要素の個数[" + this.m_busstops_.Count + "]");

                        string message = this.GetType().Name + "#SetStarPos：　リストの要素より2多いインデックスを指定されましたので、追加しません。starIndex=[" + finger1[i] + "] / this.stars.Count=[" + this.m_busstops_.Count + "]";
                        //LarabeLogger.GetInstance().WriteLineError(LarabeLoggerList.ERROR, message);
                        throw new Exception(message);
                    }
                    else
                    {
                        this.m_busstops_[(int)finger1[i]] = busstops1[i];
                    }
                }
            }
        }




        #region プロパティー

        /// <summary>
        /// TODO:
        /// </summary>
        public Playerside KaisiPside { get { return this.kaisiPside; } }
        private Playerside kaisiPside;
        public void SetKaisiPside(Playerside pside)
        {
            this.kaisiPside = pside;
        }

        /// <summary>
        /// 何手目済みか。初期局面を 0 とする。
        /// </summary>
        public int Temezumi { get { return this.temezumi; } }
        private int temezumi;
        public void SetTemezumi(int temezumi)
        {
            this.temezumi = temezumi;
        }

        /// <summary>
        /// 盤面なので、動かないもの（駒）の位置のリストだぜ☆（＾～＾）駒しかないはずなので、４０個のはずだぜ☆（＾～＾）
        /// </summary>
        private List<Busstop> m_busstops_;
        public List<Busstop> Busstops
        {
            get
            {
                return this.m_busstops_;
            }
        }

        #endregion


        public int Count
        {
            get
            {
                return this.m_busstops_.Count;
            }
        }

        public static SkyImpl NewInstance(Sky src, int temezumi_orMinus1)
        {
            SkyImpl result = new SkyImpl(src, false, temezumi_orMinus1, new Finger[] { Fingers.Error_1 }, new Busstop[] { Busstop.Empty });
            return result;
        }

        public static SkyImpl NewInstance_ReversePside(Sky src, int temezumi_orMinus1)
        {
            SkyImpl result = new SkyImpl(src, true, temezumi_orMinus1, new Finger[]{Fingers.Error_1},new Busstop[]{Busstop.Empty});
            return result;
        }

        /// <summary>
        /// 駒を１個　更新します。
        /// 
        /// （１）指したとき。戻したとき。
        /// （２）駒の向き変更にも使われる。
        /// </summary>
        /// <param name="src"></param>
        /// <param name="finger1"></param>
        /// <param name="busstop1"></param>
        /// <returns></returns>
        public static SkyImpl NewInstance_OverwriteOrAdd_Light(Sky src, int temezumi_orMinus1, Finger finger1, Busstop busstop1)
        {
            SkyImpl result = new SkyImpl(src, false, temezumi_orMinus1, new Finger[]{finger1},new Busstop[]{busstop1});
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="finger1">指した駒、指されていた駒</param>
        /// <param name="busstops1"></param>
        /// <param name="finger2">取った駒、取っていた駒</param>
        /// <param name="busstops2"></param>
        /// <returns></returns>
        public static SkyImpl NewInstance_OverwriteOrAdd_Light(
            Sky src, int temezumi_orMinus1, Finger finger1, Busstop busstops1, Finger finger2, Busstop busstops2)
        {
            SkyImpl result = new SkyImpl(src, false, temezumi_orMinus1, new Finger[] { finger1, finger2 }, new Busstop[] { busstops1, busstops2 });
            return result;
        }

        public void AssertFinger(
            Finger finger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (0 <= (int)finger && (int)finger < this.m_busstops_.Count)
            {
                return;
            }
            else
            {
                // エラー
                string message = this.GetType().Name + "#StarIndexOf：　スプライト配列の範囲を外れた添え字を指定されましたので、取得できません。スプライト番号=[" + finger + "] / スプライトの数=[" + this.m_busstops_.Count + "]\n memberName=" + memberName + "\n sourceFilePath=" + sourceFilePath + "\n sourceLineNumber=" + sourceLineNumber;
                Debug.Fail(message);
                throw new Exception(message);
            }
        }

        public Busstop BusstopIndexOf(Finger finger)
        {
            this.AssertFinger(finger);

            return this.m_busstops_[(int)finger];
        }


        public delegate void DELEGATE_Sky_Foreach(Finger finger, Busstop busstop, ref bool toBreak);
        public void Foreach_Busstops(SkyImpl.DELEGATE_Sky_Foreach delegate_Sky_Foreach)
        {
            bool toBreak = false;

            Finger finger = 0;
            foreach (Busstop busstop in this.m_busstops_)
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

            this.Foreach_Busstops((Finger finger, Busstop light, ref bool toBreak) =>
            {
                fingers.Add(finger);
            });

            return fingers;
        }





        /// <summary>
        /// 駒台に戻すとき
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="finger"></param>
        /// <param name="busstop"></param>
        public void PutOverwriteOrAdd_Busstop(Finger finger, Busstop busstop)
        {
            if (this.m_busstops_.Count == (int)finger)
            {
                // オブジェクトを追加します。
                this.Busstops.Add(busstop);
            }
            else if (this.m_busstops_.Count + 1 <= (int)finger)
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

    }
}
