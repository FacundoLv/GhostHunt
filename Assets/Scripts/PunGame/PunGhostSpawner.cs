using Photon.Pun;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class PunGhostSpawner : MonoBehaviour
{
    public event Action<PunGhost> OnGhostSpawned;

    private Camera _cam;

    private const float SPAWN_RATE = 2f;
    private const float LEFT = -0.1f;
    private const float RIGHT = 1.1f;
    private bool _isActive = true;

    private void Awake()
    {
        _cam = Camera.main;
    }

    public void StartSpawn()
    {
        _isActive = true;
        StartCoroutine(SpawnGhosts());
    }

    public void StopSpawn()
    {
        _isActive = false;
    }

    private IEnumerator SpawnGhosts()
    {
        while (_isActive)
        {
            Vector3 pos = GetSpawnPosition();
            Vector3 dir = GetLookAtDirection(pos);

            var ghostGo = PhotonNetwork.Instantiate(Path.Combine("GhostHunt", "Ghost"), new Vector3(0, 0, -10), Quaternion.identity);
            var ghost = ghostGo
                .GetComponent<PunGhost>()
                .SetPosition(pos)
                .SetDirection(dir);

            OnGhostSpawned?.Invoke(ghost);

            yield return new WaitForSeconds(SPAWN_RATE);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        float side = Random.value >= 0.5f ? RIGHT : LEFT;
        float height = Random.Range(0.2f, 0.8f);
        float distance = Random.Range(2.5f, 8f);
        return _cam.ViewportToWorldPoint(new Vector3(side, height, distance));
    }

    private static Vector3 GetLookAtDirection(Vector3 pos)
    {
        return pos.x > 1 ? -Vector3.right : Vector3.right;
    }
}
