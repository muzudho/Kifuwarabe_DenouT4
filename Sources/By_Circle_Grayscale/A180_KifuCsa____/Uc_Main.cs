using Grayscale.A180_KifuCsa____.B120_KifuCsa____.C___250_Struct;
using Grayscale.A180_KifuCsa____.B120_KifuCsa____.C250____Struct;
using Grayscale.A180_KifuCsa____.B120_KifuCsa____.C500____Writer;
using System;
using System.Windows.Forms;

namespace Grayscale.P169_Form_______
{
    public partial class Uc_Main : UserControl
    {
        private CsaKifu CsaKifu { get; set; }

        public Uc_Main()
        {
            this.CsaKifu = new CsaKifuImpl();
            InitializeComponent();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            this.CsaKifu = Util_Csa.ReadFile( this.txtKifuFilepath.Text );

            string filepath_out = this.txtKifuFilepath.Text + ".デバッグ.txt";
            MessageBox.Show("終わった。デバッグ出力をする☆\nファイルパス=[" + filepath_out + "]", "かんりょう");
            //デバッグ用にファイルを書き出します。
            CsaKifuWriterImpl.WriteForDebug(filepath_out, this.CsaKifu);
        }
    }
}
