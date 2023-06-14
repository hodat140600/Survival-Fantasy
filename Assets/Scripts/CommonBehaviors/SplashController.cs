using Assets.Scripts.Skills.SplashSkill;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SplashController : MonoBehaviour
{
	[Header("Use Splashs (default)")]
	public GameObject splashPrefab;
	public float fireRate = 1f;
	public List<Transform> firePoints;
	private float fireCountdown = 0f;
	[SerializeField]GameObject player;
	[SerializeField] AudioSource _audioSource;
	SplashSkillBehavior _splashSkillBehavior;
 //   private void Update()
 //   {
	//	transform.position = player.transform.position;
	//}
	void CheckNumSplash()
    {
		int projectiles = _splashSkillBehavior.numberProjectiles.RealPoint > firePoints.Count ? firePoints.Count : _splashSkillBehavior.numberProjectiles.RealPoint;
		float degreeEachProjectile = (2 * Mathf.PI) / projectiles;
		for (int i = 0; i < firePoints.Count; i++)
		{
			firePoints[i].gameObject.SetActive(i < projectiles);
			CirclePosition(firePoints[i], degreeEachProjectile *(i+1));
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
			MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(SplashSkillSettings), _splashSkillBehavior.cooldown.RealPoint));
			yield return new WaitForSeconds(_splashSkillBehavior.cooldown.RealPoint);
			CheckNumSplash();
			for (int i = 0; i < firePoints.Count; i++)
			{
				if (firePoints[i].gameObject.activeInHierarchy)
				{
					_audioSource.Play();
					GameObject splashGO = ProjectilesSpawner.Instance.SpawnProjectile(splashPrefab.name, firePoints[i].position, firePoints[i].rotation);
					SplashBehavior bullet = splashGO.GetComponent<SplashBehavior>();
					if (bullet != null)
					{
						bullet.damage = _splashSkillBehavior.damage.RealPoint;
						bullet.passThrough = _splashSkillBehavior.passThrough;
						bullet.Force(_splashSkillBehavior.flySpeed, firePoints[i].forward);
					}
				}
			}
		}
	}
	public void Init(SplashSkillBehavior splashSkillBehavior)
	{
		_splashSkillBehavior = splashSkillBehavior;
		this.gameObject.SetActive(true);
		StopAllCoroutines();
		StartCoroutine(Shoot());
	}
}
