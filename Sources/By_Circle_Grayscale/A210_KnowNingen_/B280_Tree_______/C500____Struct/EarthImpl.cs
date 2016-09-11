using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B630_Sennitite__.C___500_Struct;
using Grayscale.A210_KnowNingen_.B630_Sennitite__.C500____Struct;
using System;
using System.Collections.Generic;

#if DEBUG
using System.Diagnostics;
#endif

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C500____Struct
{
    public class EarthImpl : Earth
    {
        public EarthImpl()
        {
            //----------------------------------------
            // 千日手カウンター
            //----------------------------------------
            this.sennititeCounter = new SennititeCounterImpl();



            this.properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// 千日手カウンター。
        /// </summary>
        /// <returns></returns>
        public SennititeCounter GetSennititeCounter()
        {
            return this.sennititeCounter;
        }
        private SennititeCounter sennititeCounter;


        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜を空っぽにします。
        /// ************************************************************************************************************************
        /// 
        /// ルートは残します。
        /// 
        /// </summary>
        public void Clear()
        {
            //----------------------------------------
            // 千日手カウンター
            //----------------------------------------
            this.sennititeCounter.Clear();
        }







        /// <summary>
        /// 使い方自由。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetProperty(string key, object value)
        {
            if (this.properties.ContainsKey(key))
            {
                this.properties[key] = value;
            }
            else
            {
                this.properties.Add(key, value);
            }
        }

        /// <summary>
        /// 使い方自由。
        /// </summary>
        public object GetProperty(string key)
        {
            object result;

            if (this.properties.ContainsKey(key))
            {
                result = this.properties[key];
            }
            else
            {
                result = "Unknown kifu property [" + key + "]";
            }

            return result;
        }
        private Dictionary<string, object> properties;

    }
}
