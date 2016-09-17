using Grayscale.A000_Platform___.B025_Machine____;
using Grayscale.A060_Application.B110_Log________.C___500_Struct;
using Grayscale.A060_Application.B110_Log________.C500____Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B320_ConvWords__.C500____Converter;
using Grayscale.A210_KnowNingen_.B420_UtilSky258_.C500____UtilSky;
using Grayscale.A210_KnowNingen_.B670_ConvKyokume.C500____Converter;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C___250_OperationA;
using Grayscale.A210_KnowNingen_.B690_Ittesasu___.C500____UtilA;
using Grayscale.A210_KnowNingen_.B740_KifuParserA.C400____Conv;
using System.Collections.Generic;
using System.Diagnostics;

namespace Grayscale.A950_UnitTest___
{
    class Program
    {
        static void Main(string[] args)
        {
            KwLogger logger = Util_Loggers.ProcessUnitTest_DEFAULT;

            logger.AppendLine("テストＡ");
            logger.Flush(LogTypes.Plain);
            MachineImpl.GetInstance().ReadKey();



            Sky position_Sky = Util_SkyCreator.New_Hirate();

            // 盤面をログ出力したいぜ☆
            logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(position_Sky)));
            logger.Flush(LogTypes.Plain);
            MachineImpl.GetInstance().ReadKey();


            List<Move> pv = new List<Move>();
            pv.Add(Move.Empty);// 「同」（※同歩など）を調べるために１つ前を見にくるので、空を入れておく。

            //────────────────────────────────────────
            // 一手指す☆
            //────────────────────────────────────────
            // ▲７六歩
            string commandLine = "7g7f 3c3d 8h2b+ 3a2b B*8h";

            for (int iPly=1; iPly<6; iPly++)
            {
                string rest;
                Move move = Conv_StringMove.ToMove(out rest, commandLine, pv[pv.Count - 1], position_Sky, logger);
                commandLine = rest;

                IttesasuResult syuryoResult;
                Util_IttesasuRoutine.DoMove(out syuryoResult, move, position_Sky, logger);
                move = syuryoResult.SyuryoMove;// 駒を取った場合、moveは更新される。
                position_Sky = syuryoResult.SyuryoKyokumenW;
                pv.Add(move);

                // 盤面をログ出力したいぜ☆
                logger.AppendLine("sfen=[" + Conv_Move.ToSfen(move) + "] captured=["+Conv_Komasyurui.ToStr_Ichimoji(Conv_Move.ToCaptured(move))+"]");
                logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(position_Sky)));
                logger.Flush(LogTypes.Plain);
                MachineImpl.GetInstance().ReadKey();
            }




            //────────────────────────────────────────
            // 一手戻す☆
            //────────────────────────────────────────
            for (int iPly = 5; 0<iPly; iPly--)
            {
                Move moved = pv[pv.Count - 1];
                pv.RemoveAt(pv.Count - 1);

                IttemodosuResult syuryoResult2;
                Util_IttemodosuRoutine.UndoMove(
                    out syuryoResult2,
                    moved,
                    position_Sky,
                    logger
                    );
                position_Sky = syuryoResult2.SyuryoSky;
                Debug.Assert(null!= position_Sky, "局面がヌル");

                // 盤面をログ出力したいぜ☆
                logger.AppendLine("back sfen=[" + Conv_Move.ToSfen(moved) + "] captured=[" + Conv_Komasyurui.ToStr_Ichimoji(Conv_Move.ToCaptured(moved)) + "]");
                logger.AppendLine(Conv_Shogiban.ToLog(Conv_Sky.ToShogiban(position_Sky)));
                logger.Flush(LogTypes.Plain);
                MachineImpl.GetInstance().ReadKey();
            }

        }
    }
}
