using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C500____Struct;
using System.Runtime.CompilerServices;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using System.Collections.Generic;

namespace Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct
{
    public delegate void DELEGATE_Sky_Foreach(Finger finger, Busstop busstop, ref bool toBreak);

    public interface Sky
    {
        void AssertFinger(
            Finger finger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void IncreasePsideTemezumi();
        void DecreasePsideTemezumi();

        /// <summary>
        /// これから指す側。
        /// </summary>
        Playerside GetKaisiPside();//TODO: PositionからPsideを廃止したいぜ☆（＞＿＜）
        Playerside GetKaisiPside(Move move);
        void SetKaisiPside(Playerside pside);
        /// <summary>
        /// 先後を逆転させます。
        /// </summary>
        void ReversePside();

        /// <summary>
        /// 何手目済みか。初期局面を 0 とする。
        /// </summary>
        int Temezumi { get; }
        void SetTemezumi(int temezumi);

        Busstop BusstopIndexOf(
            Finger finger
            /*
            ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            */
        );

        void Foreach_Busstops(DELEGATE_Sky_Foreach delegate_Sky_Foreach);

        List<Busstop> Busstops { get; }

        /// <summary>
        /// 盤上の駒数と、持ち駒の数の合計。
        /// </summary>
        int Count
        {
            get;
        }

        Fingers Fingers_All();

        /// <summary>
        /// 持ち駒の枚数だぜ☆（＾▽＾）
        /// </summary>
        int[] MotiSu { get; set; }

        /// <summary>
        /// 手番を反転します。
        /// </summary>
        void ReversePlayerside();

        /// <summary>
        /// 追加分があれば。
        /// </summary>
        /// <param name="addsFinger1"></param>
        /// <param name="addsBusstops1"></param>
        void AddObjects(Finger[] addsFinger1, Busstop[] addsBusstops1);


        /// <summary>
        /// 駒台に戻すとき
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="finger"></param>
        /// <param name="busstop"></param>
        void PutOverwriteOrAdd_Busstop(Finger finger, Busstop busstop);
    }
}
