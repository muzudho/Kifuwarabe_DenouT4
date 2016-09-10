using Grayscale.A210_KnowNingen_.B130_Json_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B130_Json_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B600_UtilSky____.C500____Util;
using System.Diagnostics;

namespace Grayscale.A210_KnowNingen_.B610_NodeWriter_.C500____Writer
{
    public class NodeWriterImpl<T1, T2>
    {

        public Json_Val ToJsonVal(NodeImpl<T1, T2> node)
        {
            Json_Obj obj = new Json_Obj();

            Sky kWrap = node.Value as Sky;
            if (null != kWrap)
            {
                // TODO: ログが大きくなるので、１行で出力したあとに改行にします。

                Json_Prop prop = new Json_Prop("kyokumen", Util_Sky307.ToJsonVal(kWrap));
                obj.Add(prop);
            }
            else
            {
                Debug.Fail("this.Value as Sky じゃなかった。");
            }

            return obj;
        }

    }
}
