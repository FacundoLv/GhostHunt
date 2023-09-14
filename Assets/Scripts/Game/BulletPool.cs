using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public Bullet GetBullet { get => _pool.GetObject(); }

    [SerializeField] private Bullet _bulletPrefab = null;

    private ObjectPool<Bullet> _pool;

    //private Dictionary<int, Bullet> _bullets = new Dictionary<int, Bullet>();

    private void Awake()
    {
        _pool = new ObjectPool<Bullet>(NewBullet, Bullet.OnTurnOn, Bullet.OnTurnOff);
    }

    private Bullet NewBullet()
    {
        var bullet = Instantiate(_bulletPrefab, transform);
        bullet.OnReachedTarget += ReturnBullet;
        //var bulletGo = PhotonNetwork.Instantiate(Path.Combine("GhostHunt", _bulletPrefab.name), transform.position, Quaternion.identity);
        //var bullet = bulletGo.GetComponent<Bullet>();
        //bullet.OnReachedTarget += ReturnBullet;
        //bullet.Id = bullet.GetHashCode();
        //_bullets.Add(bullet.Id, bullet);
        return bullet;
    }

    public void ReturnBullet(Bullet bullet)
    {
        _pool.ReturnObject(bullet);
    }

    //public void ReturnBullet(int bulletId)
    //{
    //    _pool.ReturnObject(_bullets[bulletId]);
    //}
}
