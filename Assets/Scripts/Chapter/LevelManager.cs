using Assets.Scripts.Skills.LiveSkill;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
// TODO: move qua folder Chapter
public class LevelManager : Singleton<LevelManager>
{
    public List<ChapterSettings> ChapterList;
    public List<MyPooler.ObjectPooler> ObjectPoolers;
    public GameObject road;
    public GenerateTileMap generateTileMap;
    [SerializeField]
    private int currentChapter;
    int amountEnenmy = 100;
    int enenmyExtensionLimit = 500;
    int amountBoss = 1;
    int bossExtensionLimit = 1;
    [SerializeField]
    private List<Transform> enemyTransforms = new List<Transform>();

    public int CurrentChapter { get => currentChapter; set => currentChapter = value; }
    public int IndexCurrentChap { get => currentChapter - 1; }
    public int EnenmyTypeCount { get => ChapterList[IndexCurrentChap].Spawn.enemies.Count; }
    public Map MapCurrentChap { get => ChapterList[IndexCurrentChap].Map; }
    public int GetWaveCount
    {
        get
        {
            return this.ChapterList[IndexCurrentChap].Spawn.waves.Count;
        }
    }
    public MyPooler.ObjectPooler GetObjectPoolerCurrentChapter
    {
        get {
            //always get first item in list
            return ObjectPoolers[0]; 
        }
    }

    public List<Transform> EnemyTransforms { get => enemyTransforms;private set => enemyTransforms = value; }

    public void IncreaseChapter()
    {
        CurrentChapter++;
        CurrentChapter = CurrentChapter > ChapterList.Count ? ChapterList.Count - 1 : CurrentChapter; 
        GameManager.Instance.playerSettings.lastWinChapter = CurrentChapter;
    }

    public GameObject GetEnemyInChapter(int enemyType, int indexChap)
    {
        this.ChapterList[indexChap].Spawn.InitStatusEnemy();
        return this.ChapterList[indexChap].Spawn.enemies[enemyType].enemy;
    }
    public GameObject GetBossInChapter()
    {
        this.ChapterList[IndexCurrentChap]
            .Spawn
            .boss
            .enemy
            .GetComponent<EnemyVisitor>()
            .enemyStatus = this.ChapterList[IndexCurrentChap]
            .Spawn
            .boss
            .enemyStatus;
        return this.ChapterList[IndexCurrentChap].Spawn.boss.enemy;
    }  public GameObject GetBossInChapter(int indexChap)
    {
        this.ChapterList[indexChap]
            .Spawn
            .boss
            .enemy
            .GetComponent<EnemyVisitor>()
            .enemyStatus = this.ChapterList[IndexCurrentChap]
            .Spawn
            .boss
            .enemyStatus;
        return this.ChapterList[indexChap].Spawn.boss.enemy;
    }
    public EnemyWave GetCurrentWaveInChapter(int currentWaveIndex)
    {
        return this.ChapterList[IndexCurrentChap].Spawn.waves[currentWaveIndex];
    }

    public int GetTotalEnemiesInChapter()
    {
        int total = 0;
        for(int indexWave = 0; indexWave < ChapterList[IndexCurrentChap].Spawn.waves.Count; indexWave++)
        {
            total += ChapterList[IndexCurrentChap].Spawn.waves[indexWave].waveTotal * ChapterList[IndexCurrentChap].Spawn.waves[indexWave].numberPerWave;
        }
        return total;
    }
    public int GetTotalWavesInChapter()
    {
        int total = 0;
        for (int indexWave = 0; indexWave < ChapterList[IndexCurrentChap].Spawn.waves.Count; indexWave++)
        {
            total += ChapterList[IndexCurrentChap].Spawn.waves[indexWave].waveTotal;
        }
        return total;
    }
    private void Awake()
    {
        MessageBroker.Default.Receive<ChapterStartEvent>().Subscribe(x => { CreatePoolChapter(CurrentChapter); }).AddTo(gameObject);
        generateTileMap = GetComponent<GenerateTileMap>();
    }
    void CreatePoolChapter(int currentChapter)
    {
        var chapterPool = new GameObject("PoolChapter" + currentChapter);
        chapterPool.transform.SetParent(this.transform);
        chapterPool.SetActive(false);
        ObjectPoolers.Add(chapterPool.AddComponent<MyPooler.ObjectPooler>());
        LoadEnemyPool(IndexCurrentChap, chapterPool.GetComponent<MyPooler.ObjectPooler>());
        //Check if have only 1 item else delete the second
        if (ObjectPoolers.Count > 1)
        {
            DestroyObject(ObjectPoolers[0].gameObject);
            ObjectPoolers.RemoveAt(0);
        }
    }
    void LoadEnemyPool(int indexChap, MyPooler.ObjectPooler pooler)
    {
        int i = 0;
        pooler.pools = new List<MyPooler.ObjectPooler.Pool>();
        for (i = 0; i < EnenmyTypeCount; i++)
        {
            pooler.pools.Add(new MyPooler.ObjectPooler.Pool());
            CreatePoolDictionary(GetEnemyInChapter(i, indexChap), i, amountEnenmy, enenmyExtensionLimit, pooler);
        }
        pooler.pools.Add(new MyPooler.ObjectPooler.Pool());
        CreatePoolDictionary(GetBossInChapter(indexChap), i, amountBoss, bossExtensionLimit, pooler);
    }

    [SerializeField] private GameObject player;
    public void LoadLevel()
    {
        foreach (var item in ObjectPoolers)
        {
            item.gameObject.SetActive(false);
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

    public void AddAllEnemy()
    {
        Transform poolTrans = transform.GetChild(1);
        int childCount = poolTrans.childCount;
        //Transform tst = poolTrans.GetChild(0);
        for(int childIndex = 0; childIndex < childCount - 1; childIndex++)
        {
            foreach(Transform child in poolTrans.GetChild(childIndex))
            {
                EnemyTransforms.Add(child);
            }
        }
    }
}
