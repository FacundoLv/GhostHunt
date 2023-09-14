using Photon.Pun;
using UnityEngine;
using TMPro;
using System;

public class PunGameUI : MonoBehaviourPun
{
    [SerializeField] private TextMeshProUGUI[] playerScoreDisplays = null;
    [SerializeField] private TextMeshProUGUI roundTimerDisplay = null;

    internal void UpdateTimer(float timeLeft)
    {
        photonView.RPC(nameof(UpdateTimerDisplay), RpcTarget.AllBuffered, timeLeft);
    }

    [PunRPC]
    private void UpdateTimerDisplay(float timeLeft)
    {
        roundTimerDisplay.text = string.Format("{0:00}", timeLeft);
    }

    internal void UpdateScore(float score, int playerId)
    {
        photonView.RPC(nameof(UpdatePlayerScore), RpcTarget.AllBuffered, new object[] { score, playerId });
    }

    [PunRPC]
    private void UpdatePlayerScore(float score, int playerId)
    {
        playerScoreDisplays[playerId].text = string.Format("{0:000}", score);
    }
}
