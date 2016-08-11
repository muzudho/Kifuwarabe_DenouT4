using Grayscale.P324_KifuTree___.C___250_Struct;
using Grayscale.P325_PnlTaikyoku.C___250_Struct;

namespace Grayscale.P325_PnlTaikyoku.C250____Struct
{
    public class Model_TaikyokuImpl : Model_Taikyoku
    {
        public KifuTree Kifu
        {
            get
            {
                return this.kifu;
            }
        }
        public void SetKifu(KifuTree kifu)
        {
            this.kifu = kifu;
        }
        private KifuTree kifu;

        public Model_TaikyokuImpl(KifuTree kifu)
        {
            this.kifu = kifu;
        }
    }
}
