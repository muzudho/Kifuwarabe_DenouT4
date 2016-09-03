using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C510____OperationB;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B720_Conv_SasuEx.C500____Converter
{
    public abstract class Conv_SasuEntry
    {

        /// <summary>
        /// SasuEntry→KifuNode
        /// </summary>
        /// <param name="sasuEntry"></param>
        /// <param name="src_Sky"></param>
        /// <param name="errH"></param>
        /// <returns></returns>
        public static KifuNode ToKifuNode(
            Move move,
            Sky src_Sky,
            KwLogger errH
            )
        {
            // 指す前の駒を、盤上のマス目で指定
            Finger figSasumaenoKoma = Util_Sky_FingersQuery.InMasuNow_Old(src_Sky, Conv_Move.ToSrcMasu(move)).ToFirst();

            return new KifuNodeImpl(
                move,
                new KyokumenWrapper(
                Util_Sasu341.Sasu(
                    src_Sky,//指定局面
                    figSasumaenoKoma,//指す駒
                    Conv_Move.ToDstMasu(move),//移動先升
                    Conv_Move.ToPromotion(move),//成るか。
                    errH
            )));
        }
    }
}
