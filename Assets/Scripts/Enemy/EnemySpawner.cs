using System.Collections;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Events;

public class EnemySpawner : Singleton<EnemySpawner>
{
    //[Range(0, 100)]
    //private float _range = 100.0f;
    [SerializeField]
    private float _minDistance = 50.0f;
    [SerializeField]
    private float _maxDistance = 70.0f;
    [SerializeField]
    float _timeBetweenSpawn = 0.2f;
    [SerializeField] private int currentWaveIndex;
    private float startSpawnTimer = 0;
    [SerializeField]
    private MyPooler.ObjectPooler _pool;
    [SerializeField]
    private EnemiesController _controller;
    //[SerializeField]
    private Vector3 _playerPos;
    [SerializeField]
    private GameObject _player;
    private Transform _playerTransform;
    //bool isBossSpawned;
    [SerializeField]
    private List<GameObject> _enemiesEnable = new List<GameObject>();
    [SerializeField]
    private List<EnemyBehavior> _enemyBehaviours = new List<EnemyBehavior>();

    private int _maxDamage = 2000;
    private float _radiusKill = 70f;

    [SerializeField]
    private int _limitEnemies = 150;

    [SerializeField]
    private float _timeSortEnemyList = 5f;
    
    [SerializeField]private int _enemiesKilled = 0;
    public int EnemiesKilled
    {
        get => _enemiesKilled;
    }
    //[SerializeField]
    //private Dictionary<int, int> waveInSpawn = new Dictionary<int, int>();

    public Transform PlayerTransform => _playerTransform;

    public List<GameObject> EnemiesEnable { get => _enemiesEnable; private set => _enemiesEnable = value; }
    public List<EnemyBehavior> EnemyBehaviours { get => _enemyBehaviours; set => _enemyBehaviours = value; }
    private void Awake()
    {
        _player = GameManager.Instance.player;
        _playerTransform = GameManager.Instance.playerTransform;
        MessageBroker.Default.Receive<BossDeadEvent>().Subscribe(bossDeadEvent => { EndChapter(); }).AddTo(gameObject);
        MessageBroker.Default.Receive<OnReviveEvent>().Subscribe(reviveEvent => { KillEnemyOnRevive(); }).AddTo(gameObject);
        MessageBroker.Default.Receive<TickEvent>().Where(tickEvent => tickEvent.secondNumber % _timeSortEnemyList == 0)
            .Subscribe(tickEvent => ReturnClosestObject()).AddTo(gameObject);
    }
    public void StartToSpawn()
    {
        _pool = LevelManager.Instance.GetObjectPoolerCurrentChapter;
        _pool.gameObject.SetActive(true);
        StartChapter();
        //_controller.StartEnemiesController();
    }
    public void ReturnClosestObject()
    {
        //Debug.Log("sort");
        EnemyBehaviours = EnemyBehaviours.OrderBy((EnemiesEnable) => EnemiesEnable.Distance
        /*Vector3.Distance(EnemiesEnable.transformEnemy.position, GameManager.Instance.playerTransform.position)*/).ToList();
        List<EnemyBehavior> listEnemiesFar = EnemyBehaviours.FindAll(((EnemiesEnable) => EnemiesEnable.Distance > _maxDistance * _maxDistance)).ToList();
        foreach (EnemyBehavior item in listEnemiesFar)
        {
            item.transformEnemy.position = CheckInRange.GetPosOnRing(GameManager.Instance.playerTransform.position, _minDistance, _maxDistance);
        }
    }
    private int _totalEnemiesInChapter;
    [SerializeField]
    private int _enemiesSpawnedInChapter;
    public int TotalEnemiesInChapter => _totalEnemiesInChapter;    
    private int _totalWavesInChapter;
    [SerializeField]
    private int _wavesSpawnedInChapter;
    public int TotalWavesInChapter => _totalWavesInChapter;
    void StartChapter()
    {
        _wavesSpawnedInChapter = 0;
        _enemiesSpawnedInChapter = 0;
        _totalEnemiesInChapter = LevelManager.Instance.GetTotalEnemiesInChapter();
        _totalWavesInChapter = LevelManager.Instance.GetTotalWavesInChapter();
        //Debug.Log("Total: " + _totalEnemiesInChapter);
        currentWaveIndex = 0;
        EnemyWave lastWave = LevelManager.Instance.GetCurrentWaveInChapter(LevelManager.Instance.GetWaveCount - 1);
        float timeSpawnBoss = (lastWave.timeBetweenWave * (lastWave.waveTotal - 1)) + lastWave.timeForSpawnWave;
        /*MessageBroker.Default.Publish(new TimeSpawnBossEvent() { timeSpawnBoss = (int)timeSpawnBoss });*/
        //Debug.Log("Wave Count : " + LevelManager.Instance.GetWaveCount);
        while (currentWaveIndex < LevelManager.Instance.GetWaveCount)
        {
            EnemyWave currentWave = LevelManager.Instance.GetCurrentWaveInChapter(currentWaveIndex);
            startSpawnTimer = currentWave.timeForSpawnWave;
            //yield return new WaitForSeconds(currentWave.timeForSpawnWave);
            MessageBroker.Default.Publish(new WaveStartEvent(currentWaveIndex, currentWave));
            for (int waveCount = 0; waveCount < currentWave.waveTotal; waveCount++)
            {
                StartCoroutine(SpawnWave(currentWave, startSpawnTimer, timeSpawnBoss));
                //Debug.Log("Current Wave: " + currentWave.idWave + "Count: " + waveCount);
                startSpawnTimer += currentWave.timeBetweenWave;
            }
            currentWaveIndex++;
        }
    }
    IEnumerator SpawnWave(EnemyWave enemyWave, float timer, float timeSpawnBoss)
    {
        yield return new WaitForSeconds(timer);
        //Debug.Log(enemyWave.enemyTypeInWaves+ " " + timer);
        _wavesSpawnedInChapter++;
        //Debug.Log("Wave ID : " + enemyWave.idWave + "Timer :" + timer);
        MessageBroker.Default.Publish(new CurrentWave{curWave = /*enemyWave.idWave*/_wavesSpawnedInChapter});
        if (/*enemyWave.idWave >= LevelManager.Instance.GetWaveCount && timer >= (timeSpawnBoss)*/ _wavesSpawnedInChapter >= TotalWavesInChapter)
        {
            //Debug.Log("Time Spawn Boss : " + timeSpawnBoss);
            yield return new WaitForSeconds(1f);
            MessageBroker.Default.Publish(new BossComeEvent());
            SpawnBoss();
        }
        for (int enemyCount = 0; enemyCount < enemyWave.numberPerWave; enemyCount++)
        {
            yield return new WaitUntil(() => EnemiesEnable.Count < _limitEnemies);
            SpawnWaveEnemy(enemyWave);
            _enemiesSpawnedInChapter++;
            yield return new WaitForSeconds(_timeBetweenSpawn);
        }
        //Debug.Log("Wave ID : " + enemyWave.idWave + "Timer :" + timer);
        //float timeSpawnBoss = (enemyWave.timeBetweenWave * (enemyWave.waveTotal - 1)) + enemyWave.timeForSpawnWave;
    }
    
    void SpawnBoss()
    {
        string bossName = LevelManager.Instance.GetBossInChapter().name;
        Spawn(bossName);
        currentWaveIndex = 0;
    }
    void SpawnWaveEnemy(EnemyWave currentWave)
    {
        string enemyType = _pool.pools[currentWave.GetIdEnemyInWave(currentWaveIndex)].tag;
        Spawn(enemyType);
    }
    void Spawn(string name)
    {
        _playerPos = _player.transform.position;
        var enemy = _pool.GetFromPool(name, CheckInRange.GetPosOnRing(_playerPos, _minDistance, _maxDistance), Quaternion.identity);
        if(enemy != null)
        {
            EnemiesEnable.Add(enemy);
            EnemyBehaviours.Add(enemy.GetComponent<EnemyBehavior>());
        }
    }
    public void ReturnToPool(GameObject enemy)
    {
        int index = enemy.name.IndexOf("(Clone)");
        string enemyTagInPool = enemy.name.Substring(0, index);
        EnemiesEnable.Remove(enemy);
        EnemyBehaviours.Remove(enemy.GetComponent<EnemyBehavior>());
        _pool.ReturnToPool(enemyTagInPool, enemy);
        _enemiesKilled++;
        //if (GameManager.Instance.State == GameState.Playing) numberEnemyLeft--;
        //if (numberEnemyLeft == 0 && GameManager.Instance.State != GameState.SelectToPlay)
        //{
        //    Debug.Log("Wait for boss Dead !");

        //    //EndChapter();
        //}
    }
    void EndChapter()
    {
        //Debug.Log("End Chapter !");
        LevelManager.Instance.IncreaseChapter();
        GameManager.Instance.UpdateGameState(GameState.Victory);
    }
    void KillEnemyOnRevive()
    {
        CheckTakeDamage(CheckInRange.CheckEnemyNotBossInRange(_radiusKill), _maxDamage);
    }

    public void CheckTakeDamage(List<EnemyBehavior> enemyBehaviours, int damage)
    {
        foreach (EnemyBehavior enemyBehavior in enemyBehaviours)
        {
            enemyBehavior.TakeDamage(damage);
        }
    }
    public void ClearListEnemyAndStopSpawnAndResetEnemiesKilled()
    {
        _enemiesKilled = 0;
        EnemiesEnable.Clear();
        EnemyBehaviours.Clear();
        StopAllCoroutines();
    }
}
