using System.Collections;
//using UniRx;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTransform;    
    [SerializeField]
    private EnemySpawner _enemySpawner;

    private void Start()
    {
        _playerTransform = EnemySpawner.Instance.PlayerTransform;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State != GameState.Playing && GameManager.Instance.State != GameState.Lose) return;
        for (int index = 0; index < EnemySpawner.Instance.EnemyBehaviours.Count; index++)
        {
            _enemySpawner.EnemyBehaviours[index].MoveToPositionWithRotation(_playerTransform.position, Time.fixedDeltaTime);
        }
    }
}
