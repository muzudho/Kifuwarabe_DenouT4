using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A500_ShogiEngine.B130_FeatureVect.C___500_Struct;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;

#if DEBUG || LEARN
using System.Text;
#endif

namespace Grayscale.A500_ShogiEngine.B180_Hyokakansu_.C500____Hyokakansu
{


    public class Hyokakansu_Komawari : HyokakansuAbstract
    {

        public Hyokakansu_Komawari()
            : base(HyokakansuName.N05_Komawari_________)
        {
        }

        /// <summary>
        /// 評価値を返します。先手が有利ならプラス、後手が有利ならマイナス、互角は 0.0 です。
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override float Evaluate(
            Playerside psideA,
            Sky positionA,
            FeatureVector fv,
            KwLogger errH
            )
        {
            float score_p1 = 0.0f;
            float score_p2 = 0.0f;//2Pは、負の数なほどグッドということに注意。



            positionA.Foreach_Busstops((Finger finger, Busstop koma, ref bool toBreak) =>
            {
                // 駒の種類による点数
                float komaScore_temp = fv.Komawari[(int)Conv_Busstop.ToKomasyurui( koma)];

                // 持ち駒は、価値を高めます。（ボーナス）序盤に駒をぽんぽん打つのを防ぐため。
                if(
                    (Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(Conv_Busstop.ToOkiba(koma))
                    )
                {
                    //komaScore_temp *= 1.05f;// 1.05倍だと、相手の桂馬の利きに、桂馬をタダ捨てした。足りてないか。
                    komaScore_temp *= 1.13f;
                    //komaScore_temp *= 1.25f;// 1.25倍だと、金、金、角を打たずに王手されて終わってしまった。ボーナスを付けすぎたか☆
                }


                if (Conv_Busstop.ToPlayerside( koma) == Playerside.P1)
                {
                    score_p1 += komaScore_temp;
                }
                else
                {
                    // 駒割は、他の評価値と違って、
                    // １プレイヤーも、２プレイヤーも正の数になっている。
                    // ２プレイヤーは　符号を反転させること。
                    score_p2 += -komaScore_temp;
                }
            });

            //----------------------------------------
            // 明細項目
            //----------------------------------------
#if DEBUG || LEARN
            string utiwake = "";
            // 明細
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("駒割");
                utiwake = sb.ToString();
            }
#endif

            //
            // ２プレイヤーは　負の数になっている（負の数が多いほど有利）ので、
            // 足すだけでいい。
            //
            return score_p1 + score_p2;
        }
    }
}
