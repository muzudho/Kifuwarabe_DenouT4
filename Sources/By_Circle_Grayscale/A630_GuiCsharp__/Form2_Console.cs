using System.Windows.Forms;

namespace Grayscale.P699Form
{
    public partial class Form2_Console : Form
    {

        public Form1Shogi Form1_Shogi { get { return this.form1_Shogi; } }
        private Form1Shogi form1_Shogi;

        public UcForm2Main Uc_Form2Main { get { return this.uc_Form2Main; } }

        public Form2_Console(Form1Shogi form1_Shogi)
        {
            this.form1_Shogi = form1_Shogi;

            InitializeComponent();
        }



    }
}
