using Photon.Pun;
using TMPro;
using UnityEngine;

public class TimerBehaviour : MonoBehaviourPun
{
    public Timer Timer { get => _timer; }

    [SerializeField] private float CountDownTime = 5f;
    [SerializeField] private TextMeshProUGUI _timerDisplay = null;

    private Timer _timer;

    private void Awake()
    {
        _timer = new Timer(CountDownTime);
        _timer.OnTimerComplete += TimerComplete;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        _timer.Tick(Time.deltaTime);
        photonView.RPC(nameof(ShowTimeLeft), RpcTarget.All, _timer.Countdown);
    }

    [PunRPC]
    private void ShowTimeLeft(float time)
    {
        _timerDisplay.text = string.Format("{0:00}", time);
    }

    private void TimerComplete()
    {
        _timer.OnTimerComplete -= TimerComplete;
        PhotonNetwork.Destroy(gameObject);
    }
}
