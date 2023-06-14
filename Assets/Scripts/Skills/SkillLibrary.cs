using System.Collections.Generic;
using Assets.Scripts.Skills.GoldSkill;
using Assets.Scripts.Skills.Heal;
using Assets.Scripts.Skills.LiveSkill;
using Assets.Scripts.Skills.ProjectilesSkill;
using Assets.Scripts.Skills.SlowSkill;
using Assets.Scripts.Skills.SmashSkill;
using Assets.Scripts.Skills.SplashSkill;
using Assets.Scripts.Skills.TargetSkill;
using Assets.Scripts.Utils;
using heroSkills = DatHQ.Skill.Hero;
using DatHQ.Skill.Hero;

namespace Assets.Scripts.Skills
{
    // Quan ly Skills cua ca game,
    // Quan ly MAX SKILL Level
    // Quan ly danh sach skill cua ca Game 
    public class SkillLibrary : Singleton<SkillLibrary>
    {
        public List<OrbsSkillSettings> orbsSkillSettingsList;
        public List<TargetSkillSettings> targetSkillSettingsList;
        public List<BlastSkillSettings> blastSkillSettingsList;
        public List<SlowSkillSettings> slowSkillSettingsList;
        public List<SplashSkillSettings> splashSkillSettingsList;
        public List<ChainLightningSkillSetting> chainLightningSkillSettingsList;
        public List<DamageSettings> damageSettingsList;
        public List<LiveSkillSettings> liveSkillSettingsList;
        public List<SmashSkillSettings> smashSkillSettingsList;
        public List<MoveSkillSettings> moveSkillSettingsList;
        public List<RadiusSettings> radiusSettingsList;
        public List<ProjectilesSettings> projectilesSettingsList;
        public List<MagnetSkillSettings> magnetSkillSettingsList;
        public List<heroSkills.MeteorStrikeSkillSettings> meteorStrikeSkillSettings;
        public List<heroSkills.LaserBeamSkillSettings> laserBeamSkillSettings;
        public List<heroSkills.FrostFieldSkillSettings> frostFieldSkillSettings;
        public List<heroSkills.EarthquakeSkillSettings> earthquakeSkillSettings;
        public List<heroSkills.PoisonSkillSettings> poisonSkillSettings;
        public List<PryoSkillSettings> pryoSkillSettings;
        public List<CooldownSettings> CooldownSettingsList;
        public List<HealSkillSettings> HealSkillSettingsList;
        public List<ArmorSkillSettings> ArmorSkillSettingsList;

        public GoldSettings goldSkill;
        private Dictionary<string, List<ISkillSettings>> _skillLevels = new Dictionary<string, List<ISkillSettings>>();
        private void Awake()
        {
            //add dictionary
            _skillLevels.Add(nameof(OrbsSkillSettings), orbsSkillSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(TargetSkillSettings), targetSkillSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(BlastSkillSettings), blastSkillSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(SlowSkillSettings), slowSkillSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(SplashSkillSettings), splashSkillSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(ChainLightningSkillSetting), chainLightningSkillSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(DamageSettings), damageSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(LiveSkillSettings), liveSkillSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(SmashSkillSettings), smashSkillSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(MoveSkillSettings), moveSkillSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(RadiusSettings), radiusSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(ProjectilesSettings), projectilesSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(MagnetSkillSettings), magnetSkillSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(heroSkills.MeteorStrikeSkillSettings), meteorStrikeSkillSettings.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(heroSkills.LaserBeamSkillSettings), laserBeamSkillSettings.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(heroSkills.FrostFieldSkillSettings), frostFieldSkillSettings.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(heroSkills.EarthquakeSkillSettings), earthquakeSkillSettings.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(heroSkills.PoisonSkillSettings), poisonSkillSettings.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(PryoSkillSettings), pryoSkillSettings.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(CooldownSettings),CooldownSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(HealSkillSettings),HealSkillSettingsList.GetListType<ISkillSettings>());
            _skillLevels.Add(nameof(ArmorSkillSettings),ArmorSkillSettingsList.GetListType<ISkillSettings>());
        }
        public ISkillSettings NextSkillLevel(ISkillSettings iSkillSettings)
        {
            return _skillLevels[iSkillSettings.GetType().Name].Find(skillSettings => skillSettings.Level == iSkillSettings.Level + 1);
        }
        public List<ISkillSettings> NextSkillList(List<ISkillSettings> listSkill)
        {
            List<ISkillSettings> nextLevelList = new List<ISkillSettings>();
            foreach (var item in listSkill)
            {
                if (!IsMaxLevel(item))
                {
                    nextLevelList.Add(NextSkillLevel(item));
                }
            }
            return nextLevelList;
        }

        private bool IsMaxLevel(ISkillSettings skill)
        {
            return NextSkillLevel(skill) == null;
        }

        public List<ISkillSettings> GetNextLevelsOfSkillSettings(ISkillSettings skillSettings, int maxlevel)
        {
            var nextLevelsSkill = new List<ISkillSettings>();
            var skill = skillSettings;
            for (int i = 0; i < maxlevel; i++)
            {
                if (!IsMaxLevel(skill))
                {
                    skill = NextSkillLevel(skill);
                    nextLevelsSkill.Add(skill);
                };
            }
            return nextLevelsSkill;
        }

        public List<ISkillSettings> GetSkillsNotMaxLevel(List<ISkillSettings> skills)
        {
            List<ISkillSettings> skillSettingsList = new List<ISkillSettings>();
            foreach (var skill in skills)
            {
                if (!IsMaxLevel(skill))
                {
                    skillSettingsList.Add(skill);
                }
            }

            return skillSettingsList;
        }
        
    }

}