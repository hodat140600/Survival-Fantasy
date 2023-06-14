using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    public class BossSkillSystem : MonoBehaviour
    {
        public enum SkillType
        {
            ThunderStrike,
            MeteorStrike,
            Earthquake,
            FrostField
        }
        public SkillType skillType;

        #region Skill Settings
        public ThunderStrikeSkillSettings thunderStrikeSkillSettings;
        public MeteorStrikeSkillSettings meteorStrikeSkillSettings;
        public EarthquakeSkillSettings earthquakeSkillSettings;
        public EarthquakeSkillSettings frostFieldSkillSettings;
        #endregion


        private ISkillSettings _skillSettings;


        private void Start()
        {
            _skillSettings = LoadSkill();
            _skillSettings.LevelUp(gameObject);
        }

        private ISkillSettings LoadSkill()
        {
            switch (skillType)
            {
                case SkillType.ThunderStrike:
                    return thunderStrikeSkillSettings;
                case SkillType.MeteorStrike:
                    return meteorStrikeSkillSettings;
                case SkillType.Earthquake:
                    return earthquakeSkillSettings;
                case SkillType.FrostField:
                    return frostFieldSkillSettings;
            }

            //* This never work, trust me *//
            return null;
        }
    }
}
