using Grayscale.Kifuwaragyoku.Entities.Configuration;
using Grayscale.Kifuwaragyoku.Entities.Evaluation;
using Grayscale.Kifuwaragyoku.Entities.Features;
using Grayscale.Kifuwaragyoku.Entities.Positioning;
using Grayscale.Kifuwaragyoku.Entities.Searching;

namespace Grayscale.Kifuwaragyoku.Entities
{
    public interface IPlaying
    {
        /// <summary>
        /// エンジン設定。
        /// </summary>
        IEngineConf EngineConf { get; }

        /// <summary>
        /// 右脳。
        /// </summary>
        IFeatureVector FeatureVector { get; set; }

        /// <summary>
        /// 時間管理
        /// </summary>
        ITimeManager TimeManager { get; set; }

        /// <summary>
        /// 千日手カウンター。
        /// </summary>
        /// <returns></returns>
        SennititeCounter GetSennititeCounter();

        /// <summary>
        /// 棋譜です。
        /// Loop2で使います。
        /// </summary>
        ITree Kifu { get; }

        /// <summary>
        /// 開始局面。
        /// </summary>
        IPosition StartingPosition { get; set; }

        /// <summary>
        /// 棋譜を空っぽにします。
        /// 
        /// ルートは残します。
        /// </summary>
        void ClearEarth();

        /// <summary>
        /// 使い方自由。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetEarthProperty(string key, object value);

        /// <summary>
        /// 使い方自由。
        /// </summary>
        object GetEarthProperty(string key);
    }
}
