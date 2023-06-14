using Assets.Scripts.Utils;
using UnityEngine;
using UniRx;

namespace Assets.Scripts.Skills.Attributes
{
    [System.Serializable]
    public class Health : IAttribute
    {
        public int BasePoint;
        public int Percent;

        public int PercentHeal;

        public bool IsDead { get => this.CurrentPoint == 0; }
        public float RemainingPercent { get => (float)this.CurrentPoint / (float)this.FullPoint; }
        [SerializeField]
        private int CurrentPoint;
        private int FullPoint { get => BasePoint + MathUtils.GetPercentOfValue(Percent, BasePoint); }

        public Health()
        {
            BasePoint = 0;
            Percent = 0;
        }

        public void SetFullPoint()
        {
            this.CurrentPoint = FullPoint;
        }

        public void IncreaseHealth(Health health)
        {
            //* health * percent
            float addedHealth = (health.Percent * this.FullPoint) / 100;
            SpawnDamageNumber.Instance.SpawnHealNumberMesh(addedHealth); 
            MessageBroker.Default.Publish(new PlaySoundEvent(nameof(Heal)));
            //* health = health + health * percent
            this.CurrentPoint = (int)Mathf.Clamp(this.CurrentPoint + addedHealth, 0.0f, this.FullPoint);

            //Debug.Log("PLayer heal " + this.CurrentPoint + " with " + health.Percent + " percent , added health " + addedHealth);
        }

        public void DecreaseHealth(int damage)
        {
            this.CurrentPoint = Mathf.Clamp(this.CurrentPoint - damage, 0, (int)this.FullPoint);
        }

        public void Heal()
        {
            var addedHealth=MathUtils.GetPercentOfValue(PercentHeal, BasePoint);
            CurrentPoint += addedHealth;
            SpawnDamageNumber.Instance.SpawnHealNumberMesh(addedHealth);
            MessageBroker.Default.Publish(new PlaySoundEvent(nameof(Heal)));
        }
    }
}

