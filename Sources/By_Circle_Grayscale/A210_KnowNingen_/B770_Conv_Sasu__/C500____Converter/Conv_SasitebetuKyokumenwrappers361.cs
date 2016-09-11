using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B410_Collection_.C500____Struct;
using Grayscale.A060_Application.B520_Syugoron___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;

using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B770_Conv_Sasu__.C500____Converter
{
    public abstract class Conv_SasitebetuSkys361
    {
        /// <summary>
        /// FIXME:使ってない？
        /// 
        /// 変換『「指し手→局面」のコレクション』→『「駒、指し手」のペアのリスト』
        /// </summary>
        public static List<Couple<Finger, SyElement>> NextNodes_ToKamList(
            Sky src_Sky_genzai,
            Node hubNode,
            KwLogger errH
            )
        {
            List<Couple<Finger, SyElement>> kmList = new List<Couple<Finger, SyElement>>();

            // TODO:
            hubNode.Foreach_ChildNodes((Move move, Node nextNode, ref bool toBreak) =>
            {
                SyElement srcMasu = Conv_Move.ToSrcMasu(move);
                SyElement dstMasu = Conv_Move.ToDstMasu(move);

                Finger figKoma = Util_Sky_FingersQuery.InMasuNow_Old(src_Sky_genzai, srcMasu).ToFirst();

                kmList.Add(new Couple<Finger, SyElement>(figKoma, dstMasu));
            });

            return kmList;
        }
    }
}
