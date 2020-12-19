using Grayscale.Kifuwaragyoku.Entities.Logging;
using Grayscale.A500ShogiEngine.B130FeatureVect.C500Struct;
using Grayscale.A690FvLearn.B110FvLearn.C420Inspection;

#if DEBUG
// using Grayscale.Kifuwaragyoku.Entities.Logging;
#endif

namespace Grayscale.A690FvLearn.B110FvLearn.C430Zooming
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class UtilZooming
    {

        /// <summary>
        /// フィーチャー・ベクターの概要をデバッグ出力します。
        /// 
        /// 順位を点数に変換します。
        /// </summary>
        public static void ZoomTo_FvParamRange(FeatureVector fv, ILogTag logTag)
        {
            float negative_length;// 負の数の一番小さな値の絶対値。
            float positive_length;// 正の数の一番大きな値の絶対値。
            bool longest_positive; // 正の方の絶対値の方が大きければ真。
            int negative_items;//負の項目数。平均値を求めるのに使う。
            int positive_items;//正の項目数
            float negative_total;//負の合計。平均値を求めるのに使う。
            float positive_total;//正の合計。
            float zoom;
            int notZero;
            {
                negative_length = 0.0f;
                positive_length = 0.0f;
                negative_items = 0;
                positive_items = 0;
                negative_total = 0.0f;
                positive_total = 0.0f;
                notZero = 0;
                for (int p1 = 0; p1 < FeatureVectorImpl.CHOSA_KOMOKU_P; p1++)
                {
                    for (int p2 = 0; p2 < FeatureVectorImpl.CHOSA_KOMOKU_P; p2++)
                    {
                        float cellValue = fv.NikomaKankeiPp_ForMemory[p1, p2];
                        if (cellValue < -negative_length)
                        {
                            negative_length = -cellValue;
                        }
                        else if (positive_length < cellValue)
                        {
                            positive_length = cellValue;
                        }

                        if (cellValue != 0.0f)
                        {
                            notZero++;
                        }

                        if (cellValue < 0.0f)
                        {
                            negative_items++;
                            negative_total += cellValue;
                        }
                        else if (0.0f < cellValue)
                        {
                            positive_items++;
                            positive_total += cellValue;
                        }
                    }
                }

                // 長いのは正負のどちらか。
                if (negative_length < positive_length)
                {
                    longest_positive = true;
                }
                else
                {
                    longest_positive = false;
                }
#if DEBUG
                logTag.AppendLine("topology");
                logTag.AppendLine("   negative_length =" + negative_length);
                logTag.AppendLine("   positive_length =" + positive_length);
                logTag.AppendLine("   longest_positive=" + longest_positive);
                logTag.AppendLine("   negative_average=" + (negative_items == 0 ? 0 : negative_total / negative_items));
                logTag.AppendLine("   positive_average=" + (positive_items == 0 ? 0 : positive_total / positive_items));
                logTag.AppendLine("   notZero         =" + notZero);
                logTag.AppendLine("----------------------------------------");
                logTag.Flush(LogTypes.Plain);
#endif
            }


            //----------------------------------------
            // 正負の長い方 を abs 999.0(*bairitu) に合わせたい。
            //----------------------------------------
            if (longest_positive)
            {
                zoom = UtilInspection.FvParamRange(fv) / positive_length;
            }
            else
            {
                zoom = UtilInspection.FvParamRange(fv) / negative_length;
            }

            negative_length = 0.0f;
            positive_length = 0.0f;
            negative_items = 0;
            positive_items = 0;
            negative_total = 0.0f;
            positive_total = 0.0f;
            notZero = 0;
            for (int p1 = 0; p1 < FeatureVectorImpl.CHOSA_KOMOKU_P; p1++)
            {
                for (int p2 = 0; p2 < FeatureVectorImpl.CHOSA_KOMOKU_P; p2++)
                {
                    float value = fv.NikomaKankeiPp_ForMemory[p1, p2] * zoom;
                    fv.NikomaKankeiPp_ForMemory[p1, p2] = value;
                    if (value < -negative_length)
                    {
                        negative_length = -value;
                    }
                    else if (positive_length < value)
                    {
                        positive_length = value;
                    }

                    if (value != 0.0d)
                    {
                        notZero++;
                    }

                    if (value < 0.0d)
                    {
                        negative_items++;
                        negative_total += value;
                    }
                    else if (0.0d < value)
                    {
                        positive_items++;
                        positive_total += value;
                    }
                }
            }

            // 長いのは正負のどちらか。
            if (negative_length < positive_length)
            {
                longest_positive = true;
            }
            else
            {
                longest_positive = false;
            }
#if DEBUG
            logTag.AppendLine("end");
            logTag.AppendLine("   negative_length =" + negative_length);
            logTag.AppendLine("   positive_length =" + positive_length);
            logTag.AppendLine("   longest_positive=" + longest_positive);
            logTag.AppendLine("   zoom            =" + zoom);
            logTag.AppendLine("   negative_average=" + (negative_items == 0 ? 0 : negative_total / negative_items));
            logTag.AppendLine("   positive_average=" + (positive_items == 0 ? 0 : positive_total / positive_items));
            logTag.AppendLine("   notZero         =" + notZero);
            logTag.AppendLine("----------------------------------------");
            logTag.Flush(LogTypes.Plain);
#endif
        }

    }

}
