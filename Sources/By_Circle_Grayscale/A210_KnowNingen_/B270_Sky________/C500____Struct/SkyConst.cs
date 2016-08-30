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
    /// 読取専用の局面データです。
    /// 
    /// 使い方。
    /// 
    /// SkyConst(Sky src) を使って生成してください。
    /// Skyは他に、SkyBuffer があります。
    /// 
    /// </summary>
    public class SkyConst : Sky
    {
        #region プロパティー

        /// <summary>
        /// TODO:
        /// </summary>
        public Playerside KaisiPside { get { return this.kaisiPside; } }
        private Playerside kaisiPside;

        /// <summary>
        /// 何手目済みか。初期局面を 0 とする。
        /// </summary>
        public int Temezumi { get { return this.temezumi; } }
        private int temezumi;

        /// <summary>
        /// 
        /// </summary>
        private List<Busstop> m_busstops_;

        #endregion


        public Sky Clone()
        {
            return SkyConst.NewInstance(this,
                -1//クローンなので、そのまま。
                );
        }


        public int Count
        {
            get
            {
                return this.m_busstops_.Count;
            }
        }


        ///// <summary>
        ///// 棋譜を新規作成するときに使うコンストラクター。
        ///// </summary>
        //public SkyConst()
        //{
        //    this.PsideIsBlack = true;
        //    this.starlights = new List<Starlight>();
        //}

        public static SkyConst NewInstance(Sky src, int temezumi_orMinus1)
        {
            SkyConst result = new SkyConst(src, false, temezumi_orMinus1, new Finger[] { Fingers.Error_1 }, new Busstop[] { Busstop.Empty },
                // 手得計算
                Komasyurui14.H00_Null___, 0, Masu_Honshogi.Query_Basho(Masu_Honshogi.nError)
                );
            return result;
        }

        public static SkyConst NewInstance_ReversePside(Sky src, int temezumi_orMinus1)
        {
            SkyConst result = new SkyConst(src, true, temezumi_orMinus1, new Finger[]{Fingers.Error_1},new Busstop[]{Busstop.Empty},
                // 手得計算
                Komasyurui14.H00_Null___, 0, Masu_Honshogi.Query_Basho(Masu_Honshogi.nError)
                );
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
        public static SkyConst NewInstance_OverwriteOrAdd_Light(Sky src, int temezumi_orMinus1, Finger finger1, Busstop busstop1)
        {
            SkyConst result = new SkyConst(src, false, temezumi_orMinus1, new Finger[]{finger1},new Busstop[]{busstop1},
                // 手得計算
                Komasyurui14.H00_Null___, 0, Masu_Honshogi.Query_Basho(Masu_Honshogi.nError)
                );
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
        public static SkyConst NewInstance_OverwriteOrAdd_Light(Sky src, int temezumi_orMinus1, Finger finger1, Busstop busstop1,
            // 手得計算
            Komasyurui14 tedokuKeisan_komasyurui, int tedokukeisan_index, SyElement tedokukeisan_sasitamasu
            )
        {
            SkyConst result = new SkyConst(src, false, temezumi_orMinus1, new Finger[] { finger1 }, new Busstop[] { busstop1 },
                // 手得計算
                tedokuKeisan_komasyurui, tedokukeisan_index, tedokukeisan_sasitamasu
                );
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
        public static SkyConst NewInstance_OverwriteOrAdd_Light(Sky src, int temezumi_orMinus1, Finger finger1, Busstop busstops1, Finger finger2, Busstop busstops2)
        {
            SkyConst result = new SkyConst(src, false, temezumi_orMinus1, new Finger[] { finger1, finger2 }, new Busstop[] { busstops1, busstops2 },
                // 手得計算
                Komasyurui14.H00_Null___, 0, Masu_Honshogi.Query_Basho(Masu_Honshogi.nError)
                );
            return result;
        }

        /// <summary>
        /// クローンを作ります。
        /// </summary>
        /// <param name="src"></param>
        private SkyConst(Sky src, bool toReversePlayerside, int update_temezumi_orMinus1, Finger[] finger1, Busstop[] busstops1,
            //
            // 手得計算
            //
            Komasyurui14 tedokuKeisan_komasyurui, int tedokukeisan_index, SyElement tedokukeisan_sasitamasu)
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


        public void Foreach_Busstops(SkyBuffer.DELEGATE_Sky_Foreach delegate_Sky_Foreach)
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


    }
}
