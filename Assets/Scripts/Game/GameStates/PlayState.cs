using Photon.Pun;
using System.IO;
using TMPro;
using UnityEngine;

public class PlayState : States<GameStates>
{
    private bool _startGame = false;
    private Timer _roundTimer;
    private const float ROUND_TIME = 60f;

    private PunGameUI _ui;

    private PunGhostSpawner _ghostSpawner;

    private float[] _scores;

    private const float GHOST_VALUE = 100;

    public override void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        _scores = new float[] { 0, 0 };

        _roundTimer = new Timer(ROUND_TIME);
        _roundTimer.OnTimerComplete += TimerComplete;

        _ui = InstantiateGameUI();

        _ghostSpawner = GetGhostSpawner();
        _ghostSpawner.OnGhostSpawned += GhostSpawned;

        var startTimerGo = InstantiateStartTimer();
        startTimerGo
            .GetComponent<TimerBehaviour>()
            .Timer
            .OnTimerComplete += () => { _startGame = true; _ghostSpawner.StartSpawn(); };
    }

    public override void Execute()
    {
        // Start running game
        if (!_startGame) return;

        if (!PhotonNetwork.IsMasterClient) return;

        // Tick level timer (30s or 1m)
        _roundTimer.Tick(Time.deltaTime);

        // Update Timer
        _ui.UpdateTimer(_roundTimer.Countdown);
    }

    public override void Sleep()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        // Stop game running
        _startGame = false;
        _roundTimer.OnTimerComplete -= TimerComplete;

        _ghostSpawner.StopSpawn();
        _ghostSpawner.OnGhostSpawned -= GhostSpawned;
    }

    private static PunGameUI InstantiateGameUI()
    {
        return PhotonNetwork
            .Instantiate(Path.Combine("GhostHunt/UI", "GameUI"), Vector3.zero, Quaternion.identity)
            .GetComponent<PunGameUI>();
    }

    private static PunGhostSpawner GetGhostSpawner()
    {
        return new GameObject("PunGhostSpawner").AddComponent<PunGhostSpawner>();
    }

    private GameObject InstantiateStartTimer()
    {
        return PhotonNetwork.Instantiate(Path.Combine("GhostHunt", "StartTimer"), new Vector3(), Quaternion.identity);
    }

    private void TimerComplete()
    {
        // DETERMINE WHO WINS
        //Transition to WinState or Loosestate
        _startGame = false;

        int winner = (int)_scores[0] > (int)_scores[1] ? 0 : (int)_scores[0] == (int)_scores[1] ? -1 : 1;
        Debug.Log("winner: " + winner);

        var endLevelGo = PhotonNetwork.Instantiate(Path.Combine("GhostHunt", "EndLevel"), Vector3.zero, Quaternion.identity);
        endLevelGo
            .GetComponent<EndLevel>()
            .Notify(winner);
    }

    private void GhostSpawned(PunGhost ghost)
    {
        ghost.OnDeath += OnGhostDeath;
    }

    private void OnGhostDeath(int playerId, PunGhost ghost)
    {
        ghost.OnDeath -= OnGhostDeath;

        // Update score per player
        _scores[playerId] += GHOST_VALUE;

        _ui.UpdateScore(_scores[playerId], playerId);
    }
}