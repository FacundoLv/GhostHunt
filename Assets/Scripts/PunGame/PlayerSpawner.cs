using Photon.Pun;
using System.IO;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] private Transform[] _spawnPoints = null;
    [SerializeField] private GameObject[] _playerPrefabs = null;
    [SerializeField] private GameObject[] _crosshairPrefabs = null;

    private void Awake()
    {
        int playerNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Vector3 position = _spawnPoints[playerNum].position;
        string prefab = _playerPrefabs[playerNum].name;
        string crosshair = _crosshairPrefabs[playerNum].name;

        LoadPlayer(position, prefab);
        LoadCrosshair(crosshair);
    }

    private static void LoadPlayer(Vector3 position, string prefab)
    {
        PhotonNetwork.Instantiate(Path.Combine("GhostHunt", prefab), position, Quaternion.identity, 0);
    }

    private static void LoadCrosshair(string crosshair)
    {
        PhotonNetwork.Instantiate(Path.Combine("GhostHunt/UI", crosshair), Vector3.zero, Quaternion.identity, 0);
    }
}
