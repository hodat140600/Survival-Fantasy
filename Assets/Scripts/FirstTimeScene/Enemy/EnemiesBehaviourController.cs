using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class EnemiesBehaviourController : MonoBehaviour
{
    public static EnemiesBehaviourController Instance
    { get => _instance; private set => _instance = value; }
    private static EnemiesBehaviourController _instance;
    public EnemiesSpawner EnemySpawner { get => _enemySpawner;private set => _enemySpawner = value; }
    public Transform PlayerTransform { get => _playerTransform;private set => _playerTransform = value; }

    public CameraShake cameraShake;

    [SerializeField]
    private Transform _playerTransform;
    private PlayerController _playerController;
    [SerializeField]
    private EnemiesSpawner _enemySpawner;
    public List<GameObject> skillEnemies;
    public bool isBossBehave;
    public bool isMinionBehave;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        _playerController = _playerTransform.gameObject.GetComponent<PlayerController>();
        MessageBroker.Default.Receive<SpawnMinionEvent>().Subscribe(x => { SpawnMinion(); }).AddTo(gameObject);
        MessageBroker.Default.Receive<SummonBossEvent>().Subscribe(x => { _playerController.ResetPositionHero(); }).AddTo(gameObject); 
    }

    #region Minion Controller
    void SpawnMinion()
    {
        _playerController.TogglePausing(false);
        _enemySpawner.StartSpawnMinionCutscene();
        isMinionBehave = true;
        StartCoroutine(MinionController());
    }
    IEnumerator MinionController()
    {
        while (isMinionBehave)
        {
            for (int index = 0; index < EnemySpawner.MinionBehaviours.Count; index++)
            {
                EnemySpawner.MinionBehaviours[index].MoveToPositionWithRotation(PlayerTransform.position, Time.fixedDeltaTime);
            }
            yield return null;
        }
    }
    #endregion

    #region Boss Controller
    IEnumerator BossesController()
    {
        while (isBossBehave)
        { 
            for (int index = 0; index < EnemySpawner.EnemyBehaviours.Count; index++)
            {
                EnemySpawner.EnemyBehaviours[index].MoveToPositionWithRotation(PlayerTransform.position, Time.fixedDeltaTime);
            }
            yield return null;
        }
    }

    private void SummonBoss()
    {
        for (int i = 0; i < EnemySpawner.EnemyBehaviours.Count; i++)
        {
            EnemySpawner.EnemyBehaviours[i].model.SetActive(true);
            DOTween.To(() => EnemySpawner.EnemyBehaviours[i].model.transform.position, position => EnemySpawner.EnemyBehaviours[i].model.transform.position = position,
    new Vector3(EnemySpawner.EnemyBehaviours[i].model.transform.position.x, 0f, EnemySpawner.EnemyBehaviours[i].model.transform.position.z), 1f)
            .OnComplete(() =>
            {
                
            });
        }
        StartCoroutine(DeclareSkill());
        StartCoroutine(BossesController());
    }
    [SerializeField]
    private float _radiusFormation = 25f;
    public void BossesCircleFormation()
    {
        for (int i = 0; i < EnemySpawner.EnemyBehaviours.Count; i++)
        {
            EnemySpawner.EnemyBehaviours[i].gameObject.SetActive(true);
            /* Distance around the circle */
            var radians = 2 * Mathf.PI / EnemySpawner.EnemyBehaviours.Count * i;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            /* Get the spawn position */
            var spawnPos = PlayerTransform.position + spawnDir * _radiusFormation; // Radius is just the distance away from the point

            /* Now spawn */
            //var enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity) as GameObject;
            EnemySpawner.EnemyBehaviours[i].transformEnemy.position = spawnPos;

            /* Rotate the enemy to face towards player */
            EnemySpawner.EnemyBehaviours[i].transform.LookAt(PlayerTransform.position);
            EnemySpawner.EnemyBehaviours[i].model.SetActive(false);
        }
        Invoke(nameof(SummonBoss), 0.5f);
    }

    private float _minRadiusPosSpanw = 1f;
    private float _maxRadiusPosSpanw = 5f;
    public List<SkillBossInfoInCutscene> skillBossInfoInCutscenes;
    void ShakeCamera()
    {
        cameraShake.ShakeCamera();
    }
    IEnumerator DeclareSkill()
    {
        Invoke(nameof(ShakeCamera), 1f);
        foreach (var skill in skillBossInfoInCutscenes)
        {
            for(int index = 0; index < skill.numberSkill; index++)
            {
                Vector3 Position;
                float radiusRegionSpawn = 5.0f;
                var angle = (index * skill.angleStep) + skill.startAngle; 
                Vector3 newPosition = PositionRadian(skill.objectSkill, PlayerTransform, angle, skill.radius);
                Position = newPosition;
                //Debug.Log("Skill" + skill.objectSkill.name + " - Angle: " + Position);
                Position.y = 0.1f;
                Instantiate(skill.objectSkill, Position, skill.objectSkill.transform.rotation, this.transform);
                yield return new WaitForSeconds(0.2f);
            }
        }
        _playerController.OnHeroDeadCutscene();
    }
    Vector3 PositionRadian(GameObject objectSkill, Transform player, float angle, float radius)
    {
        //get the up and right vectors from the player object so we can orient the buttons
        Vector3 up = player.up;
        Vector3 right = player.right;

        angle = Mathf.Deg2Rad * angle;  //convert degrees to radians.  radians=degrees * 2pi / 360
        //Debug.Log("Skill" + objectSkill.name + " - Angle: " + angle);
        //cos(angle) give an x coordinate, on a unit circle centered around 0
        //sin(angle) is the y coordinate on the unit circle
        //take those values, multiply them by the up and right vectors to orient them to the player, 
        //multiply by the radius to move them the correct distance from the buttoncenter, 
        //and add the buttoncenter position so they circle around the correct point
        Vector3 pos = new Vector3(player.position.x + radius * Mathf.Cos(angle), 0f, player.position.z + radius * Mathf.Sin(angle));
        return pos;
    }
    #endregion
}
[System.Serializable]
public class SkillBossInfoInCutscene
{
    public GameObject objectSkill;
    public int numberSkill;
    public float radius;
    public float angleStep;
    public float startAngle;
}
