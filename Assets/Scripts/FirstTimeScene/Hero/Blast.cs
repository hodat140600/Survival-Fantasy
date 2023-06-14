using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
	public Rigidbody rigidbody;

	public AudioSource audioSource;

	public const string TAG = "Blast";

	private float _timeDisable = 5f;

	public int damage;

	public int explodeRadius;

	private void Reset()
	{
		rigidbody = GetComponent<Rigidbody>();
	}
	Transform hitTarget = null;
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			hitTarget = other.transform;
			//audioSource.Play();
			other.GetComponent<EnemyBehaviour>().TakeDamage(damage);
			AudioSource.PlayClipAtPoint(audioSource.clip, hitTarget.position);
			HitImpactSpawner.Instance.SpawnHitImpact(TAG, hitTarget.position, hitTarget.rotation);
			ProjectilesSpawner.Instance.ReturnPoolProjectile(this.gameObject, TAG);
		}
	}
	void Explode()
	{
		for (int index = 0; index < EnemiesBehaviourController.Instance.EnemySpawner.EnemyBehaviours.Count; index++)
		{
			if (Vector3.Distance(this.transform.position, EnemiesBehaviourController.Instance.EnemySpawner.EnemyBehaviours[index].transformEnemy.position) <= explodeRadius)
			{
				//Debug.Log("Hit");
				EnemiesBehaviourController.Instance.EnemySpawner.EnemyBehaviours[index].TakeDamage(damage);
				HitImpactSpawner.Instance.SpawnHitImpact(TAG, 
					EnemiesBehaviourController.Instance.EnemySpawner.EnemyBehaviours[index].transformEnemy.position, 
					EnemiesBehaviourController.Instance.EnemySpawner.EnemyBehaviours[index].transformEnemy.rotation);
			}
		}
	}
	public void Force(float force, Vector3 direction)
	{
		rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
	}

	void DisableProjectile()
	{
		ProjectilesSpawner.Instance.ReturnPoolProjectile(gameObject, TAG);
	}

	private void OnEnable()
	{
		rigidbody.velocity = Vector3.zero;
		hitTarget = null;
		Invoke(nameof(DisableProjectile), _timeDisable);
	}
	private void OnDisable()
	{
		CancelInvoke();
	}
}
