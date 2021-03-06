﻿#if DEBUG
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    /// <summary>
    /// ログを取るためのもの。
    /// </summary>
    public class MmLogGenjoImpl : MmLogGenjo
    {

        public KaisetuBoard BrdMove { get; set; }

        public int Temezumi_yomiCur { get { return this.temezumi_yomiCur; } }
        private int temezumi_yomiCur;

        /// <summary>
        /// 読み開始手目済み
        /// </summary>
        public int YomikaisiTemezumi { get { return this.yomikaisiTemezumi; } }
        private int yomikaisiTemezumi;


        public Move Move { get { return this.m_move_; } }
        private Move m_move_;

        public ILogger ErrH { get { return this.logTag; } }
        private ILogger logTag;


        public MmLogGenjoImpl(
            int yomikaisiTemezumi,
            KaisetuBoard brdMove,
            int temezumi_yomiCur,
            Move move,
            ILogger logTag
            )
        {
            this.BrdMove = brdMove;
            this.yomikaisiTemezumi = yomikaisiTemezumi;
            this.temezumi_yomiCur = temezumi_yomiCur;
            this.m_move_ = move;
            this.logTag = logTag;
        }

        public void Log1(Playerside pside_genTeban3)
        {
            // this.BrdMove.Caption = "移動可能_" + ConvMove.Move_To_KsString_ForLog(this.Move, pside_genTeban3);
            this.BrdMove.Temezumi = this.Temezumi_yomiCur;
            this.BrdMove.YomikaisiTemezumi = this.YomikaisiTemezumi;
            this.BrdMove.GenTeban = pside_genTeban3;// 現手番
        }

        public void Log2(
            Playerside tebanSeme,//手番（利きを調べる側）
            Playerside tebanKurau//手番（喰らう側）
        )
        {
            if (Playerside.P1 == tebanSeme)
            {
                this.BrdMove.NounaiSeme = Gkl_NounaiSeme.Sente;
            }
            else if (Playerside.P2 == tebanSeme)
            {
                this.BrdMove.NounaiSeme = Gkl_NounaiSeme.Gote;
            }
        }


        public void Log3(
            IPosition src_Sky,
            Playerside tebanKurau,//手番（喰らう側）
            Playerside tebanSeme,//手番（利きを調べる側）
            Fingers fingers_kurau_IKUSA,//戦駒（喰らう側）
            Fingers fingers_kurau_MOTI,// 持駒（喰らう側）
            Fingers fingers_seme_IKUSA,//戦駒（利きを調べる側）
            Fingers fingers_seme_MOTI// 持駒（利きを調べる側）
        )
        {
            // 攻め手の駒の位置
            KaisetuBoard boardLog_clone = new KaisetuBoard(this.BrdMove);
            foreach (Finger finger in fingers_seme_IKUSA.Items)
            {
                Busstop koma = src_Sky.BusstopIndexOf(finger);

                Gkl_KomaMasu km = new Gkl_KomaMasu(
                    Util_Converter_LogGraphicEx.PsideKs14_ToString(tebanSeme, Conv_Busstop.ToKomasyurui(koma), ""),
                    Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma))
                    );
                boardLog_clone.KomaMasu1.Add(km);
            }

            foreach (Finger finger in fingers_kurau_IKUSA.Items)
            {
                Busstop koma = src_Sky.BusstopIndexOf(finger);

                this.BrdMove.KomaMasu2.Add(new Gkl_KomaMasu(
                    Util_Converter_LogGraphicEx.PsideKs14_ToString(tebanKurau, Conv_Busstop.ToKomasyurui(koma), ""),
                    Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma))
                    ));
            }

            foreach (Finger finger in fingers_seme_MOTI.Items)
            {
                Busstop koma = src_Sky.BusstopIndexOf(finger);

                Gkl_KomaMasu km = new Gkl_KomaMasu(
                    Util_Converter_LogGraphicEx.PsideKs14_ToString(tebanSeme, Conv_Busstop.ToKomasyurui(koma), ""),
                    Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma))
                    );
                this.BrdMove.KomaMasu3.Add(km);
            }

            foreach (Finger finger in fingers_kurau_MOTI.Items)
            {
                Busstop koma = src_Sky.BusstopIndexOf(finger);

                this.BrdMove.KomaMasu4.Add(new Gkl_KomaMasu(
                    Util_Converter_LogGraphicEx.PsideKs14_ToString(tebanKurau, Conv_Busstop.ToKomasyurui(koma), ""),
                    Conv_Masu.ToMasuHandle(Conv_Busstop.ToMasu(koma))
                    ));
            }
            this.BrdMove = boardLog_clone;
        }

        public void Log4(
            IPosition src_Sky,
            Playerside tebanSeme,//手番（利きを調べる側）
            Maps_OneAndOne<Finger, SySet<SyElement>> kmMove_seme_IKUSA
        )
        {
            //// 戦駒の移動可能場所
            //KaisetuBoard boardLog_clone = new KaisetuBoard(this.BrdMove);
            //kmMove_seme_IKUSA.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            //{
            //    Busstop koma = src_Sky.BusstopIndexOf(key);

            //    string komaImg = Util_Converter_LogGraphicEx.PsideKs14_ToString(tebanSeme, Conv_Busstop.ToKomasyurui(koma), "");

            //    foreach (New_Basho masu in value.Elements)
            //    {
            //        boardLog_clone.Masu_theMove.Add((int)masu.MasuNumber);
            //    }
            //});

            //this.BrdMove = boardLog_clone;
        }

    }
}
#endif