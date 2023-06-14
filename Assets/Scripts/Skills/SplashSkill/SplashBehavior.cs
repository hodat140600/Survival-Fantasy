using UnityEngine;

public class SplashBehavior : MonoBehaviour
{
    private Transform dirSplash;
    public float speed = 70f;
    public int damage = 50;
    public float timeSplashFly = 3f;
    public int passThrough = 2;
    [SerializeField]
    private int _passThroughCount;
    private float _timeDisable = 4f;
    public Rigidbody rigidbody;

    private void Reset()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    //private void Update()
    //{
    //    SplashForce();
    //}
    void DisableProjectile()
    {
        //Debug.Log("Splash");
        ProjectilesSpawner.Instance.ReturnPoolProjectile(gameObject, "Splash");
    }
    //public void LockDirection(Transform _dirSplash)
    //{
    //    dirSplash = _dirSplash;
    //}

    private void OnEnable()
    {
        rigidbody.velocity = Vector3.zero;
        _passThroughCount = 0;
        Invoke(nameof(DisableProjectile), _timeDisable);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBehavior e = other.GetComponent<EnemyBehavior>();
        if (e != null)
        {
            e.TakeDamage(damage);
            HitImpactSpawner.Instance.SpawnHitImpact("Splash", e.transform.position, Quaternion.identity);
            _passThroughCount++;
        }
        if(_passThroughCount >= passThrough) DisableProjectile();
    }

    //void SplashForce()
    //{
    //    float distanceThisFrame = speed * Time.deltaTime;
    //    transform.Translate(dirSplash.forward.normalized * distanceThisFrame, Space.World);
    //    //transform.LookAt(dirSplash.forward);
    //}
    public void Force(float force, Vector3 direction)
    {
        rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
    }
}
