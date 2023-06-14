using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;

namespace Skills.Interfaces
{
    public interface IProjectilesSettings: ISkillSettings
    {
        public NumberProjectiles NumberProjectiles { get; }
    }
}