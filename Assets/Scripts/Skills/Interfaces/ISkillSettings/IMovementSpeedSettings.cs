using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
namespace Skills.Interfaces
{
    public interface IMovementSpeedSettings: ISkillSettings
    {
        public Speed Speed { get; }
    }
}