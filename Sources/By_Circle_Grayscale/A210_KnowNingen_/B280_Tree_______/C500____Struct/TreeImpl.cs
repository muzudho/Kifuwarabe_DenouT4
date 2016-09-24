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
            MoveNode root, Sky sky
        )
        {
            this.m_curNode_ = root;
            this.m_sky_ = sky;
            //this.SetCurNode(root, sky);
        }

        #region プロパティ類

        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        public MoveNode CurNode3okok { get { return this.m_curNode_; } }
        /// <summary>
        /// 棋譜を空っぽにします。
        /// 
        /// ルートは残します。
        /// </summary>
        public MoveNode OnClearMove(Sky sky)
        {
            // ルートまで遡ります。
            while (!this.CurNode3okok.IsRoot())
            {
                this.m_curNode_ = this.CurNode3okok.GetParentNode();
            }

            // ルートの次の手を全クリアーします。
            this.CurNode3okok.Children1.ClearAll();

            this.m_sky_ = sky;
            return this.m_curNode_;
        }
        public MoveNode OnDoMove(MoveNode node, Sky sky)
        {
            this.m_curNode_ = node;
            this.m_sky_ = sky;
            return this.m_curNode_;
        }
        public MoveNode OnUndoMove(MoveNode node, Sky sky)
        {
            //一手戻した後処理に必要
            // 現在の要素を切り取って返します。なければヌル。
            // カレントは、１手前に戻ります。
            {
                if (this.CurNode3okok.IsRoot())
                {
                    // やってはいけない操作は、例外を返すようにします。
                    string message = "ルート局面を削除しようとしました。";
                    throw new Exception(message);
                }

                //>>>>> ラスト要素がルートでなかったら

                // 一手前の要素（必ずあるはずです）
                MoveNode deleteeElement = this.CurNode3okok;
                // 残されたリストの最後の要素の、次リンクを切ります。
                deleteeElement.GetParentNode().Children1.ClearAll();

                // カレントを、１つ前の要素に替えます。
                this.m_curNode_ = this.CurNode3okok.GetParentNode();
            }


            this.m_curNode_ = node;
            this.m_sky_ = sky;
            return this.m_curNode_;
        }
        /// <summary>
        /// 局面編集中
        /// </summary>
        /// <param name="node"></param>
        /// <param name="sky"></param>
        /// <returns></returns>
        public MoveNode OnEditMove(MoveNode node, Sky sky)
        {
            this.m_curNode_ = node;
            this.m_sky_ = sky;
            return this.m_curNode_;
        }
        public Sky PositionA {
            get { return this.m_sky_; }
        }
        private Sky m_sky_;
        private MoveNode m_curNode_;

        #endregion
    }
}
