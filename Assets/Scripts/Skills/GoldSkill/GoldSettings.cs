using UniRx;
using UnityEngine;

namespace Assets.Scripts.Skills.GoldSkill
{
    [CreateAssetMenu(menuName = "Skills/GoldSettings", fileName = "GoldSettings")]
    public class GoldSettings : ScriptableObject,ISkillSettings
    {
        [SerializeField] private int _addGold;
        public void LevelUp(GameObject gameObject)
        {
            gameObject.GetComponent<SkillSystem>().AddGold(_addGold);
        }

        public int Level { get; }
        [SerializeField] private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Description
        {
            get
            {
                return "<color=white>Gold <color=green>+ " + _addGold.ToString();
            }
        }
    }
}