﻿using Grayscale.A210_KnowNingen_.B130_Json_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B130_Json_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct;
using Grayscale.A210_KnowNingen_.B370_KyokumenWra.C500____Struct;
using Grayscale.P307_UtilSky____.C500____Util;
using System.Diagnostics;

namespace Grayscale.P320_NodeWriter_.C500____Writer
{
    public class NodeWriterImpl<T1, T2>
    {

        public Json_Val ToJsonVal(NodeImpl<T1, T2> node)
        {
            Json_Obj obj = new Json_Obj();

            KyokumenWrapper kWrap = node.Value as KyokumenWrapper;
            if (null != kWrap)
            {
                // TODO: ログが大きくなるので、１行で出力したあとに改行にします。

                Json_Prop prop = new Json_Prop("kyokumen", Util_Sky307.ToJsonVal(kWrap.KyokumenConst));
                obj.Add(prop);
            }
            else
            {
                Debug.Fail("this.Value as KyokumenWrapper じゃなかった。");
            }

            return obj;
        }

    }
}
