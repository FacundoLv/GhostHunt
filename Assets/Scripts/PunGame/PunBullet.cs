using Photon.Pun;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PunBullet : MonoBehaviourPun
{
    public Vector3 Force
    {
        get
        {
            return _body != null ? _body.velocity * _body.mass : Vector3.zero;
        }
    }

    public int Owner { get => _owner; }

    private Rigidbody _body;
    private TrailRenderer _trail;
    private int _owner;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _trail = GetComponent<TrailRenderer>();
    }

    public PunBullet SetVelocity(Vector3 velocity)
    {
        _body.velocity = velocity;
        return this;
    }

    public PunBullet SetOwner(int ownerId)
    {
        photonView.RPC(nameof(SetOwnerId), RpcTarget.All, new object[] { ownerId, photonView.ViewID });
        return this;
    }

    [PunRPC]
    private void SetOwnerId(int ownerId, int bulletId)
    {
        if (bulletId != photonView.ViewID) return;
        _owner = ownerId;
    }

    private void OnCollisionEnter(Collision collision)
    {
        BulletHit();
    }

    private void OnBecameInvisible()
    {
        Invoke(nameof(BulletHit), 1f);
    }

    private void BulletHit()
    {
        if (photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }
}
