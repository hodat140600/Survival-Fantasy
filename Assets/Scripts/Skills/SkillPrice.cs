    using Assets.Scripts.Skills;
    using Skills.Interfaces;

    public class SkillPrice
    {
        public SkillPrice(ISkillSettings skill)
        {
            Skill = skill;
        }

        public ISkillSettings Skill
        {
            get;
        }

        public int Price
        {
            get
            {
                var skillPriceSetting = GameManager.Instance.skillPriceSettings;
                return skillPriceSetting.GetGold(this.Skill as IPercentSettings);
            }
        }

        public string PercentToString()
        {
            IPercentSettings percentSettings = (IPercentSettings) Skill;
            return "+ "+(percentSettings.GetAddedPercent()-GameManager.Instance.skillPriceSettings.GetPercentPerStep(Skill.GetType().Name)).ToString()+" %"; 
        }
        
        
        
    }