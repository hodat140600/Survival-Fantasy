using System.Collections.Generic;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.GoldSkill;
using UniRx;
using UnityEngine;

namespace Manager.HUD.UIElements
{
    public class SkillContainer : MonoBehaviour
    {
        [SerializeField] private GameObject skillIconPrefab;
        [SerializeField] List<SkillIcon> skillIcons;
        void Start()
        {
            MessageBroker.Default.Receive<UpdateCoolDownEvent>()
                .Subscribe(updateCoolDownEvent => SetCoolDown(updateCoolDownEvent.skillName, updateCoolDownEvent.time))
                .AddTo(gameObject);
        }
        private void SetCoolDown(string skillName,float time)
        {
            SkillIcon skill = skillIcons.Find(skillIcon => skillIcon.name == skillName);
            skill.SetCoolDown(time);
            //Debug.Log("recive message");
        }

        public void SetActive(ISkillSettings skill)
        {
            if (skill is GoldSettings)
            {
                return;
            }
            bool isDamageSKill = skill is IDamageSettings;
            SkillIcon skillIcon = skillIcons.Find(skillIcon => 
                skillIcon.name==skill.GetType().Name || // xac dinh name skill neu trung
                //skillIcon chua active va xac dinh vi tri cho damageSkillIcon
                (skillIcon.isActivated == false && isDamageSKill==skillIcon.isDamage)
                );
            skillIcon.SetActive(skill);
        }
    }
}