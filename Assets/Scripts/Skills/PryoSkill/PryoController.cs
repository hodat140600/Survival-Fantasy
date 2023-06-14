using Assets.Scripts.Skills;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PryoController : MonoBehaviour
{
	[Header("Use Pryo (default)")]
	public GameObject pryoPrefab;
	public float fireRate = 1f;
	public List<Transform> firePoints;
	private float fireCountdown = 0f;
	[SerializeField] GameObject player;
	[SerializeField] AudioSource _audioSource;
	PryoSkillBehaviour _pryoSkillBehavior;
	[SerializeField]
	private float _degreeEachProjectile => _pryoSkillBehavior.angle;
	public int Projectile => _pryoSkillBehavior.numberProjectiles.RealPoint;

	void CheckNumPryo()
	{
		int projectiles = Projectile > firePoints.Count ? firePoints.Count :Projectile;
		int eachSideProjectile = projectiles / 2;
		//bool parityProjectile = projectiles % 2 == 0;
		//projectiles = parityProjectile ? projectiles + 1 : projectiles;
		//Debug.Log("Parity = " + parityProjectile);
		//Debug.Log("Projectile = " + projectiles);
		for (int i = 0; i < firePoints.Count; i++)
		{
			int coefficient = (i)%2 != 0 ? -1 * ((i + 1) / 2) : 1 * ((i + 1) / 2);
			//Debug.Log("Coefficient: " + coefficient);                                
			//Debug.Log("Angle: " + _degreeEachProjectile * eachSideProjectile * (coefficient));
			firePoints[i].gameObject.SetActive(i < projectiles);
			FanPosition(firePoints[i], /*(-projectiles / 2) **/ _degreeEachProjectile * eachSideProjectile * (coefficient));
		}
		//if(parityProjectile) firePoints[0].gameObject.SetActive(false);
	}

	void FanPosition(Transform transformObj, float angle)
	{
		//Debug.Log("Rotation: " + Quaternion.AngleAxis(angle, GameManager.Instance.playerTransform.up) * GameManager.Instance.playerTransform.rotation);
		//Debug.Log("Transform : " + GameManager.Instance.playerTransform.forward);
		var qAngle = Quaternion.AngleAxis(angle, GameManager.Instance.playerTransform.up) * GameManager.Instance.playerTransform.rotation;
		var qDelta = Quaternion.AngleAxis(angle, GameManager.Instance.playerTransform.up);
		//transformObj.rotation = Quaternion.AngleAxis(angle, GameManager.Instance.playerTransform.forward);
		transformObj.rotation = qAngle;
		//transformObj.eulerAngles = new Vector3(0, angle , 0);
	}
	IEnumerator Shoot()
	{
		while (isActiveAndEnabled)
		{
			MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(PryoSkillSettings), _pryoSkillBehavior.cooldown.RealPoint));
			yield return new WaitForSeconds(_pryoSkillBehavior.cooldown.RealPoint);
			CheckNumPryo();
			for (int i = 0; i < firePoints.Count; i++)
			{
				if (firePoints[i].gameObject.activeInHierarchy)
				{
					_audioSource.Play();
					GameObject pryoGO = ProjectilesSpawner.Instance.SpawnProjectile(pryoPrefab.name, firePoints[i].position, firePoints[i].rotation);
					PryoBehaviour bullet = pryoGO.GetComponent<PryoBehaviour>();
					if (bullet != null)
					{
						bullet.damage = _pryoSkillBehavior.damage.RealPoint;
						bullet.Force(_pryoSkillBehavior.flySpeed, firePoints[i].forward);
					}
				}
			}
		}
	}
	public void Init(PryoSkillBehaviour pryoSkillBehavior)
	{
		_pryoSkillBehavior = pryoSkillBehavior;
		this.gameObject.SetActive(true);
		StopAllCoroutines();
		StartCoroutine(Shoot());
	}
}
