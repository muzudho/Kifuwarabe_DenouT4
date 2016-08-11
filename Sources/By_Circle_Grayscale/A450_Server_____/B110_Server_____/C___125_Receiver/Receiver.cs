using System.Diagnostics;

namespace Grayscale.A450_Server_____.B110_Server_____.C___125_Receiver
{
    /// <summary>
    /// 受信機能です。
    /// </summary>
    public interface Receiver
    {

        #region プロパティー

        /// <summary>
        /// 擬似将棋サーバーです。
        /// </summary>
        object Owner_EngineClient { get; }//EngineClient型
        void SetOwner_EngineClient(object owner_EngineClient);//EngineClient型

        #endregion


        void OnListenUpload_Async(object sender, DataReceivedEventArgs e);

    }
}
