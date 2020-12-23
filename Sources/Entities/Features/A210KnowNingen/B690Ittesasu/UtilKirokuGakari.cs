using System.Text;
using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 記録係
    /// ************************************************************************************************************************
    /// </summary>
    public abstract class UtilKirokuGakari
    {

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜データを元に、符号リスト１(*1)を出力します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…「▲２六歩△８四歩▲７六歩」といった書き方。
        /// 
        /// 
        /// FIXME: 将棋GUII には要るものの、将棋エンジンには要らないはず。
        /// 
        /// </summary>
        /// <param name="fugoList"></param>
        public static string ToJsaFugoListString(
            IPlaying playing,
            //MoveEx curNode_base,
            ITree kifu1,
            string hint)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");

            sb.Append(playing.GetEarthProperty(Word_KifuTree.PropName_Startpos));
            sb.Append(" moves ");

            // 採譜用に、新しい対局を用意します。
            ITree saifuKifu2;//使い捨て☆
            {
                IPosition positionInit = UtilSkyCreator.New_Hirate();//日本の符号読取時
                saifuKifu2 = new TreeImpl(positionInit);
                playing.ClearEarth();

                // 棋譜を空っぽにします。
                Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(saifuKifu2, positionInit);

                playing.SetEarthProperty(
                    Word_KifuTree.PropName_Startpos, "startpos");//平手の初期局面 // FIXME:平手とは限らないのでは？
            }

            MoveEx curNode = saifuKifu2.MoveEx_Current;
            Util_Tree.ForeachHonpu2(
                kifu1,//curNode_base,
                (int temezumi, Move move, ref bool toBreak) =>
            {
                if (0 == temezumi)
                {
                    // 初期局面はスキップします。
                    goto gt_EndLoop;
                }

                //------------------------------
                // 符号の追加（記録係）
                //------------------------------
                IPosition saifu_PositionA = new Position(saifuKifu2.PositionA);


                // 採譜用新ノード
                MoveEx saifu_newChild = new MoveExImpl(move);
                saifu_PositionA.ReversePlayerside();
                saifu_PositionA.SetTemezumi(temezumi);


                // 記録係り用棋譜（採譜）
                // 新しい次ノードを追加。次ノードを、これからカレントとする。
                //----------------------------------------
                // 次ノート追加
                //----------------------------------------
                playing.GetSennititeCounter().CountUp_New(
                    Conv_Sky.ToKyokumenHash(saifu_PositionA),
                    hint + "/AppendChild_And_ChangeCurrentToChild");

                saifuKifu2.MoveEx_SetCurrent(TreeImpl.OnDoCurrentMove(saifu_newChild, saifuKifu2, saifu_PositionA));

                // 後手の符号がまだ含まれていない。
                string jsaFugoStr = ConvMoveStrJsa.ToMoveStrJsa(
                    saifu_newChild.Move,
                    saifuKifu2.Pv_ToList(),
                    saifu_PositionA);
                sb.Append(jsaFugoStr);

            gt_EndLoop:
                ;
            });

            return sb.ToString();
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜データを元に、符号リスト２(*1)を出力します。
        /// ************************************************************************************************************************
        /// 
        ///     *1…「position startpos moves 7g7f 3c3d 2g2f」といった書き方。
        /// 
        /// </summary>
        /// <param name="fugoList"></param>
        public static string ToSfen_PositionCommand(
            IPlaying playing,
            //MoveEx endNode1
            ITree kifu1
            )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("position ");
            sb.Append(playing.GetEarthProperty(Word_KifuTree.PropName_Startpos));
            sb.Append(" moves ");

            // 本譜
            int count = 0;
            Util_Tree.ForeachHonpu2(
                //endNode1,
                kifu1,
                (int temezumi, Move move, ref bool toBreak) =>
            {
                if (0 == temezumi)
                {
                    // 初期局面はスキップします。
                    goto gt_EndLoop;
                }

                sb.Append(ConvMove.ToSfen(move));
                sb.Append(" ");

            gt_EndLoop:
                count++;
            });

            return sb.ToString();
        }

    }
}
