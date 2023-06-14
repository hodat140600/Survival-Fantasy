using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PowerUpType { none, Shield, FireBall, Smash }
public class CheckInRange : MonoBehaviour
{
    public static string ENEMY_TAG = "Enemy";
    public static string EXP_TAG = "Exp";
    public static float _minDistance = 0f;
    public static float _maxDistance = 20f;
    public static List<EnemyBehavior> CheckEnemyNotBossInRange(float radius)
    {
        List<EnemyBehavior> list = new List<EnemyBehavior>();
        foreach(EnemyBehavior item in EnemySpawner.Instance.EnemyBehaviours)
        {
            if(UpdateDistance(item.transform, GameManager.Instance.playerTransform.position) <= radius*radius && !item.isBoss)
            {
                list.Add(item);
            }
        }
        return list;
    }
    public static List<EnemyBehavior> CheckEnemyInRange(float radius)
    {
        List<EnemyBehavior> list = new List<EnemyBehavior>();
        foreach (EnemyBehavior item in EnemySpawner.Instance.EnemyBehaviours)
        {
            if (UpdateDistance(item.transform, GameManager.Instance.playerTransform.position) <= radius*radius)
            {
                list.Add(item);
            }
        }
        return list;
    }
    private static float UpdateDistance(Transform enemy, Vector3 targetPosition)
    {
        float distance;
        var tempPosition = enemy.position;
        float xD = targetPosition.x - tempPosition.x;
        float zD = targetPosition.z - tempPosition.z;
        distance = xD * xD + zD * zD;
        return distance;
    }
    public static Transform GetClosestTarget(List<GameObject> enemyGameobjects, Transform startTrans)
    {
        //StartCoroutine(WaitForEnemySpawn(enemyGameobjects));
        float _shortestDistance = Mathf.Infinity;
        Transform target = null;
        foreach (var enemy in enemyGameobjects)
        {
            float distanceToEnemy = Distance(startTrans.position, enemy.transform.position);
            if (distanceToEnemy < _shortestDistance /*&& distanceToEnemy > 1f*/)
            {
                _shortestDistance = distanceToEnemy;
                target = enemy.transform;
            }
        }
        return target;
    }

    private static float Distance(Vector3 vectorA, Vector3 vectorB) //only vectorA.y==vectorB.y
    {
        return (vectorA.x - vectorB.x)*(vectorA.x-vectorB.x) + (vectorA.z-vectorB.z)*(vectorA.z-vectorB.z);
    }

    public static Vector3 GetClosestTargetVector(List<GameObject> enemyGameobjects, Transform startTrans)
    {
        if (enemyGameobjects.Count <= 0)
        {
            return GetPosOnRing(startTrans.position, _minDistance, _maxDistance);
        }
        //StartCoroutine(WaitForEnemySpawn(enemyGameobjects));
        float _shortestDistance = Mathf.Infinity;
        Transform target = null;
        foreach (var enemy in enemyGameobjects)
        {
            float distanceToEnemy = Vector3.Distance(startTrans.position, enemy.transform.position);
            if (distanceToEnemy < _shortestDistance /*&& distanceToEnemy > 1f*/)
            {
                _shortestDistance = distanceToEnemy;
                target = enemy.transform;
            }
        }
        return target.position;
    }

    IEnumerator WaitForEnemySpawn(List<GameObject> enemyGameobjects)
    {
        yield return new WaitUntil(() => enemyGameobjects != null);
    }

    public static Transform UpdateTarget(List<Transform> enemyTransforms, Transform startTrans)
    {
        float _shortestDistance = Mathf.Infinity;
        Transform target = null;
        //Debug.Log("enemyTransforms" + enemyTransforms.Count);
        foreach (var enemy in enemyTransforms)
        {
            if (enemy && enemy.gameObject && enemy.gameObject.activeInHierarchy)
            {
                float distanceToEnemy = Vector3.Distance(startTrans.position, enemy.transform.position);
                if (distanceToEnemy < _shortestDistance /*&& distanceToEnemy > 1f*/)
                {
                    _shortestDistance = distanceToEnemy;
                    target = enemy;
                }
            }
        }

        if (target == null)
        {
            Debug.LogError("Target");
        }
        return target;
    }

    public static Vector3 RandomPosition(Vector2 origin, float minRadius, float maxRadius)
    {
        var randomDirection = (Random.insideUnitCircle * origin).normalized;

        var randomDistance = Random.Range(minRadius, maxRadius);

        var point = origin + randomDirection * randomDistance;

        Vector3 pos = new Vector3(point.x, 0, point.y);
        return pos;
    }
    public static Vector3 ToXZ(Vector2 aVec)
    {
        return new Vector3(aVec.x, 0, aVec.y);
    }

    public static Vector3 GetPosOnRing(Vector3 origin, float minRadius, float maxRadius)
    {
        var v = Random.insideUnitCircle;
        return ToXZ(v.normalized * minRadius + v * (maxRadius - minRadius)) + origin;

    }

}
