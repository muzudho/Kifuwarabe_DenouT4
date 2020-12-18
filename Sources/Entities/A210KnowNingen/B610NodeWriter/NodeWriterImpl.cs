using System.Diagnostics;
using Grayscale.A210KnowNingen.B130Json.C500Struct;
using Grayscale.A210KnowNingen.B270Sky.C500Struct;
using Grayscale.A210KnowNingen.B280Tree.C500Struct;
using Grayscale.A210KnowNingen.B600UtilSky.C500Util;

namespace Grayscale.A210KnowNingen.B610NodeWriter.C500Writer
{
    public class NodeWriterImpl
    {

        public IJsonVal ToJsonVal(ISky sky1, MoveExImpl node)
        {
            Json_Obj obj = new Json_Obj();

            //ISky sky1 = node.Value as ISky;
            if (null != sky1)
            {
                // TODO: ログが大きくなるので、１行で出力したあとに改行にします。

                Json_Prop prop = new Json_Prop("kyokumen", UtilSky307.ToJsonVal(sky1));
                obj.Add(prop);
            }
            else
            {
                Debug.Fail("this.Value as ISky じゃなかった。");
            }

            return obj;
        }

    }
}
