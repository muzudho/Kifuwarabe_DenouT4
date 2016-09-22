using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B630_Sennitite__.C___500_Struct;
using Grayscale.A210_KnowNingen_.B630_Sennitite__.C500____Struct;
using System;
using System.Collections.Generic;

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{

    /// <summary>
    /// 棋譜。
    /// </summary>
    public class TreeImpl : Tree
    {
        public TreeImpl(
            KifuNode root
        )
        {
            this.m_curNode_ = root;
        }





        #region プロパティ類

        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        public KifuNode CurNode1 { get { return this.m_curNode_; } }
        public KifuNode CurNode2ok { get { return this.m_curNode_; } }
        public KifuNode SetCurNode(KifuNode node,Sky sky) {
            this.m_curNode_ = node;
            this.m_sky_ = sky;
            return this.m_curNode_;
        }
        public Sky PositionA {
            get { return this.m_sky_; }
        }
        private Sky m_sky_;
        private KifuNode m_curNode_;

        #endregion








        #region カレント移動系

        public void Move_Previous()
        {
            this.m_curNode_ = this.CurNode2ok.GetParentNode();
        }

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// 現在の要素を切り取って返します。なければヌル。
        /// ************************************************************************************************************************
        /// 
        /// カレントは、１手前に戻ります。
        /// 
        /// </summary>
        /// <returns>ルートしかないリストの場合、ヌルを返します。</returns>
        public KifuNode PopCurrentNode()
        {
            KifuNode deleteeElement = null;

            if (this.CurNode2ok.IsRoot())
            {
                // やってはいけない操作は、例外を返すようにします。
                string message = "ルート局面を削除しようとしました。";
                throw new Exception(message);
            }

            //>>>>> ラスト要素がルートでなかったら

            // 一手前の要素（必ずあるはずです）
            deleteeElement = this.CurNode2ok;
            // 残されたリストの最後の要素の、次リンクを切ります。
            deleteeElement.GetParentNode().Children1.ClearAll();

            // カレントを、１つ前の要素に替えます。
            this.Move_Previous();//this.CurNode = deleteeElement.GetParentNode();

            return deleteeElement;
        }













        #region クリアー系

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜を空っぽにします。
        /// ************************************************************************************************************************
        /// 
        /// ルートは残します。
        /// 
        /// </summary>
        public virtual void Clear()
        {
            // ルートまで遡ります。
            while (!this.CurNode2ok.IsRoot())
            {
                this.Move_Previous();
            }

            // ルートの次の手を全クリアーします。
            this.CurNode2ok.Children1.ClearAll();
        }

        #endregion






        #region ランダムアクセッサ

        public KifuNode GetRoot()
        {
            return this.NodeAt(0);
        }

        public KifuNode NodeAt(int sitei_temezumi)
        {
            KifuNode found = null;

            Util_Tree.ForeachHonpu1(this.CurNode2ok, (int temezumi2, Move move, KifuNode node, ref bool toBreak) =>
            {
                if (sitei_temezumi == temezumi2) //新Verは 0 にも対応。
                {
                    found = node;
                    toBreak = true;
                }

            });

            if (null == found)
            {
                string message = "[" + sitei_temezumi + "]の局面ノード6はヌルでした。";
                //LarabeLogger.GetInstance().WriteLineError(LarabeLoggerList.ERROR, message);
                throw new Exception(message);
            }

            return found;
        }

        #endregion
    }
}
