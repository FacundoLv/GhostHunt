using UnityEngine;

public class GhostPool : MonoBehaviour
{
    public Ghost GetGhost { get => _pool.GetObject(); }

    [SerializeField] private Ghost _ghostPrefab = null;

    private ObjectPool<Ghost> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Ghost>(NewGhost, Ghost.OnTurnOn, Ghost.OnTurnOff);
    }

    private Ghost NewGhost()
    {
        return Instantiate(_ghostPrefab, transform);
    }

    public void ReturnGhost(Ghost ghost)
    {
        _pool.ReturnObject(ghost);
    }
}
