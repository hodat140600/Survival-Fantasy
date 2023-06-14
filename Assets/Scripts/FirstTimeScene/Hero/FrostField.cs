using Assets.Scripts.Skills.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostField : MonoBehaviour
{
    public GameObject objectSkill;
    public Damage damage;
    public Radius radius;
    private void Update()
    {
        objectSkill.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    }
    public float Radius => radius.RealPoint *2f;
    public int Damage => damage.RealPoint;
    private void Start()
    {
        transform.localScale = (Vector3.one * Radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBehaviour>().TakeDamage(Damage);
            //StartCoroutine(OnDamagePerSecond(Damage, other.GetComponent<EnemyBehavior>()));
        }
    }
}
