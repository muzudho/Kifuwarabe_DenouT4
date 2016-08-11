namespace Grayscale.A120_KifuSfen___.B140_SfenStruct_.C250____Struct
{

    /// <summary>
    /// SFENのstartpos文字列を入れているという明示をします。
    /// 
    /// string では分かりづらかったので。
    /// </summary>
    public class SfenstringImpl
    {
        public string ValueStr { get { return this.valueStr; } }
        private string valueStr;

        public SfenstringImpl()
        {
            this.valueStr = "";
        }

        public SfenstringImpl(string src)
        {
            this.valueStr = src;
        }

        public override string ToString()
        {
            return this.ValueStr;
        }
    }
}
