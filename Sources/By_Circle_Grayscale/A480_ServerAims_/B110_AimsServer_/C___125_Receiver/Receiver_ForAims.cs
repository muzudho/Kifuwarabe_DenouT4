using Grayscale.A450_Server_____.B110_Server_____.C___125_Receiver;
using Grayscale.A480_ServerAims_.B110_AimsServer_.C___070_ServerBase;

namespace Grayscale.A480_ServerAims_.B110_AimsServer_.C___125_Receiver
{
    public interface Receiver_ForAims : Receiver
    {

        /// <summary>
        /// オーナー・サーバー。
        /// </summary>
        AimsServerBase Owner_AimsServer { get; }
        void SetOwner_AimsServer(AimsServerBase owner_AimsServer);

    }
}
