using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField]
    private float _minDistance = 50.0f;
    [SerializeField]
    private float _maxDistance = 70.0f;
    [SerializeField]
    float _timeBetweenSpawn = 0.2f;
    [SerializeField]
    private MyPooler.ObjectPooler _pool;
    [SerializeField]
    private EnemiesBehaviourController _controller;
    [SerializeField]
    private GameObject _player;
    private Transform _playerTransform;
    private PlayerController _playerController;
    [SerializeField]
    private List<EnemyBehaviour> _enemyBehaviours = new List<EnemyBehaviour>();
    [SerializeField]
    private List<EnemyBehaviour> _minionBehaviours = new List<EnemyBehaviour>();
    public List<EnemyBehaviour> EnemyBehaviours { get => _enemyBehaviours; set => _enemyBehaviours = value; }
    public List<EnemyBehaviour> MinionBehaviours { get => _minionBehaviours; set => _minionBehaviours = value; }
    public int limitInScene = 150;
    private void Start()
    {
        _controller = EnemiesBehaviourController.Instance;
        _playerController = _controller.PlayerTransform.gameObject.GetComponent<PlayerController>();
        _playerTransform = _controller.PlayerTransform;
    }

    void StopSpawnAndKillAllMinion()
    {
        _playerController.skillHolder.SetActive(false);
        _playerController.novaSkill.SetActive(true);
        StopAllCoroutines();
        var count = MinionBehaviours.Count;
        for (int index = 0; index < count; index++)
        {
            // always remove first index
            if (MinionBehaviours[0] == null) break;
            MinionBehaviours[0].TakeDamage(MinionBehaviours[0].maxHealth);
        }
        _playerController.dialogPanel.SetActive(true);
    }

    public int delayMinion;
    public float delayKill = 6f;
    public void StartSpawnMinionCutscene()
    {
        for(int index = 0; index < TypeMinions.Count; index++)
        {
            StartCoroutine(SpawnWave(TypeMinions[index].name));
        }
        Invoke(nameof(StopSpawnAndKillAllMinion), delayKill);
    }
    WaitForSeconds WaitForSeconds = new WaitForSeconds(0.5f);

    IEnumerator SpawnWave(string name)
    {
        for (int index = 0; index < amountMinion; index++)
        {
            yield return new WaitUntil(() => MinionBehaviours.Count < limitInScene);
            SpawnMinion(name);
            if (index < delayMinion) yield return WaitForSeconds;
        }
    }
    void SpawnMinion(string name)
    {
        var enemy = _pool.GetFromPool(name, CheckInRange.GetPosOnRing(_playerTransform.position, _minDistance, _maxDistance), Quaternion.identity);
        if (enemy != null)
        {
            MinionBehaviours.Add(enemy.GetComponent<EnemyBehaviour>());
        }
    }
    public void ReturnToPool(EnemyBehaviour enemyBehaviour)
    {
        var minion = enemyBehaviour.gameObject;
        int index = minion.name.IndexOf("(Clone)");
        string enemyTagInPool = minion.name.Substring(0, index);
        MinionBehaviours.Remove(enemyBehaviour);
        EnemyBehaviours.Remove(enemyBehaviour);
        _pool.ReturnToPool(enemyTagInPool, minion);
    }

    #region Create Pool Minion
    public List<GameObject> TypeMinions;
    public int amountMinion;
    public int extensionMinion;
    [Button("Create Pool", ButtonSizes.Medium)]
    void CreatePool()
    {
        LoadEnemyPool(_pool);
    }
    void LoadEnemyPool(MyPooler.ObjectPooler pooler)
    {
        int i = 0;
        pooler.pools = new List<MyPooler.ObjectPooler.Pool>();
        for (i = 0; i < TypeMinions.Count; i++)
        {
            pooler.pools.Add(new MyPooler.ObjectPooler.Pool());
            CreatePoolDictionary(TypeMinions[i], i, amountMinion, extensionMinion, pooler);
        }
    }
    void CreatePoolDictionary(GameObject prefab, int typeEnemy, int amount, int extensionLimit, MyPooler.ObjectPooler pooler)
    {
        pooler.pools[typeEnemy].prefab = prefab;
        pooler.pools[typeEnemy].tag = prefab.name;
        pooler.pools[typeEnemy].shouldExpandPool = true;
        pooler.pools[typeEnemy].amount = amount;
        pooler.pools[typeEnemy].extensionLimit = extensionLimit;
    }
    #endregion
}
