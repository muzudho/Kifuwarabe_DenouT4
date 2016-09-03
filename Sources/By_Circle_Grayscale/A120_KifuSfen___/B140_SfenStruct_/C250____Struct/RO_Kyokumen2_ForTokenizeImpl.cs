using Grayscale.A120_KifuSfen___.B120_ConvSujiDan.C500____Converter;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using Grayscale.A120_KifuSfen___.B140_SfenStruct_.C___250_Struct;
using System;

namespace Grayscale.A120_KifuSfen___.B140_SfenStruct_.C250____Struct
{

    /// <summary>
    /// SFENのstartpos。読取専用。
    /// </summary>
    public class RO_Kyokumen2_ForTokenizeImpl : RO_Kyokumen2_ForTokenize
    {

        #region プロパティー

        /// <summary>
        /// 盤上の駒を、文字の配列で。
        /// 持ち駒を、カウントで。
        /// 
        /// [0～80] 盤上
        /// [81～120] 先手駒台
        /// [121～160] 後手駒台
        /// [161～200] 駒袋
        /// </summary>
        public string[] Masu201 { get { return masu201; } } private string[] masu201;
        public string AsMasu(int masuHandle)
        {
            return this.masu201[masuHandle];
        }

        public string GetKomaAs(int suji, int dan)
        {
            return this.Masu201[ Conv_SujiDan.ToMasu(suji, dan)];
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


        /// <summary>
        /// 先後。
        /// </summary>
        public bool PsideIsBlack { get { return psideIsBlack; } } private bool psideIsBlack;




        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 手目済
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public int Temezumi { get { return this.temezumi; } }private int temezumi;


        #endregion



        public RO_Kyokumen2_ForTokenizeImpl(
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

            Array.Copy(motiSu, this.MotiSu, motiSu.Length);

            // 駒袋の中に残っている駒の数を数えます。
            this.fukuroK = fK;
            this.fukuroR = fR;
            this.fukuroB = fB;
            this.fukuroG = fG;
            this.fukuroS = fS;
            this.fukuroN = fN;
            this.fukuroL = fL;
            this.fukuroP = fP;

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
                    ro_Kyokumen1.Ban[suji,dan] = this.GetKomaAs(suji, dan);
                }
            }

            Array.Copy(this.MotiSu, ro_Kyokumen1.MotiSu, this.MotiSu.Length);

            return ro_Kyokumen1;
        }
    }
}
