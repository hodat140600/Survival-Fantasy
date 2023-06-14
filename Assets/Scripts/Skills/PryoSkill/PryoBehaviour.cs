
using UnityEngine;

public class PryoBehaviour : MonoBehaviour
{
    public float speed = 70f;
    public int damage = 50;
    public float timePryoFly = 3f;
    private float _timeDisable = 4f;
    public Rigidbody rigidbody;

    private void Reset()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void DisableProjectile()
    {
        //Debug.Log("Pryo");
        ProjectilesSpawner.Instance.ReturnPoolProjectile(gameObject, "PryoEnergy");
    }

    private void OnEnable()
    {
        rigidbody.velocity = Vector3.zero;
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
            //HitImpactSpawner.Instance.SpawnHitImpact("PryoEnergy", e.transform.position, Quaternion.identity); 
        }
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
