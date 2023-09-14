using Photon.Pun;
using System;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PunGhost : MonoBehaviourPun
{
    public event Action<int, PunGhost> OnDeath;

    public ParticleSystem hitEffect;
    public ParticleSystem deathEffect;

    [SerializeField] private float _speed = 5f;
    private Rigidbody _body;
    private ParticleSystem _hitEffect = null;
    private ParticleSystem _deathEffect = null;

    private bool _canMove = true;
    private bool _isDisabled;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        Move();
    }

    private void Move()
    {
        if (!_canMove) return;
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject;
        // Ponerle un owner a la bala para notificar al fantasma en el Die > onDeath<ghost, int>
        if (obj.TryGetComponent(out PunBullet bullet))
        {
            ContactPoint contactPoint = collision.GetContact(0);

            photonView.RPC(
                nameof(KnockBack),
                RpcTarget.MasterClient,
                new object[]
                {
                    photonView.ViewID,
                    contactPoint.point,
                    contactPoint.normal,
                    bullet.Force
                });

            photonView.RPC(
                nameof(ShowHitEffect),
                RpcTarget.All,
                new object[]
                {
                    contactPoint.point,
                    contactPoint.normal
                });

            Die(bullet.Owner);
        }
    }

    [PunRPC]
    private void KnockBack(int id, Vector3 point, Vector3 normal, Vector3 force)
    {
        if (id != photonView.ViewID) return;
        _canMove = false;
        UnfreezeRBody();
        _body.AddForceAtPosition(GetForce(normal, force), point, ForceMode.Impulse);
    }

    private Vector3 GetForce(Vector3 direction, Vector3 force)
    {
        return direction * (force.magnitude / 2);
    }

    [PunRPC]
    private void ShowHitEffect(Vector3 point, Vector3 normal)
    {
        if (_hitEffect == null)
            _hitEffect = Instantiate(hitEffect, transform);

        Transform fxTransform = _hitEffect.transform;
        fxTransform.position = point;
        fxTransform.forward = -normal;

        _hitEffect.Play();
    }

    private void OnBecameInvisible()
    {
        if (photonView.IsMine && !_isDisabled)
        {
            _isDisabled = true;
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private async void Die(int hitterId)
    {
        await Task.Delay(1000);

        photonView.RPC(nameof(DeathEffect), RpcTarget.All);

        photonView.RPC(nameof(KillThis), RpcTarget.MasterClient, new object[] { photonView.ViewID, hitterId });
    }

    [PunRPC]
    private void KillThis(int id, int hitterId)
    {
        if (id != photonView.ViewID) return;

        if (photonView.IsMine && !_isDisabled)
        {
            OnDeath?.Invoke(hitterId, this);
            _isDisabled = true;
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    private void DeathEffect()
    {
        if (_deathEffect == null)
            _deathEffect = Instantiate(deathEffect);

        _deathEffect.transform.position = transform.position;
        _deathEffect.Play();
    }

    public PunGhost SetPosition(Vector3 position)
    {
        transform.position = position;
        return this;
    }

    public PunGhost SetDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        return this;
    }

    public PunGhost UnfreezeRBody()
    {
        _body.useGravity = true;
        _body.constraints = RigidbodyConstraints.None;
        return this;
    }
}
