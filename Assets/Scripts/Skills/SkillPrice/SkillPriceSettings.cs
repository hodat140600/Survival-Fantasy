using System;
using System.Collections.Generic;
using Assets.Scripts.Skills.LiveSkill;
using Skills.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    [Serializable]
    public class BuySetting
    {
        public int goldStep;
        public int percentStepPoint;

        public int GetGold()
        {
            return goldStep;
        }
    }
    [CreateAssetMenu(menuName = ("Price/SkillPriceSettings"),fileName = ("SkillPriceSettings"))]
    public class SkillPriceSettings : ScriptableObject
    {
        [SerializeField]private BuySetting _damageSkillBuySetting;
        [SerializeField]private BuySetting _liveSkillBuySetting;
        [SerializeField]private BuySetting _lootBuySetting;
        public Dictionary<string, BuySetting> buySettings = new Dictionary<string, BuySetting>();
        private void OnEnable()
        {
            buySettings.Add(nameof(DamageSettings),_damageSkillBuySetting);
            buySettings.Add(nameof(LiveSkillSettings),_liveSkillBuySetting);
            buySettings.Add(nameof(LootSkillSettings),_lootBuySetting);
        }

        public int GetGold(IPercentSettings skill)
        {
             
            // So lan mua = Current Percent / Percent Per step
            var buySetting = buySettings[skill.GetType().Name];
            return (skill.GetAddedPercent()/buySetting.percentStepPoint)*buySetting.GetGold();
        }

        public int GetPercentPerStep(string skillname)
        {
            return buySettings[skillname].percentStepPoint;
        }
        /*public BuySetting lootSetting;*/
        // Damage, Health, Loot Step Point
    }
}