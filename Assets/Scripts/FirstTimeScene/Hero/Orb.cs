using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        damage = gameObject.GetComponentInParent<OrbSkill>().damage.RealPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBehaviour>().TakeDamage(damage);
        }
    }
}
