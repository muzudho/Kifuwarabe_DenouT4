using Grayscale.A060Application.B520Syugoron.C250Struct;
using Grayscale.A210KnowNingen.B170WordShogi.C250Masu;
using Grayscale.A210KnowNingen.B170WordShogi.C500Word;

namespace Grayscale.A210KnowNingen.B180ConvPside.C500Converter
{
    public abstract class Conv_Okiba
    {
        public static string ToLog(Okiba okiba)
        {
            switch (okiba)
            {
                case Okiba.Empty: return "未指定";
                case Okiba.Gote_Komadai: return "後手駒台";
                case Okiba.KomaBukuro: return "駒袋";
                case Okiba.Sente_Komadai: return "先手駒台";
                case Okiba.ShogiBan: return "将棋盤";
                default: return "エラー";
            }
        }

        public static Playerside ToPside(Okiba okiba)
        {
            Playerside pside;
            switch (okiba)
            {
                case Okiba.Gote_Komadai:
                    pside = Playerside.P2;
                    break;
                case Okiba.Sente_Komadai:
                    pside = Playerside.P1;
                    break;
                default:
                    pside = Playerside.Empty;
                    break;
            }

            return pside;
        }
        public static Okiba FromPside(Playerside pside)
        {
            Okiba okiba;
            switch (pside)
            {
                case Playerside.P2:
                    okiba = Okiba.Gote_Komadai;
                    break;
                case Playerside.P1:
                    okiba = Okiba.Sente_Komadai;
                    break;
                default:
                    okiba = Okiba.Empty;
                    break;
            }

            return okiba;
        }

        public static SyElement GetFirstMasuFromOkiba(Okiba okiba)
        {
            SyElement firstMasu;

            switch (okiba)
            {
                case Okiba.ShogiBan:
                    firstMasu = Masu_Honshogi.Query_Basho(Masu_Honshogi.nban11_１一);//[0]
                    break;

                case Okiba.Sente_Komadai:
                    firstMasu = Masu_Honshogi.Query_Basho(Masu_Honshogi.nsen01);//[81]
                    break;

                case Okiba.Gote_Komadai:
                    firstMasu = Masu_Honshogi.Query_Basho(Masu_Honshogi.ngo01);//[121]
                    break;

                case Okiba.KomaBukuro:
                    firstMasu = Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro01);//[161];
                    break;

                default:
                    //エラー
                    firstMasu = Masu_Honshogi.Query_Basho(Masu_Honshogi.nError);// -1→[201];
                    break;
            }

            return firstMasu;
        }

    }
}
