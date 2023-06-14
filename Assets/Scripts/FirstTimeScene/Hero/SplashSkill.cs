using Assets.Scripts.Skills.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashSkill : MonoBehaviour
{
	public GameObject splashPrefab;
	public GameObject SplashHolder;
	public float fireRate = 1f;
	public List<Transform> firePoints;
	private float fireCountdown = 0f;
	[SerializeField] GameObject player;
	[SerializeField] AudioSource _audioSource;
	public Speed speed;
	public Damage damage;
	public NumberProjectiles numberProjectiles;
	public Radius radius;
	public Cooldown cooldown;
	public float flySpeed;

    private void Start()
    {
		StartCoroutine(Shoot());
    }
    void CheckNumSplash()
	{
		int projectiles = numberProjectiles.RealPoint > firePoints.Count ? firePoints.Count : numberProjectiles.RealPoint;
		float degreeEachProjectile = (2 * Mathf.PI) / projectiles;
		for (int i = 0; i < firePoints.Count; i++)
		{
			firePoints[i].gameObject.SetActive(i < projectiles);
			CirclePosition(firePoints[i], degreeEachProjectile * (i + 1));
		}
	}

	void CirclePosition(Transform transformObj, float angle)
	{
		transformObj.eulerAngles = new Vector3(0, Mathf.Rad2Deg * angle, 0) /*+ GameManager.Instance.playerTransform.position*/;
	}
	IEnumerator Shoot()
	{
		while (isActiveAndEnabled)
		{
			yield return new WaitForSeconds(cooldown.RealPoint);
			SplashHolder.transform.position = transform.position;
			CheckNumSplash();
			for (int i = 0; i < firePoints.Count; i++)
			{
				if (firePoints[i].gameObject.activeInHierarchy)
				{
					_audioSource.Play();
					GameObject splashGO = ProjectilesSpawner.Instance.SpawnProjectile(splashPrefab.name, firePoints[i].position, firePoints[i].rotation);
					Splash bullet = splashGO.GetComponent<Splash>();
					if (bullet != null)
					{
						bullet.damage = damage.RealPoint;
						bullet.Force(flySpeed, firePoints[i].forward);
					}
				}
			}
		}
	}
}
