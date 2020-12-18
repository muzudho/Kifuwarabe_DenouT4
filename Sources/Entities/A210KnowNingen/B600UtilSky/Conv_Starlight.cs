using Grayscale.A210KnowNingen.B130Json.C500Struct;
using Grayscale.A210KnowNingen.B180ConvPside.C500Converter;
using Grayscale.A210KnowNingen.B240Move.C500Struct;
using Grayscale.A210KnowNingen.B320ConvWords.C500Converter;
using Grayscale.A210KnowNingen.B670_ConvKyokume.C500Converter;

namespace Grayscale.A210KnowNingen.B600UtilSky.C500Util
{
    public abstract class Conv_Starlight
    {

        public static IJsonVal ToJsonVal(Busstop koma)
        {
            Json_Obj obj = new Json_Obj();

            // プレイヤーサイド
            obj.Add(new Json_Prop("pside", Conv_Playerside.ToSankaku(Conv_Busstop.ToPlayerside(koma))));// ▲△

            // マス  
            obj.Add(new Json_Prop("masu", Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma))));// ▲△

            // 駒の種類。歩、香、桂…。
            obj.Add(new Json_Prop("syurui", Conv_Komasyurui.ToStr_Ichimoji(Conv_Busstop.ToKomasyurui(koma))));// ▲△

            return obj;
        }

    }
}
