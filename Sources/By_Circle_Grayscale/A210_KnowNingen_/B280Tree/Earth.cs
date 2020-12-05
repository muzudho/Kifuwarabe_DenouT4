using Grayscale.A210KnowNingen.B630Sennitite.C500Struct;

namespace Grayscale.A210KnowNingen.B280Tree.C500Struct
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
