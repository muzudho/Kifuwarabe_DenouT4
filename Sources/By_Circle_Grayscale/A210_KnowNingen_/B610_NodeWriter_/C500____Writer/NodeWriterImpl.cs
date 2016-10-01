using Grayscale.A210_KnowNingen_.B130_Json_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B130_Json_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B600_UtilSky____.C500____Util;
using System.Diagnostics;

namespace Grayscale.A210_KnowNingen_.B610_NodeWriter_.C500____Writer
{
    public class NodeWriterImpl
    {

        public Json_Val ToJsonVal(Sky sky1, MoveExImpl node)
        {
            Json_Obj obj = new Json_Obj();

            //Sky sky1 = node.Value as Sky;
            if (null != sky1)
            {
                // TODO: ログが大きくなるので、１行で出力したあとに改行にします。

                Json_Prop prop = new Json_Prop("kyokumen", Util_Sky307.ToJsonVal(sky1));
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
