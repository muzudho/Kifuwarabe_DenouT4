﻿using System;
using System.Reflection;
using System.Windows.Forms;
using Grayscale.A060Application.B110Log.C500Struct;
using Grayscale.A630GuiCsharp.B110ShogiGui.C500Gui;
using Grayscale.A630GuiCsharp.B110ShogiGui.C510Form;

namespace Grayscale.P699Form
{
    [Serializable]
    public partial class Form1Shogi : Form, Form1Shogiable
    {
        private MainGui_Csharp owner;

        /// <summary>
        /// 別窓。コンソール・ウィンドウ。
        /// </summary>
        public Form2_Console Form2_Console
        {
            get
            {
                return this.form2_Console;
            }
        }
        private Form2_Console form2_Console;

        public UcForm1Mainable Uc_Form1Main
        {
            get
            {
                return this.uc_Form1Main;
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクターです。
        /// ************************************************************************************************************************
        /// </summary>
        public Form1Shogi(MainGui_Csharp owner)
        {
            this.owner = owner;
            InitializeComponent();
            this.uc_Form1Main.SetMainGui(this.owner);

            //----------------------------------------
            // 別窓を開きます。
            //----------------------------------------
            this.form2_Console = new Form2_Console(this);
            this.form2_Console.Show(this);
        }


        public DELEGATE_Form1_Load Delegate_Form1_Load { get; set; }


        /// <summary>
        /// ************************************************************************************************************************
        /// ウィンドウが表示される直前にしておく準備をここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_Form1_Load(object sender, EventArgs e)
        {
            //------------------------------
            // タイトルバーに表示する、「タイトル 1.00.0」といった文字を設定します。
            //------------------------------
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = String.Format("{0} {1}.{2}.{3}", this.Text, version.Major, version.Minor.ToString("00"), version.Build);

            if (null != this.Delegate_Form1_Load)
            {
                this.Delegate_Form1_Load(this.Uc_Form1Main.MainGui, sender, e);
            }
        }

        /// <summary>
        /// ************************************************************************************************************************
        /// ウィンドウが閉じられる直前にしておくことを、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ui_Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ILogger errH = ErrorControllerReference.ProcessGuiDefault;
            this.owner.Shutdown(errH);
        }

        private void Ui_Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ILogger errH = ErrorControllerReference.ProcessGuiDefault;
            this.owner.Shutdown(errH);
        }
    }
}