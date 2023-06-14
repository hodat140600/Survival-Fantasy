using System.Collections;
using UnityEngine;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Skills
{
    public class DeathCircleSkillBehavior : SkillBehavior, IDamageSkillBehavior, IRadiusSkillBehavior
    {
        public bool isEnemySkill = true;
        public Transform transformTarget;
        public Cooldown Cooldown;

        private float _minRadiusPosSpanw = 1f;
        private float _maxRadiusPosSpanw = 5f;

        protected void Awake()
        {
            damage = new Damage();
            numberProjectiles = new NumberProjectiles();
            radius = new Radius();
            Cooldown = new Cooldown();
        }

        protected void Start()
        {
            GetTransformTarget();
            StartCoroutine(DeclareSkill());
        }

        public void IncreaseDamagePercent(int percent)
        {
            damage.Percent += percent;
        }

        public void IncreaseRadiusPercent(int percent)
        {
            radius.Percent += percent;
        }

        #region Skill behavior
        IEnumerator DeclareSkill()
        {
            while (true)
            {
                yield return new WaitForSeconds(Cooldown.RealPoint);
                //RandomSkill();
                Vector3[] arrayPosition = new Vector3[numberProjectiles.RealPoint];
                float radiusRegionSpawn = numberProjectiles.BasePoint * radius.BasePoint * 2.0f;

                for (int i = 0; i < numberProjectiles.RealPoint; i++)
                {
                    //Vector3 newPosition;
                    //do
                    //{
                    //    // axis y plus 0.1 because it is on the ground not in the ground
                    //    newPosition = new Vector3(transformTarget.position.x + Random.Range(-radiusRegionSpawn, radiusRegionSpawn), transform.position.y + 0.1f, transformTarget.position.z + Random.Range(-radiusRegionSpawn, radiusRegionSpawn));
                    //} while (isNearlyOther(newPosition, arrayPosition, i));
                    Vector3 newPosition = CheckInRange.GetPosOnRing(GameManager.Instance.playerTransform.position, _minRadiusPosSpanw, _maxRadiusPosSpanw);
                    //arrayPosition[i] = new Vector3();
                    arrayPosition[i] = newPosition;
                    arrayPosition[i].y = 0.1f;

                    //StartCoroutine(SpawnSkillSequence(arrayPosition[i]));
                    SpawnSkill(arrayPosition[i]);
                    yield return new WaitForSeconds(Random.Range(0.25f, 0.75f));
                }
            }
        }

        //private void RandomSkill()
        //{
        //}

        ////private bool isNearlyOther(Vector3 newPosition, Vector3[] arrayPosition, int size)
        ////{
        ////    for (int i = 0; i < size; i++)
        ////    {
        ////        float realDistance = MathUtils.GetDistance(newPosition.x, newPosition.z, arrayPosition[i].x, arrayPosition[i].z); //* only use x and z Axis
        ////        float limitCloseDistance = Mathf.Pow(radius.BasePoint * 2, 2);

        ////        //* distance from 2 point of the gameObjects must greater than radius of 2 objects
        ////        if (realDistance < limitCloseDistance)
        ////        {
        ////            return true;
        ////        }
        ////    }
        ////    return false;
        ////}

        //IEnumerator SpawnSkillSequence(Vector3 vector3)
        //{
        //    yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));
        //    SpawnSkill(vector3);
        //}

        private DeathCircleBehavior _deathCircleBehavior;
        protected virtual void SpawnSkill(Vector3 position)
        {
            if (_deathCircleBehavior == null)
            {
                _deathCircleBehavior = skillObjectPrefab.GetComponent<DeathCircleBehavior>();
            }

            _deathCircleBehavior.Init(damage, radius, isEnemySkill);
            var skillObject = Instantiate(skillObjectPrefab, position, skillObjectPrefab.transform.rotation);
            skillObject.transform.parent = transform.parent.parent;
        }

        private void GetTransformTarget()
        {
            transformTarget = GameManager.Instance.playerTransform;
        }
        #endregion //Skill behavior
    }
}