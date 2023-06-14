using UnityEngine;

public class BlastBehaviour : MonoBehaviour
{
	private int _damage = 50;

	public Rigidbody rigidbody;

	public AudioSource audioSource;

	public const string TAG = "Blast";

	private float _explosionRadius = 0f;

	private float _timeDisable = 5f;

    public int Damage { get => _damage; set => _damage = value; }
    public float ExplosionRadius { get => _explosionRadius; set => _explosionRadius = value; }

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
			Explode();
			//audioSource.Play();
			AudioSource.PlayClipAtPoint(audioSource.clip, hitTarget.position);
			HitImpactSpawner.Instance.SpawnHitImpact(TAG, hitTarget.position, hitTarget.rotation);
			ProjectilesSpawner.Instance.ReturnPoolProjectile(this.gameObject, TAG);
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
	void Explode()
	{
		for (int index = 0; index < EnemySpawner.Instance.EnemyBehaviours.Count; index++)
		{
			if (Vector3.Distance(this.transform.position, EnemySpawner.Instance.EnemyBehaviours[index].transformEnemy.position) <= ExplosionRadius)
			{
				//Debug.Log("Hit");
				EnemySpawner.Instance.EnemyBehaviours[index].TakeDamage(Damage);
			}
		}
	}
}
