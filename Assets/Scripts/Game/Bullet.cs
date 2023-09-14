using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public Vector3 Force
    {
        get
        {
            return _body != null ? _body.velocity * _body.mass : Vector3.zero;
        }
    }

    public event Action<Bullet> OnReachedTarget;

    private Rigidbody _body;
    private TrailRenderer _trail;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _trail = GetComponent<TrailRenderer>();
    }

    internal static void OnTurnOn(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    internal static void OnTurnOff(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    public Bullet SetPosition(Vector3 position)
    {
        transform.position = position;
        _trail.Clear();
        return this;
    }

    public Bullet SetVelocity(Vector3 velocity)
    {
        _body.velocity = velocity;
        return this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ReachedTarget();
    }

    private void OnBecameInvisible()
    {
        Invoke("ReachedTarget", .5f);
    }

    private void ReachedTarget()
    {
        OnReachedTarget?.Invoke(this);
    }
}
