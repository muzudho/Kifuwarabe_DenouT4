﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.Kifuwaragyoku.Entities.Positioning
{
    public delegate void DELEGATE_Sky_Foreach(Finger finger, Busstop busstop, ref bool toBreak);

    public interface IPosition
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
