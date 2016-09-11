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
    Node root
    )
        {
            this.properties = new Dictionary<string, object>();
            this.SetCurNode(root);
        }





        #region プロパティ類

        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        public Node CurNode { get { return this.curNode; } }
        public void SetCurNode(Node node) { this.curNode = node; }
        private Node curNode;


        ///// <summary>
        ///// ------------------------------------------------------------------------------------------------------------------------
        ///// 一手目の手番
        ///// ------------------------------------------------------------------------------------------------------------------------
        ///// 
        /////     １手目を指す手番の、先後を指定してください。
        /////
        /////     ルート局面には　平手初期局面　が入っていると想定していますので、
        /////     ２個目のノードが　一手目の局面と想定しています。
        ///// 
        /////     TODO:プロパティに「pside=black」のように「文字列,object」で入れたい。
        ///// 
        ///// </summary>
        //public Object Pside_FirstSky{get;set;}

        /// <summary>
        /// 使い方自由。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetProperty(string key,object value)
        {
            if (this.properties.ContainsKey(key))
            {
                this.properties[key] = value;
            }
            else
            {
                this.properties.Add(key,value);
            }
        }

        /// <summary>
        /// 使い方自由。
        /// </summary>
        public object GetProperty(string key)
        {
            object result;

            if (this.properties.ContainsKey(key))
            {
                result = this.properties[key];
            }
            else
            {
                result = "Unknown kifu property [" + key + "]";
            }

            return result;
        }
        private Dictionary<string,object> properties;

        #endregion








        #region カレント移動系

        public void Move_Previous()
        {
            this.SetCurNode( this.CurNode.GetParentNode());
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
        public Node PopCurrentNode()
        {
            Node deleteeElement = null;

            if (this.CurNode.IsRoot())
            {
                // やってはいけない操作は、例外を返すようにします。
                string message = "ルート局面を削除しようとしました。";
                throw new Exception(message);
            }

            //>>>>> ラスト要素がルートでなかったら

            // 一手前の要素（必ずあるはずです）
            deleteeElement = this.CurNode;
            // 残されたリストの最後の要素の、次リンクを切ります。
            deleteeElement.GetParentNode().Clear_ChildNodes();

            // カレントを、１つ前の要素に替えます。
            this.Move_Previous();//this.CurNode = deleteeElement.GetParentNode();

            return deleteeElement;
        }



        /// <summary>
        /// 本譜だけ。
        /// </summary>
        /// <param name="endNode">葉側のノード。</param>
        /// <param name="delegate_Foreach"></param>
        public void ForeachHonpu1(Node endNode, DELEGATE_Foreach1 delegate_Foreach)
        {
            bool toBreak = false;

            // 本譜（ノードのリスト）
            List<Node> honpu = new List<Node>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            while (null!=endNode)//ルートを含むところまで遡ります。
            {
                honpu.Add(endNode); // リスト作成

                endNode = endNode.GetParentNode();
            }
            honpu.Reverse();

            //
            // 手済みを数えます。
            //
            int temezumi = 0;//初期局面が[0]

            foreach (Node item in honpu)//正順になっています。
            {
                delegate_Foreach(temezumi, item.Key, item.Value, item, ref toBreak);
                if (toBreak)
                {
                    break;
                }

                temezumi++;
            }
        }
        /// <summary>
        /// 本譜だけ。
        /// </summary>
        /// <param name="endNode">葉側のノード。</param>
        /// <param name="delegate_Foreach"></param>
        public void ForeachHonpu2(Node endNode, DELEGATE_Foreach2 delegate_Foreach)
        {
            bool toBreak = false;

            // 本譜（ノードのリスト）
            List<Node> honpu = new List<Node>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            while (null != endNode)//ルートを含むところまで遡ります。
            {
                honpu.Add(endNode); // リスト作成

                endNode = endNode.GetParentNode();
            }
            honpu.Reverse();

            //
            // 手済みを数えます。
            //
            int temezumi = 0;//初期局面が[0]

            foreach (Node item in honpu)//正順になっています。
            {
                delegate_Foreach(temezumi, item.Key, ref toBreak);
                if (toBreak)
                {
                    break;
                }

                temezumi++;
            }
        }
        /// <summary>
        /// 全て。
        /// </summary>
        /// <param name="endNode"></param>
        /// <param name="delegate_Foreach"></param>
        public void ForeachZenpuku(Node startNode, DELEGATE_Foreach1 delegate_Foreach)
        {

            List<Node> list8 = new List<Node>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            int temezumi = 0;//※指定局面が0。
            bool toFinish_ZenpukuTansaku = false;

            this.Recursive_Node_NextNode(
                temezumi, startNode, delegate_Foreach, ref toFinish_ZenpukuTansaku
                );
            if (toFinish_ZenpukuTansaku)
            {
                goto gt_EndMetdhod;
            }

        gt_EndMetdhod:
            ;
        }

        private void Recursive_Node_NextNode(
            int temezumi1,
            
            //Sky position,
            Node node1,
            
            DELEGATE_Foreach1 delegate_Foreach1, ref bool toFinish_ZenpukuTansaku
            )
        {
            bool toBreak1 = false;

            // このノードを、まず報告。
            delegate_Foreach1(temezumi1, node1.Key, node1.Value, node1, ref toBreak1);
            if (toBreak1)
            {
                //この全幅探索を終わらせる指示が出ていた場合
                toFinish_ZenpukuTansaku = true;
                goto gt_EndMetdhod;
            }

            // 次のノード
            node1.Foreach_ChildNodes((Move key2, Node node2, ref bool toBreak2) =>
            {
                bool toFinish_ZenpukuTansaku2 = false;
                this.Recursive_Node_NextNode(
                    temezumi1 + 1, node2, delegate_Foreach1, ref toFinish_ZenpukuTansaku2
                    );
                if (toFinish_ZenpukuTansaku2)//この全幅探索を終わらせる指示が出ていた場合
                {
                    toBreak2 = true;
                    goto gt_EndBlock;
                }

            gt_EndBlock:
                ;
            });

        gt_EndMetdhod:
            ;
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
            while (!this.CurNode.IsRoot())
            {
                this.Move_Previous();
            }

            // ルートの次の手を全クリアーします。
            this.CurNode.Clear_ChildNodes();
        }

        #endregion






        #region ランダムアクセッサ

        public Node GetRoot()
        {
            return (Node)this.NodeAt(0);
        }

        public Node NodeAt(int sitei_temezumi)
        {
            Node found = null;

            this.ForeachHonpu1(this.CurNode, (int temezumi2, Move move, Sky sky, Node node, ref bool toBreak) =>
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


        /// <summary>
        /// この木の、全てのノード数を数えます。
        /// </summary>
        /// <returns></returns>
        public int CountAllNodes()
        {
            int result = 0;

            if(null!=this.GetRoot())
            {
                result = this.GetRoot().CountAllNodes();
            }

            return result;
        }










        /// <summary>
        /// これから追加する予定のノードの先後を診断します。
        /// </summary>
        /// <param name="node"></param>
        public void AssertChildPside(Playerside parentPside, Playerside childPside)
        {
#if DEBUG
            Debug.Assert(
                (parentPside==Playerside.P1 && childPside==Playerside.P2)
                ||
                (parentPside==Playerside.P2 && childPside==Playerside.P1)
                , "親子の先後に、異順序がありました。現手番[" + parentPside + "]　<> 次手番[" + childPside + "]");
#endif
        }


        ///// <summary>
        ///// この木の、全てのノードを、フォルダーとして作成します。
        ///// </summary>
        ///// <returns></returns>
        //public void CreateAllFolders(string folderpath, int limitDeep)
        //{
        //    int currentDeep = 0;

        //    if (null != this.GetRoot() && currentDeep <= limitDeep)
        //    {
        //        ((KifuNode)this.GetRoot()).CreateAllFolders(folderpath, currentDeep+1, limitDeep);
        //    }
        //}

    }


}
