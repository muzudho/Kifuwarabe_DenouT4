﻿
using System.Windows.Forms;

namespace Grayscale.A630GuiCsharp.B110ShogiGui.C100Widgets
{
    public abstract class AspectOriented_TextBox
    {

        /// <summary>
        /// 全選択　[Ctrl]+[A]
        /// </summary>
        public static void KeyDown_SelectAll(object sender, KeyEventArgs e)
        {
            //------------------------------
            // [Ctrl]+[A] で、全選択します。
            //------------------------------
            if (e.KeyCode == System.Windows.Forms.Keys.A & e.Control == true)
            {
                ((TextBox)sender).SelectAll();
            }
        }

    }
}
