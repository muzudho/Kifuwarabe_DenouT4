using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.A210_KnowNingen_.B170_WordShogi__.C500____Word;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B630_Sennitite__.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct
{
    public interface Earth
    {
        /// <summary>
        /// 千日手カウンター。
        /// </summary>
        /// <returns></returns>
        SennititeCounter GetSennititeCounter();

        void Clear();





        /// <summary>
        /// 使い方自由。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetProperty(string key, object value);
        /// <summary>
        /// 使い方自由。
        /// </summary>
        object GetProperty(string key);

    }
}
