using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBulletBehaviour : MonoBehaviour
{
    public int damage = 50;

    public Rigidbody rigidbody;

    public const string TAG = "Target";

    private float _timeDisable = 5f;

    private void Reset()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Damage(other.transform);
            ProjectilesSpawner.Instance.ReturnPoolProjectile(this.gameObject, TAG);
        }
    }
    void Damage(Transform enemy)
    {
        EnemyBehavior e = enemy.GetComponent<EnemyBehavior>();

        if (e != null)
        {
            e.TakeDamage(damage);
            HitImpactSpawner.Instance.SpawnHitImpact(TAG, e.transform.position, e.transform.rotation);
        }
    }
    public void Force(float force, Vector3 direction)
    {
        rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
        //transform.LookAt(dirSplash.forward);
    }
    void DisableProjectile()
    {
        ProjectilesSpawner.Instance.ReturnPoolProjectile(gameObject, TAG);
    }

    private void OnEnable()
    {
        rigidbody.velocity = Vector3.zero;
        Invoke(nameof(DisableProjectile), _timeDisable);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
