using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ghost : MonoBehaviour
{
    public event Action<Ghost> OnDisable;
    public event Action<Ghost> OnDeath;

    public ParticleSystem hitEffect;
    public ParticleSystem deathEffect;

    [SerializeField] private float _speed = 5f;
    private Rigidbody _body;
    private ParticleSystem _hitEffect = null;
    private ParticleSystem _deathEffect = null;

    private bool _canMove;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
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
        if (obj.TryGetComponent(out Bullet bullet))
        {
            SetMovement(false);
            UnfreezeRBody();

            ContactPoint contactPoint = collision.GetContact(0);
            _body.AddForceAtPosition(GetForce(contactPoint.normal, bullet.Force), contactPoint.point, ForceMode.Impulse);

            if (_hitEffect == null) _hitEffect = Instantiate(hitEffect, transform);
            ShowEffectAt(_hitEffect, contactPoint);

            Invoke("Die", 1f);
        }
    }

    private Vector3 GetForce(Vector3 direction, Vector3 force)
    {
        return direction * (force.magnitude / 2);
    }

    private void OnBecameInvisible()
    {
        //Usar otro metodo
        OnDisable?.Invoke(this);
    }

    private void Die()
    {
        if (_deathEffect == null) _deathEffect = Instantiate(deathEffect);
        ShowEffectAt(_deathEffect, transform.position);

        OnDeath?.Invoke(this);
        OnDisable?.Invoke(this);
    }

    private void ShowEffectAt(ParticleSystem effect, ContactPoint contactPoint)
    {
        Transform fxTransform = effect.transform;
        fxTransform.position = contactPoint.point;
        fxTransform.forward = -contactPoint.normal;

        effect.Play();
    }

    private void ShowEffectAt(ParticleSystem effect, Vector3 position)
    {
        effect.transform.position = position;
        effect.Play();
    }

    public Ghost SetPosition(Vector3 position)
    {
        transform.position = position;
        return this;
    }

    public Ghost SetDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        return this;
    }

    public Ghost FreezeRBody()
    {
        _body.useGravity = false;
        _body.constraints = RigidbodyConstraints.FreezeAll;
        return this;
    }

    public Ghost UnfreezeRBody()
    {
        _body.useGravity = true;
        _body.constraints = RigidbodyConstraints.None;
        return this;
    }

    public Ghost SetMovement(bool canMove)
    {
        _canMove = canMove;
        return this;
    }

    internal static void OnTurnOn(Ghost ghost)
    {
        ghost.gameObject.SetActive(true);
    }

    internal static void OnTurnOff(Ghost ghost)
    {
        ghost.gameObject.SetActive(false);
    }
}
