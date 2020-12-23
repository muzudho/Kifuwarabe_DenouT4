using System.Diagnostics;
using Grayscale.Kifuwaragyoku.Entities.Positioning;

namespace Grayscale.Kifuwaragyoku.Entities.Features
{
    public class NodeWriterImpl
    {

        public IJsonVal ToJsonVal(IPosition sky1, MoveExImpl node)
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
