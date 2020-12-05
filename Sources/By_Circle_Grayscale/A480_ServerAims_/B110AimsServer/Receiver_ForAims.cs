using Grayscale.A450Server.B110Server.C125Receiver;
using Grayscale.A480ServerAims.B110AimsServer.C070ServerBase;

namespace Grayscale.A480ServerAims.B110AimsServer.C125Receiver
{
    public interface Receiver_ForAims : IReceiver
    {

        /// <summary>
        /// オーナー・サーバー。
        /// </summary>
        AimsServerBase Owner_AimsServer { get; }
        void SetOwner_AimsServer(AimsServerBase owner_AimsServer);

    }
}
