using Photon.Pun;
using System.IO;
using UnityEngine;

public class PunGun : MonoBehaviourPun
{
    public ParticleSystem muzzleFlash;

    [SerializeField] private Transform _muzzle = null;
    [SerializeField] private float _firePower = 5f;

    private Camera _cam;
    private Vector3 _fireDir;
    private ParticleSystem _muzzleFlash;

    [SerializeField] private PunBullet _bulletPrefab = null;

    private void Awake()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        PointAtTarget();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Shoot();
    }

    private void PointAtTarget()
    {
        Ray mouseRay = _cam.ScreenPointToRay(Input.mousePosition);
        _fireDir = mouseRay.direction;
        transform.LookAt(mouseRay.origin + _fireDir);
    }

    private void Shoot()
    {
        var bulletGo = PhotonNetwork.Instantiate(Path.Combine("GhostHunt", _bulletPrefab.name), _muzzle.position, transform.rotation);
        bulletGo
            .GetComponent<PunBullet>()
            .SetVelocity(_fireDir * _firePower)
            .SetOwner(PhotonNetwork.LocalPlayer.ActorNumber - 1);

        photonView.RPC("EmitMuzzleParticle", RpcTarget.All);
    }

    [PunRPC]
    private void EmitMuzzleParticle()
    {
        if (_muzzleFlash == null)
            _muzzleFlash = Instantiate(muzzleFlash, _muzzle.position, _muzzle.rotation, _muzzle);

        if (!_muzzleFlash.isPlaying) _muzzleFlash.Play();
    }
}
