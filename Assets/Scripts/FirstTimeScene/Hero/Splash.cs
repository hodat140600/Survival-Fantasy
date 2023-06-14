using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    private Transform dirSplash;
    public float speed = 70f;
    public int damage = 50;
    public float timeSplashFly = 3f;
    public int passThrough = 4;
    [SerializeField]
    private int _passThroughCount;
    private float _timeDisable = 2f;
    public Rigidbody rigidbody;

    private void Reset()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void DisableProjectile()
    {
        ProjectilesSpawner.Instance.ReturnPoolProjectile(gameObject, "Splash");
    }


    private void OnEnable()
    {
        rigidbody.velocity = Vector3.zero;
        _passThroughCount = 0;
        Invoke(nameof(DisableProjectile), _timeDisable);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBehaviour e = other.GetComponent<EnemyBehaviour>();
        if (e != null)
        {
            e.TakeDamage(damage);
            HitImpactSpawner.Instance.SpawnHitImpact("Splash", e.transform.position, Quaternion.identity);
            _passThroughCount++;
        }
        if (_passThroughCount >= passThrough) DisableProjectile();
    }

    public void Force(float force, Vector3 direction)
    {
        rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
    }
}
