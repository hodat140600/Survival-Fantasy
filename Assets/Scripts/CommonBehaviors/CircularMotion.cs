using Assets.Scripts.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    float timeCounter = 0;
    OrbsSkillBehavior _orbsSkillBehavior;
    public GameObject player;
    public Vector3 offset;
    public List<Transform> transforms;

    private void Start()
    {
        _orbsSkillBehavior = player.GetComponent<OrbsSkillBehavior>();
    }

    private void FixedUpdate()
    {
        offset = player.transform.position;
        offset.y = 1;
        timeCounter += (Time.deltaTime * _orbsSkillBehavior.speed.RealPoint);
        CircularMovementByNumberProjectile();
    }
    [SerializeField]
    public float radius;
    public float projectiles;
    void CircleMove(Transform transformObj, float angle)
    {
        transformObj.position = new Vector3(Mathf.Cos(timeCounter + angle) * radius
            , 0, Mathf.Sin(timeCounter + angle) * radius) + offset;
    }

    void CircularMovementByNumberProjectile()
    {
        // return in limit projectile
        projectiles = _orbsSkillBehavior.numberProjectiles.RealPoint > transforms.Count ? transforms.Count : _orbsSkillBehavior.numberProjectiles.RealPoint;
        radius = _orbsSkillBehavior.radius.RealPoint * /*(1- (projectiles/10))*/0.6f ;
        float degreeEachProjectile = (2* Mathf.PI) / projectiles;
        for (int i = 0; i < transforms.Count; i++)
        {
            CircleMove(transforms[i], degreeEachProjectile * (i + 1));
            transforms[i].gameObject.SetActive(i < projectiles);
        }
    }

    public void Init(OrbsSkillBehavior orbsSkillBehavior)
    {
        _orbsSkillBehavior = orbsSkillBehavior;
        this.gameObject.SetActive(true);
    }
}
