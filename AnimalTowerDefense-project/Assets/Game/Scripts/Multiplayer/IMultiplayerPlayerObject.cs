using Photon.Pun;

namespace Multiplayer
{
    public interface IMultiplayerPlayerObject
    {
        void WhenNotMine();
        void SyncVariable(PhotonStream stream, PhotonMessageInfo info);
    }
}

