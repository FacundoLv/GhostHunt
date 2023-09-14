using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class EndLevel : MonoBehaviourPun
{
    internal void Notify(int winner)
    {
        photonView.RPC(nameof(NotifyWinner), RpcTarget.All, winner);
    }

    [PunRPC]
    private void NotifyWinner(int winner)
    {
        Debug.Log("LocalPlayer: " + (PhotonNetwork.LocalPlayer.ActorNumber - 1));
        if (PhotonNetwork.LocalPlayer.ActorNumber - 1 == winner)
            GameManager.Instance.State = GameStates.Victory;
        else
            GameManager.Instance.State = GameStates.Loose;
    }
}
