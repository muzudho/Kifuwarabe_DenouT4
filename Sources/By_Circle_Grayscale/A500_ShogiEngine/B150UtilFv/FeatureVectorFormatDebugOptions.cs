
namespace Grayscale.A500ShogiEngine.B523UtilFv.C490UtilFvFormat
{
    /// <summary>
    /// デバッグ・オプション。
    /// 
    /// フィーチャー・ベクター・ファイルに出力する内容を、デバッグ用のものに切り替えるフラグを持ちます。
    /// </summary>
    public static class FeatureVectorFormatDebugOptions
    {
        /// <summary>
        /// 評価値ではなく、パラメーター・インデックスを出力したい場合は真。デバッグ用。
        /// </summary>
        public static bool ParameterIndexOutput = false;


        /// <summary>
        /// 評価値を読み込むのではなく、パラメーター・インデックスを読み込みたい場合は真。デバッグ用。
        /// </summary>
        public static bool ParameterIndexInput = false;
    }
}
