using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B260_TedokuHisto.C___250_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using System.Runtime.CompilerServices;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct
{
    public interface Sky
    {
        void AssertFinger(
            Finger finger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// 手得ヒストリー。
        /// </summary>
        TedokuHistory TedokuHistory { get; }

        /// <summary>
        /// これから指す側。
        /// </summary>
        Playerside KaisiPside { get; }

        /// <summary>
        /// 何手目済みか。初期局面を 0 とする。
        /// </summary>
        int Temezumi { get; }

        Busstop BusstopIndexOf(
            Finger finger
            /*
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            */
        );

        void Foreach_Busstops(SkyBuffer.DELEGATE_Sky_Foreach delegate_Sky_Foreach);

        int Count
        {
            get;
        }

        Sky Clone();

        Fingers Fingers_All();
    }
}
