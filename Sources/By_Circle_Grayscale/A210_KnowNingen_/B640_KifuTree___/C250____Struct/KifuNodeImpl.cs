using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B600_UtilSky____.C500____Util;
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C___250_Struct;
using Grayscale.A210_KnowNingen_.B620_KyokumHyoka.C250____Struct;
using Grayscale.A210_KnowNingen_.B640_KifuTree___.C___250_Struct;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using System.Text;

namespace Grayscale.A210_KnowNingen_.B640_KifuTree___.C250____Struct
{
    public class KifuNodeImpl : NodeImpl<Move, Sky>, KifuNode
    {

        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="shootingStarlightable"></param>
        /// <param name="Sky"></param>
        public KifuNodeImpl(Move shootingStarlightable, Sky Sky)
            : base(shootingStarlightable, Sky)
        {
            this.NodeEx = new NodeExImpl();
            this.kyHyokaSheet = new KyHyokaSheetImpl();
        }


        #region プロパティー

        public NodeEx NodeEx { get; set; }

        /// <summary>
        /// 局面評価明細。Mutable なので、SkyConst には入れられない。
        /// </summary>
        public KyHyokaSheet KyHyokaSheet_Mutable { get { return this.kyHyokaSheet; } }
        /// <summary>
        /// 枝専用。
        /// </summary>
        /// <param name="branchKyHyoka"></param>
        public void SetBranchKyHyokaSheet(KyHyokaSheet branchKyHyoka)
        {
            this.kyHyokaSheet = branchKyHyoka;
        }
        private KyHyokaSheet kyHyokaSheet;

        #endregion

        /// <summary>
        /// TODO:この局面データを、読み込めないようにします。（使っていないことを確認するため用）
        /// </summary>
        public void Lock_Kyokumendata()
        {
            this.Value = null;
        }

        public bool IsLeaf { get { return 0 == this.Count_ChildNodes; } }



        public bool HasTuginoitte(
            Move key
            )
        {
            return this.ContainsKey_ChildNodes(key);
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜に　次の一手　を追加します。
        /// ************************************************************************************************************************
        /// 
        /// KifuIO を通して使ってください。
        /// 
        /// ①コマ送り用。
        /// ②「成り」フラグの更新用。
        /// ③マウス操作用
        /// 
        /// カレントノードは変更しません。
        /// </summary>
        public void PutTuginoitte_New(
            Node<Move, Sky> newNode
            )
        {
            // 同じ指し手があれば追加してはいけない？
#if DEBUG
            System.Diagnostics.Debug.Assert(
                !this.HasTuginoitte(newNode.Key),
                "指し手[" + Conv_Move.ToSfen(newNode.Key) + "]は既に指されていました。"
                );
#endif
            // SFENをキーに、次ノードを増やします。

            this.PutAdd_ChildNode(newNode.Key, newNode);
            //手番はここでは変更できない。

            newNode.SetParentNode( this);
        }
        /// <summary>
        /// 既存の子要素を上書きします。
        /// </summary>
        /// <param name="existsNode"></param>
        public void PutTuginoitte_Override(
            Node<Move, Sky> existsNode
            )
        {
            // SFENをキーに、次ノードを増やします。
            this.NextNodes[existsNode.Key] = existsNode;
            existsNode.SetParentNode( this);
        }

        public string Json_NextNodes_MultiSky(
            string memo,
            string hint,
            int temezumi_yomiGenTeban_forLog,//読み進めている現在の手目済
            KwLogger errH)
        {
            StringBuilder sb = new StringBuilder();

            this.Foreach_ChildNodes((Move key, Node<Move, Sky> node, ref bool toBreak) =>
            {
                sb.AppendLine(Util_Sky307.Json_1Sky(
                    node.Value,
                    memo + "：" + Conv_Move.ToSfen( key),
                    hint + "_SF解1",
                    temezumi_yomiGenTeban_forLog
                    ));// 局面をテキストで作成
            });

            return sb.ToString();
        }


        ///// <summary>
        ///// この木の、全てのノードを、フォルダーとして作成します。
        ///// </summary>
        ///// <returns></returns>
        //public void CreateAllFolders(string folderpath, int currentDeep, int limitDeep)
        //{
        //    try
        //    {
        //        if (null == this.GetParentNode())
        //        {
        //            // ルートノードはスルー。
        //        }
        //        else
        //        {
        //            folderpath = folderpath + "/" + Conv_SasiteStr_Sfen.ToSasiteStr_Sfen_ForFilename(this.Key);
        //            Directory.CreateDirectory(folderpath);
        //        }

        //        if (currentDeep <= limitDeep)
        //        {
        //            this.Foreach_ChildNodes((string key, Node<Move, Sky> node, ref bool toBreak) =>
        //            {
        //                ((KifuNode)node).CreateAllFolders(
        //                    folderpath,
        //                    currentDeep + 1,
        //                    limitDeep);
        //            });
        //        }
        //    }
        //    catch(PathTooLongException ex)
        //    {
        //        System.Console.WriteLine("パスが長すぎた☆無視して続行☆：" + ex.Message + "\n folderpath=[" + folderpath + "]");
        //    }
        //}

    }
}
