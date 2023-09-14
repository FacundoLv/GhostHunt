using UnityEngine;

public class Gun : MonoBehaviour
{
    public ParticleSystem muzzleFlash;

    [SerializeField] private Transform _muzzle = null;
    [SerializeField] private float _firePower = 5f;

    private Camera _cam;
    private Vector3 fireDir;
    private ParticleSystem _muzzleFlash;
    private BulletPool _bulletPool;

    private void Awake()
    {
        _cam = Camera.main;
        _bulletPool = FindObjectOfType<BulletPool>();
    }

    void Update()
    {
        PointAtTarget();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Shoot();
    }

    private void PointAtTarget()
    {
        Ray mouseRay = _cam.ScreenPointToRay(Input.mousePosition);
        fireDir = mouseRay.direction;
        transform.LookAt(mouseRay.origin + fireDir);
    }

    private void Shoot()
    {
        _bulletPool
            .GetBullet
            .SetPosition(_muzzle.position)
            .SetVelocity(fireDir * _firePower);

        EmitMuzzleParticle();
    }

    private void EmitMuzzleParticle()
    {
        if (_muzzleFlash == null)
            _muzzleFlash = Instantiate(muzzleFlash, _muzzle.position, _muzzle.rotation, _muzzle);

        if (!_muzzleFlash.isPlaying) _muzzleFlash.Play();
    }
}
