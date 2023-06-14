    using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    private float _currentPercent;
    private Slider _slider;
    private void Start()
    {
        _slider = gameObject.GetComponent<Slider>();
        MessageBroker.Default.Receive<AddExperienceEvent>()
            .Subscribe(addExperienceEvent =>
            {
                ChangeValueSlider((float)addExperienceEvent.current/addExperienceEvent.toLvUp);
            }).AddTo(gameObject);
        MessageBroker.Default.Receive<LevelUpEvent>().Subscribe(levelUpEvent =>
        {
                ChangeValueSlider((float)levelUpEvent.currentExp/levelUpEvent.expToLvUp);
        }).AddTo(gameObject);
        MessageBroker.Default.Receive<ChapterStartEvent>().Subscribe(chapterStartEvent => { ResetLevelBar(); });
    }

    private void ChangeValueSlider(float percent)
    {
        _slider.value = percent;
    }

    private void ResetLevelBar()
    {
        _slider.value = 0;
    }
}
