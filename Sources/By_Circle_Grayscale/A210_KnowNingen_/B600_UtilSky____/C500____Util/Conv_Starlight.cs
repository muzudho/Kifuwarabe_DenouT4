using Grayscale.A210_KnowNingen_.B130_Json_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B130_Json_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B180_ConvPside__.C500____Converter;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;

namespace Grayscale.A210_KnowNingen_.B600_UtilSky____.C500____Util
{
    public abstract class Conv_Starlight
    {

        public static Json_Val ToJsonVal(Busstop koma)
        {
            Json_Obj obj = new Json_Obj();

            // プレイヤーサイド
            obj.Add(new Json_Prop("pside", Conv_Playerside.ToSankaku(Conv_Busstop.ToPlayerside( koma))));// ▲△

            // マス  
            obj.Add(new Json_Prop("masu", Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu( koma))));// ▲△

            // 駒の種類。歩、香、桂…。
            obj.Add(new Json_Prop("syurui", Conv_Komasyurui.ToStr_Ichimoji(Conv_Busstop.ToKomasyurui( koma))));// ▲△

            return obj;
        }

    }
}
