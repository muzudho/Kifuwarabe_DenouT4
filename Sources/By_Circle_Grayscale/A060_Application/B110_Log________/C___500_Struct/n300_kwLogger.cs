using Grayscale.A060_Application.B110_Log________.C___500_Struct;

namespace Grayscale.A060_Application.B110_Log________.C___500_Struct
{
    /// <summary>
    /// ロガー、エラーハンドリング
    /// 
    /// きふわらべのロガー。
    /// </summary>
    public interface KwLogger
    {
        /// <summary>
        /// ファイル名
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// 拡張子抜きファイル名（without extension）
        /// </summary>
        string FileNameWoe { get; }
        /// <summary>
        /// 拡張子
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        bool Enable { get; }

        /// <summary>
        /// タイムスタンプを出力するか。
        /// </summary>
        bool Print_TimeStamp { get; }


        /// <summary>
        /// テキストを、ログ・ファイルの末尾に追記します。改行付き。
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_Add(
            string line,
            LogTypes logTypes
            );


        /// <summary>
        /// テキストで上書きします。末尾に改行付き。
        /// </summary>
        /// <param name="line"></param>
        void WriteLine(
            string line,
            LogTypes logTypes
            );

        /// <summary>
        /// ログ・ファイルに記録します。（メモ）
        /// 
        /// 一旦、ログ・ファイルを空っぽにしたい場合などに。
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_Over(string line,
            LogTypes logTypes);


        /// <summary>
        /// ログ・ファイルに記録します。（サーバーへ送ったコマンドを）
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_S(string line);


        /// <summary>
        /// ログ・ファイルに記録します。（サーバーから受け取ったコマンドを）
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_C(string line);

    }
}
