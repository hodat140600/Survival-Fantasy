using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Skills.GoldSkill;
using Assets.Scripts.Skills.LiveSkill;
using Assets.Scripts.Skills.ProjectilesSkill;
using Assets.Scripts.Utils;
using Skills.Interfaces;
using UniRx;
using UnityEngine;
namespace Assets.Scripts.Skills
{
    // SkillSystem is component of Player
    public class SkillSystem : MonoBehaviour
    {
        [SerializeField]
        private GameObject skillHolder;
        public PlayerSettings playerSettings;
        // TODO: Keep track Settings da duoc Apply

        public Dictionary<string, ISkillSettings> selectedSkillSettings = new Dictionary<string, ISkillSettings>();
        private LiveSkillBehavior _liveSkillBehavior;

        const int MAXSKILLS = 8;
        private const int MAXDAMAGESKILLS = 4;
        private const int MAXATTRIBUTESKILLS = 4;


        private CompositeDisposable _disposables = new CompositeDisposable();

        // Use this for initialization
        void Start()
        {
            playerSettings = GameManager.Instance.playerSettings;
            LoadBoughtSkillSettings();
            var chapterStartEvent = MessageBroker.Default.Receive<ChapterStartEvent>().Subscribe(x => { RemoveSkillOnStartChapter(); }).AddTo(gameObject);
            chapterStartEvent.AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Clear();
        }

        void LoadBoughtSkillSettings()
        {
            // TODO: Load BoughSkillSettings tu PlayerSettings vao SkillSystems
        }

        public void ApplyAttribute(Health health)
        {
            if (_liveSkillBehavior == null)
            {
                _liveSkillBehavior = GetComponent<LiveSkillBehavior>();
            }
            _liveSkillBehavior.Heal(health);
        }
        public void SelectSkill(ISkillSettings skillSettings)
        {
            //tao selected basesettings 
            skillSettings.LevelUp(gameObject);
            if (skillSettings is GoldSettings)
            {
                return;
            }
            if (!selectedSkillSettings.TryAdd(skillSettings.GetType().Name, skillSettings))
            {
                selectedSkillSettings[skillSettings.GetType().Name] = skillSettings;
            };
            MessageBroker.Default.Publish(new PlaySoundEvent("PickUpSkill"));
        }

        public void InitSkillToStartChapter()
        {
            var selectedHero = GameManager.Instance.playerSettings.SelectedHero;
            //if (selectedHero == null)
            //{
            //    selectedHero = GameManager.Instance.baseHeroSetting;
            //}
            //load SkillHero
            // selectedHero.live.LevelUp(gameObject);
            // selectedHero.move.LevelUp(gameObject);
            // selectedHero.magnet.LevelUp(gameObject);
            CheckLevelUp(selectedHero.live);
            CheckLevelUp(selectedHero.damage);
            CheckLevelUp(selectedHero.move);
            CheckLevelUp(selectedHero.magnet);
            CheckLevelUp(selectedHero.armor);
            CheckLevelUp(selectedHero.radius);
            CheckLevelUp(selectedHero.loot);
            CheckLevelUp(selectedHero.coolDown);
            CheckLevelUp(selectedHero.regeneration);
            CheckLevelUp(selectedHero.exp);

            //load boughtSkill
            playerSettings.BoughtLiveSkillSettings.LevelUp(gameObject);
        }

        private void CheckLevelUp(IBaseSettings skill)
        {
            if (skill != null)
            {
                skill.LevelUp(gameObject);
            }
        }

        void RemoveSkillOnStartChapter()
        {
            for (int i = 0; i < skillHolder.transform.childCount; i++)
            {
                skillHolder.transform.GetChild(i).gameObject.SetActive(false);
            }
            Object[] objects = gameObject.GetComponents<SkillBehavior>();
            foreach (Object obj in objects)
            {
                Destroy(obj);
            }
        }
        SkillPrice GetPercentSkillToSell(IPercentSettings boughtSkillSettings,
            IPercentSettings sellingPercentSkillSettings, int stepPoint)
        {
            sellingPercentSkillSettings.SetPercent(boughtSkillSettings.GetAddedPercent());
            sellingPercentSkillSettings.AddPercent(stepPoint);
            SkillPrice skillPrice = new SkillPrice(sellingPercentSkillSettings as ISkillSettings);
            return skillPrice;
        }

        private SkillPrice GetSkillPriceToBuy<T>(T skillSetting) where T : ScriptableObject, IPercentSettings
        {
            SkillPriceSettings skillPrice = GameManager.Instance.skillPriceSettings;
            IPercentSettings sellingSettings = ScriptableObject.CreateInstance<T>();
            int percentStep = skillPrice.GetPercentPerStep(typeof(T).Name);
            return GetPercentSkillToSell(skillSetting,
                sellingSettings, percentStep);
        }
        public List<SkillPrice> GetBuyableSkills()
        {
            List<SkillPrice> list = new List<SkillPrice>();
            list.Add(GetSkillPriceToBuy(playerSettings.BoughtDamageSkillSettings));
            list.Add(GetSkillPriceToBuy(playerSettings.BoughtLiveSkillSettings));
            list.Add(GetSkillPriceToBuy(playerSettings.BoughtLootSkillSettings));
            return list;
        }
        public GameObject GetObjectSelectedSkil(string skillName)
        {
            GameObject skillObj = skillHolder.transform.Find(skillName).gameObject;
            return skillObj;
        }
        public void ApplyDamageSetting(IDamageSkillBehavior skill)
        {
            var selectedDamageSettings = selectedSkillSettings.GetValueType<DamageSettings>();
            skill.IncreaseDamagePercent(selectedDamageSettings ? selectedDamageSettings.AddedPercent : 0);
            skill.IncreaseDamagePercent(playerSettings.BoughtDamageSkillSettings.AddedPercent);
        }

        public void ApplyRadiusSetting(IRadiusSkillBehavior skill)
        {
            var selectedRadiusSettings = selectedSkillSettings.GetValueType<RadiusSettings>();
            skill.IncreaseRadiusPercent(selectedRadiusSettings ? selectedRadiusSettings.AddedPercent : 0);
        }

        public void ApplyProjectilesSetting(IProjectilesSkillBehavior skill)
        {
            var selectedProjectilesSettings = selectedSkillSettings.GetValueType<ProjectilesSettings>();
            skill.IncreaseProjectilesPoint(selectedProjectilesSettings ? selectedProjectilesSettings.addedPoint : 0);
        }
        public void ApplyCooldownSetting(ICooldownSkillBehavior skill)
        {
            var selectedCooldownSettings = selectedSkillSettings.GetValueType<CooldownSettings>();
            skill.IncreaseCooldownPercent(selectedCooldownSettings ? selectedCooldownSettings.addedPercent : 0);
        }

        public List<ISkillSettings> GetNextThreeSkillSettings()
        {
            List<ISkillSettings> nextThreeSkillSettings=new List<ISkillSettings>();
            nextThreeSkillSettings = GetNextSkill();
            if (nextThreeSkillSettings.Count == 0)
            {
                nextThreeSkillSettings.Add(SkillLibrary.Instance.goldSkill);
            }
            /*while (nextThreeSkillSettings.Count < 3)
            {
                nextThreeSkillSettings.Add(SkillLibrary.Instance.goldSkill);
            }*/
            return nextThreeSkillSettings;
        }

        public List<ISkillSettings> GetNextSkill()
        {
            var skillLibrary = SkillLibrary.Instance;
            List<ISkillSettings> nextSkills = new List<ISkillSettings>();
            var damageAvailableSkills = playerSettings.availableSkills.FindAll(skillSettings => skillSettings is IDamageSettings);
            var attributeAvailableSkills = playerSettings.availableSkills.FindAll(skillSettings => !(skillSettings is IDamageSettings));

            //empty
            if (!selectedSkillSettings.Any())
            {
                return damageAvailableSkills.RandomInList(3);
            }
            // listSS=8
            if (selectedSkillSettings.Count == MAXSKILLS)
            {
                var selectedSkills = selectedSkillSettings.Values.ToList();
                nextSkills = skillLibrary.NextSkillList(selectedSkills).RandomInList(3);
                nextSkills.Shuffle();
                return nextSkills;
            }
            //random tu 0 toi 3 
            int numberFirst = Random.Range(0, 4);
            var damageSelectedSkills = selectedSkillSettings.Values.ToList().FindAll(x => x is IDamageSettings);
            var attributeSelectedSkills = selectedSkillSettings.Values.ToList()
                .FindAll(skillSettings => !(skillSettings is IDamageSettings));

            //list SDS=4
            if (damageSelectedSkills.Count == MAXDAMAGESKILLS)
            {
                var attributeSkill = skillLibrary.NextSkillList(attributeSelectedSkills);
                attributeSkill.AddRange(attributeAvailableSkills.FindAll(skillSettings => !selectedSkillSettings.ContainsKey(skillSettings.GetType().Name)));
                nextSkills = (attributeSkill.RandomInList(numberFirst));
                nextSkills.AddRange(skillLibrary.NextSkillList(damageSelectedSkills).RandomInList(3 - nextSkills.Count));
                nextSkills.AddRange(attributeSkill.RandomInList(3 - nextSkills.Count));
                nextSkills.Shuffle();
                return nextSkills;
            }
            //list SAS=4
            if (attributeSelectedSkills.Count == MAXATTRIBUTESKILLS)
            {
                var damageSkills = skillLibrary.NextSkillList(damageSelectedSkills);
                damageSkills.AddRange(damageAvailableSkills.FindAll(skillSettings => !selectedSkillSettings.ContainsKey(skillSettings.GetType().Name)));
                nextSkills = (damageSkills.RandomInList(numberFirst));
                nextSkills.AddRange(skillLibrary.NextSkillList(attributeSelectedSkills).RandomInList(3 - nextSkills.Count));
                nextSkills.AddRange(damageSkills.RandomInList(3 - nextSkills.Count));
                nextSkills.Shuffle();
                return nextSkills;
            }

            //list SDS<4 and list SAS<4
            while (numberFirst < (3 - skillLibrary.GetSkillsNotMaxLevel(selectedSkillSettings.Values.ToList()).Count))
            {
                numberFirst = Random.Range(0, 4);
            }
            var availableSkillsNotSelected =
                playerSettings.availableSkills.FindAll(skillSettings => !selectedSkillSettings.ContainsKey(skillSettings.GetType().Name));
            nextSkills = availableSkillsNotSelected.RandomInList(numberFirst);
            var selectedSkill = selectedSkillSettings.Values.ToList();
            var nextLevelSelectedSkills = skillLibrary.NextSkillList(selectedSkill);
            nextSkills.AddRange(nextLevelSelectedSkills.RandomInList(3 - nextSkills.Count));
            nextSkills.Shuffle();
            return nextSkills;
        }

        /// <summary>
        /// Calculator and update gold to playerSettings, UI.
        /// </summary>
        public void AddGold(int addGold)
        {
            float percentGold = (float)this.playerSettings.BoughtLootSkillSettings.AddedPercent / 100.0f;
            float amountAddedGold = addGold * percentGold;

            //* tổng vàng thêm = vàng cộng thêm + phần trăm vàng thêm
            int totalGoldAdded = addGold + (int)amountAddedGold;

            //Debug.Log("<color=Cyan> Add Gold " + addGold + "</color>");
            //Debug.Log("<color=Cyan> Amount percent " + (int)amountAddedGold + "</color>");
            //Debug.Log("<color=Cyan> Percent : " + percentGold + "</color>");
            this.playerSettings.IncreaseGoldInChapter(totalGoldAdded);

            //Debug.Log("<color=Cyan> Player take " + (addGold + (int)amountAddedGold) + " coins with percent : " + this.playerSettings.BoughtLootSkillSettings.AddedPercent + "</color>");
            // Call update to UI
            MessageBroker.Default.Publish(new UpdateGoldEvent { gold = this.playerSettings.Gold + this.playerSettings.GoldInChapter });
        }

        private List<ISkillSettings> SkillsReward()
        {
            List<ISkillSettings> selectedSkills = selectedSkillSettings.Values.ToList();
            List<ISkillSettings> nextSkills = new List<ISkillSettings>();
            var skills = SkillLibrary.Instance.GetSkillsNotMaxLevel(selectedSkills);
            if (skills.Count == 1)// co 1 skill co the nang cap
            {
                //nang cap skill da chon len 3 level
                nextSkills.AddRange(SkillLibrary.Instance.GetNextLevelsOfSkillSettings(skills[0],3));
                return nextSkills;
            }

            if (skills.Count == 2) // co 2 skill co the nang cap
            {
                // radom nang cap skill thu nhat tu 0 toi 3 level
                var numberSkillFirst = Random.Range(0, 4);//random tu 0->3
                nextSkills.AddRange(SkillLibrary.Instance.GetNextLevelsOfSkillSettings(skills[0],numberSkillFirst));
                // skill thu hai duoc nang cap bang 3 - level skill thu nhat
                nextSkills.AddRange(SkillLibrary.Instance.GetNextLevelsOfSkillSettings(skills[1],3-nextSkills.Count));
                if (nextSkills.Count < 3)
                {
                    nextSkills.AddRange(SkillLibrary.Instance.GetNextLevelsOfSkillSettings(skills[0],3-nextSkills.Count));
                }
                return nextSkills;
            }

            if (skills.Count >= 3)
            {
                var skillLevelUp = skills.RandomInList(3);
                nextSkills.AddRange(SkillLibrary.Instance.NextSkillList(skillLevelUp));
                return nextSkills;
            }
            // co 3 skill co the ang cap
            return nextSkills;
        }

        public List<ISkillSettings> GetSkillsReward()
        {
            var skillList = SkillsReward();
            //reward panel can 3 skill
            while (skillList.Count<3) 
            {
                skillList.Add(SkillLibrary.Instance.goldSkill);
            }
            return skillList;
        }

        public ISkillSettings GetSkillUpgrade()
        {
            var randomSkill = SkillLibrary.Instance.GetSkillsNotMaxLevel(selectedSkillSettings.Values.ToList()).RandomInList(1);
            if (randomSkill.Count == 0 || randomSkill == null)
            {
                return null;
            }
            return SkillLibrary.Instance.NextSkillLevel(randomSkill.First());
        }
    }
}