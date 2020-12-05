using System;
using Grayscale.A120KifuSfen.B120ConvSujiDan.C500Converter;

namespace Grayscale.A120KifuSfen.B140SfenStruct.C250Struct
{

    /// <summary>
    /// SFENのstartpos。読取専用。
    /// </summary>
    public class ROKyokumen2ForTokenizeImpl : ROKyokumen2ForTokenize
    {
        /// <summary>
        /// 盤上の駒を、文字の配列で。
        /// 持ち駒を、カウントで。
        /// 
        /// [0～80] 盤上
        /// [81～120] 先手駒台
        /// [121～160] 後手駒台
        /// [161～200] 駒袋
        /// </summary>
        public string[] Masu201 { get { return masu201; } }
        private string[] masu201;
        public string AsMasu(int masuHandle)
        {
            return this.masu201[masuHandle];
        }

        public string GetKomaAs(int suji, int dan)
        {
            return this.Masu201[Conv_SujiDan.ToMasu(suji, dan)];
        }

        public void Foreach_Masu201(DELEGATE_Masu201 delegate_method)
        {
            bool toBreak = false;

            int masuHandle = 0;
            foreach (string masuString in this.masu201)
            {
                System.Diagnostics.Debug.Assert(null != masuString, "masuStringがヌル");

                delegate_method(masuHandle, masuString, ref toBreak);
                masuHandle++;

                if (toBreak)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 持駒の枚数。
        /// </summary>
        public int[] MotiSu { get { return this.m_motiSu_; } }
        private int[] m_motiSu_;

        /// <summary>
        /// 駒袋 王
        /// </summary>
        public int UselessK { get { return uselessK; } }
        private int uselessK;

        /// <summary>
        /// 駒袋 飛
        /// </summary>
        public int UselessR { get { return uselessR; } }
        private int uselessR;

        /// <summary>
        /// 駒袋 角
        /// </summary>
        public int UselessB { get { return uselessB; } }
        private int uselessB;

        /// <summary>
        /// 駒袋 金
        /// </summary>
        public int UselessG { get { return uselessG; } }
        private int uselessG;

        /// <summary>
        /// 駒袋 銀
        /// </summary>
        public int UselessS { get { return uselessS; } }
        private int uselessS;

        /// <summary>
        /// 駒袋 桂
        /// </summary>
        public int UselessN { get { return uselessN; } }
        private int uselessN;

        /// <summary>
        /// 駒袋 香
        /// </summary>
        public int UselessL { get { return uselessL; } }
        private int uselessL;

        /// <summary>
        /// 駒袋 歩
        /// </summary>
        public int UselessP { get { return uselessP; } }
        private int uselessP;


        /// <summary>
        /// 先後。
        /// </summary>
        public bool PsideIsBlack { get { return psideIsBlack; } }
        private bool psideIsBlack;

        /// <summary>
        /// 手目済
        /// </summary>
        public int Temezumi { get { return this.temezumi; } }
        private int temezumi;



        public ROKyokumen2ForTokenizeImpl(
            string[] masu201,
            int[] motiSu,
            int fK,//駒袋 王
            int fR,//駒袋 飛
            int fB,//駒袋 角
            int fG,//駒袋 金
            int fS,//駒袋 銀
            int fN,//駒袋 桂
            int fL,//駒袋 香
            int fP,//駒袋 歩
            string bwStr,
            string temezumiStr
            )
        {
            //盤
            this.masu201 = masu201;

            // Array.Copy(motiSu, this.MotiSu, motiSu.Length);
            Array.Copy(motiSu, this.m_motiSu_, motiSu.Length);

            // 駒袋の中に残っている駒の数を数えます。
            this.uselessK = fK;
            this.uselessR = fR;
            this.uselessB = fB;
            this.uselessG = fG;
            this.uselessS = fS;
            this.uselessN = fN;
            this.uselessL = fL;
            this.uselessP = fP;

            //先後
            if (bwStr == "b")
            {
                this.psideIsBlack = true;
            }
            else
            {
                this.psideIsBlack = false;
            }


            //手目済
            int temezumi;
            int.TryParse(temezumiStr, out temezumi);
            this.temezumi = temezumi;
        }


        public RO_Kyokumen1_ForFormat ToKyokumen1()
        {
            RO_Kyokumen1_ForFormat ro_Kyokumen1 = new RO_Kyokumen1_ForFormatImpl();

            for (int suji = 1; suji < 10; suji++)
            {
                for (int dan = 1; dan < 10; dan++)
                {
                    ro_Kyokumen1.Ban[suji, dan] = this.GetKomaAs(suji, dan);
                }
            }

            Array.Copy(this.MotiSu, ro_Kyokumen1.MotiSu, this.MotiSu.Length);

            return ro_Kyokumen1;
        }
    }
}
