using Grayscale.P003_Log________.L___500_Struct;
using Grayscale.P224_Sky________.L500____Struct;
using Grayscale.P247_KyokumenWra.L500____Struct;
using Grayscale.P324_KifuTree___.L___250_Struct;
using Grayscale.P324_KifuTree___.L250____Struct;
using Grayscale.P341_Ittesasu___.L510____OperationB;
using Grayscale.P339_ConvKyokume.L500____Converter;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P258_UtilSky258_.L500____UtilSky;
using Grayscale.P219_Move_______.L___500_Struct;

namespace Grayscale.P353_Conv_SasuEx.L500____Converter
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
            SkyConst src_Sky,
            KwErrorHandler errH
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
