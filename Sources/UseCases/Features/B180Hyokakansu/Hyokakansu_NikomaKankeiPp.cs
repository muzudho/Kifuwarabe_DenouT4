using Grayscale.Kifuwaragyoku.Entities.Features;

#if DEBUG || LEARN
#endif

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{


    /// <summary>
    /// 評価計算
    /// 
    /// 二駒関係ＰＰ。
    /// 
    /// 駒割は含めない。
    /// </summary>
    public class Hyokakansu_NikomaKankeiPp : HyokakansuAbstract
    {

        public Hyokakansu_NikomaKankeiPp()
            : base(HyokakansuName.N14_NikomaKankeiPp___)
        {
        }


        /// <summary>
        /// 評価値を返します。先手が有利ならプラス、後手が有利ならマイナス、互角は 0.0 です。
        /// </summary>
        /// <param name="input_node"></param>
        /// <returns></returns>
        public override float Evaluate(
            Playerside psideA,
            ISky positionA,
            FeatureVector fv
            )
        {
            float out_score = 0.0f;            // -999～999(*bairitu) が 40×40個ほど足し合わせた数になるはず。


#if DEBUG
            float[] komabetuMeisai = new float[Finger_Honshogi.Items_KomaOnly.Length];
#endif
            //
            // 盤上にある駒だけ、項目番号を調べます。
            //

            //----------------------------------------
            // 項目番号リスト
            //----------------------------------------
            //
            // 40個の駒と、14種類の持ち駒があるだけなので、
            // 54個サイズの長さがあれば足りるんだぜ☆　固定長にしておこう☆
            //
            int nextIndex = 0;
            int[] komokuArray_unsorted = new int[54];//昇順でなくても構わないアルゴリズムにすること。

            for (int i = 0; i < Finger_Honshogi.Items_KomaOnly.Length; i++)// 全駒
            {
                positionA.AssertFinger(Finger_Honshogi.Items_KomaOnly[i]);
                Busstop koma = positionA.BusstopIndexOf(Finger_Honshogi.Items_KomaOnly[i]);

                if (Okiba.ShogiBan == Conv_Busstop.ToOkiba(koma))
                {
                    // 盤上
                    komokuArray_unsorted[nextIndex] = Util_FvParamIndex.ParamIndex_Banjo(koma);
                    nextIndex++;
                }
                else
                {
                    // 持ち駒は、ここでは無視します。
                }
            }
            // 持ち駒：先後×７種類
            for (int iPside = 0; iPside < Array_Playerside.Items_PlayerOnly.Length; iPside++)
            {
                for (int iKomasyurui = 0; iKomasyurui < Array_Komasyurui.MotiKoma7Syurui.Length; iKomasyurui++)
                {
                    komokuArray_unsorted[nextIndex] = Util_FvParamIndex.ParamIndex_Moti(positionA, Array_Playerside.Items_PlayerOnly[iPside], Array_Komasyurui.MotiKoma7Syurui[iKomasyurui]);
                    nextIndex++;
                }
            }
            //Array.Sort(komokuArray_unsorted);

            //
            //
            // 例えば、[1P１三歩、2P２一桂]という組み合わせと、[2P２一桂、1P１三歩]という組み合わせは、同じだが欄が２つある。
            // そこで、表の半分を省きたい。
            // しかし、表を三角形にするためには、要素は昇順にソートされている必要がある。
            // 合法手１つごとにソートしていては、本末転倒。
            // そこで、表は正方形に読み、内容は三角形の部分にだけ入っているということにする。
            //
            //
            // 例えば、[1P１三歩、1P１三歩]という組み合わせもある。これは、自分自身の絶対位置の評価として試しに、残しておいてみる☆
            //
            //
            for (int iA = 0; iA < nextIndex; iA++)
            {
                int p1 = komokuArray_unsorted[iA];

                for (int iB = 0; iB < nextIndex; iB++)
                {
                    int p2 = komokuArray_unsorted[iB];

                    if (p1 <= p2) // 「p2 < p1」という組み合わせは同じ意味なので省く。「p1==p2」は省かない。
                    {
                        //----------------------------------------
                        // よし、組み合わせだぜ☆！
                        //----------------------------------------
                        out_score += fv.NikomaKankeiPp_ForMemory[p1, p2];
                    }
                    else
                    {
                        //----------------------------------------
                        // 使っていない方の三角形だぜ☆！
                        //----------------------------------------

                        // スルー。
                    }
                }
            }

            return out_score;
        }
    }
}
