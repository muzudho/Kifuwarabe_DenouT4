namespace Grayscale.Kifuwaragyoku.Entities.Features
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
