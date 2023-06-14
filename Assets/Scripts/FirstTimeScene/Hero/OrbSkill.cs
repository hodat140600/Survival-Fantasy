using Assets.Scripts.Skills.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSkill : MonoBehaviour
{
    float timeCounter = 0;
    public GameObject player;
    public Vector3 offset;
    public List<Transform> transforms;
    public Speed speed;
    public Damage damage;
    public NumberProjectiles numberProjectiles;
    public Radius radius;
    float ROTATION = 360f;
    private void FixedUpdate()
    {
        offset = player.transform.position;
        offset.y = 1;
        timeCounter += (Time.deltaTime * speed.RealPoint);
        CircularMovementByNumberProjectile();
    }
    void CircleMove(Transform transformObj, float angle)
    {
        transformObj.position = new Vector3(Mathf.Cos(timeCounter + angle) * radius.RealPoint
            , 0, Mathf.Sin(timeCounter + angle) * radius.RealPoint) + offset;
    }
    int projectiles;
    float _radius;
    void CircularMovementByNumberProjectile()
    {
        // return in limit projectile
        projectiles = numberProjectiles.RealPoint > transforms.Count ? transforms.Count : numberProjectiles.RealPoint;
        _radius = radius.RealPoint * /*(1- (projectiles/10))*/0.6f;
        float degreeEachProjectile = (2 * Mathf.PI) / projectiles;
        for (int i = 0; i < transforms.Count; i++)
        {
            CircleMove(transforms[i], degreeEachProjectile * (i + 1));
            transforms[i].gameObject.SetActive(i < projectiles);
            transforms[i].Rotate(0, ROTATION * Time.deltaTime, 0);
        }
    }
}
