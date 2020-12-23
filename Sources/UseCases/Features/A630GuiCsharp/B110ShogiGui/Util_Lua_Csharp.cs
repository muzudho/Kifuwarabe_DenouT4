﻿using System;
using System.IO;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Nett;
using NLua;

namespace Grayscale.Kifuwaragyoku.UseCases.Features
{
    public abstract class Util_Lua_Csharp
    {
        private static Lua lua;

        public static MainGui_Csharp ShogiGui { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="luaFuncName">実行したいLua関数の名前。</param>
        public static void Perform(string luaFuncName)
        {
            var profilePath = System.Configuration.ConfigurationManager.AppSettings["Profile"];
            var toml = Toml.ReadFile(Path.Combine(profilePath, "Engine.toml"));

            using (Util_Lua_Csharp.lua = new Lua())
            {
                // 初期化
                lua.LoadCLRPackage();


                //
                // 関数の登録
                //

                // Lua「debugOut("あー☆")」
                // ↓
                // C#「C onsole.WriteLine("あー☆")」
                lua.RegisterFunction("debugOut", typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));

                // Lua「screen_refresh()」
                // ↓
                // C#「Util_Lua.Screen_Redraw()」
                lua.RegisterFunction("screen_refresh", typeof(Util_Lua_Csharp).GetMethod("Screen_Redraw", new Type[] { }));

                // Lua「screen_clearOutput()」
                // ↓
                // C#「Util_Lua.screen_redrawStarlights()」
                lua.RegisterFunction("screen_refreshStarlights", typeof(Util_Lua_Csharp).GetMethod("Screen_RedrawStarlights", new Type[] { }));



                // Lua「inputBox_play()」
                // ↓
                // C#「Util_Lua.InputBox_Play()」
                lua.RegisterFunction("inputBox_play", typeof(Util_Lua_Csharp).GetMethod("InputBox_Play", new Type[] { }));



                // Lua「outputBox_clear()」
                // ↓
                // C#「Util_Lua.Screen_ClearOutput()」
                lua.RegisterFunction("outputBox_clear", typeof(Util_Lua_Csharp).GetMethod("OutputBox_Clear", new Type[] { }));



                // Lua「kifu_clear()」
                // ↓
                // C#「Util_Lua.Kifu_Clear()」
                lua.RegisterFunction("kifu_clear", typeof(Util_Lua_Csharp).GetMethod("Kifu_Clear", new Type[] { }));


                //----------------------------------------------------------------------------------------------------

                Util_Lua_Csharp.lua.DoFile(Path.Combine(profilePath, toml.Get<TomlTable>("Resources").Get<string>("NarabeDataGuiLua")));//固定
                Util_Lua_Csharp.lua.GetFunction(luaFuncName).Call();

                // FIXME:Close()でエラーが起こってしまう。
                //Util_Lua_KifuNarabe.lua.Close();

                //----------------------------------------------------------------------------------------------------

            }
        }


        public static void Screen_Redraw()
        {
            Util_Lua_Csharp.ShogiGui.RepaintRequest.SetFlag_RefreshRequest();
        }

        public static void Screen_RedrawStarlights()
        {
            Util_Lua_Csharp.ShogiGui.RepaintRequest.SetFlag_RecalculateRequested();// 駒の再描画要求
        }

        public static void InputBox_Play()
        {
            // [再生]タイマー開始☆
            ((TimedC_SaiseiCapture)Util_Lua_Csharp.ShogiGui.TimedC).SaiseiEventQueue.Enqueue(new SaiseiEventState(SaiseiEventStateName.Start));
        }

        public static void OutputBox_Clear()
        {
            Util_Lua_Csharp.ShogiGui.RepaintRequest.SyuturyokuRequest = RepaintRequestGedanTxt.Clear;
        }

        public static void Kifu_Clear()
        {
            Util_Lua_Csharp.ClearKifu(Util_Lua_Csharp.ShogiGui, Util_Lua_Csharp.ShogiGui.RepaintRequest);
        }





        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋盤の上の駒を、全て駒袋に移動します。 [クリアー]
        /// ************************************************************************************************************************
        /// </summary>
        public static void ClearKifu(MainGui_Csharp mainGui, RepaintRequest repaintRequest)
        {
            mainGui.Link_Server.Playing.ClearEarth();

            // 棋譜を空っぽにします。
            //Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(mainGui.Link_Server.KifuTree, null,logger);

            IPosition newSky = new Position(mainGui.SkyWrapper_Gui.GuiSky);

            int figKoma;

            // 先手
            figKoma = (int)Finger_Honshogi.SenteOh;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro01), Komasyurui14.H06_Gyoku__)); //先手王
            figKoma = (int)Finger_Honshogi.GoteOh;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro02), Komasyurui14.H06_Gyoku__)); //後手王

            figKoma = (int)Finger_Honshogi.Hi1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro03), Komasyurui14.H07_Hisya__)); //飛
            figKoma = (int)Finger_Honshogi.Hi2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro04), Komasyurui14.H07_Hisya__));

            figKoma = (int)Finger_Honshogi.Kaku1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro05), Komasyurui14.H08_Kaku___)); //角
            figKoma = (int)Finger_Honshogi.Kaku2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro06), Komasyurui14.H08_Kaku___));

            figKoma = (int)Finger_Honshogi.Kin1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro07), Komasyurui14.H05_Kin____)); //金
            figKoma = (int)Finger_Honshogi.Kin2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro08), Komasyurui14.H05_Kin____));
            figKoma = (int)Finger_Honshogi.Kin3;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro09), Komasyurui14.H05_Kin____));
            figKoma = (int)Finger_Honshogi.Kin4;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro10), Komasyurui14.H05_Kin____));

            figKoma = (int)Finger_Honshogi.Gin1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro11), Komasyurui14.H04_Gin____)); //銀
            figKoma = (int)Finger_Honshogi.Gin2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro12), Komasyurui14.H04_Gin____));
            figKoma = (int)Finger_Honshogi.Gin3;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro13), Komasyurui14.H04_Gin____));
            figKoma = (int)Finger_Honshogi.Gin4;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro14), Komasyurui14.H04_Gin____));

            figKoma = (int)Finger_Honshogi.Kei1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro15), Komasyurui14.H03_Kei____)); //桂
            figKoma = (int)Finger_Honshogi.Kei2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro16), Komasyurui14.H03_Kei____));
            figKoma = (int)Finger_Honshogi.Kei3;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro17), Komasyurui14.H03_Kei____));
            figKoma = (int)Finger_Honshogi.Kei4;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro18), Komasyurui14.H03_Kei____));

            figKoma = (int)Finger_Honshogi.Kyo1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro19), Komasyurui14.H02_Kyo____)); //香
            figKoma = (int)Finger_Honshogi.Kyo2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro20), Komasyurui14.H02_Kyo____));
            figKoma = (int)Finger_Honshogi.Kyo3;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro21), Komasyurui14.H02_Kyo____));
            figKoma = (int)Finger_Honshogi.Kyo4;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro22), Komasyurui14.H02_Kyo____));

            figKoma = (int)Finger_Honshogi.Fu1;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro23), Komasyurui14.H01_Fu_____)); //歩
            figKoma = (int)Finger_Honshogi.Fu2;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro24), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu3;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro25), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu4;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro26), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu5;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro27), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu6;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro28), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu7;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro29), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu8;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro30), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu9;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P1, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro31), Komasyurui14.H01_Fu_____));

            figKoma = (int)Finger_Honshogi.Fu10;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro32), Komasyurui14.H01_Fu_____)); //歩
            figKoma = (int)Finger_Honshogi.Fu11;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro33), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu12;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro34), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu13;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro35), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu14;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro36), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu15;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro37), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu16;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro38), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu17;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro39), Komasyurui14.H01_Fu_____));
            figKoma = (int)Finger_Honshogi.Fu18;
            newSky.PutOverwriteOrAdd_Busstop(figKoma, Conv_Busstop.ToBusstop(Playerside.P2, Masu_Honshogi.Query_Basho(Masu_Honshogi.nfukuro40), Komasyurui14.H01_Fu_____));


            {
                newSky.SetTemezumi(0);//空っぽに戻すので、 0手済みに変更。
                MoveEx newNode = new MoveExImpl();

                string jsaFugoStr;

                Playerside rootPside = TreeImpl.MoveEx_ClearAllCurrent(mainGui.Link_Server.KifuTree, newSky);


                Util_Functions_Server.AfterSetCurNode_Srv(
                    mainGui.SkyWrapper_Gui,
                    newNode,
                    newNode.Move,
                    newSky,
                    out jsaFugoStr,
                    mainGui.Link_Server.KifuTree);
                repaintRequest.SetFlag_RefreshRequest();

                mainGui.Link_Server.Playing.SetEarthProperty(Word_KifuTree.PropName_Startpos, "9/9/9/9/9/9/9/9/9 b K1R1B1G2S2N2L2P9 k1r1b1g2s2n2l2p9 1");
            }
        }


    }
}
