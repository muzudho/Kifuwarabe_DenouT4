namespace Grayscale.Kifuwaragyoku.UseCases.Features
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
