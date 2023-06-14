using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SelectContainer : MonoBehaviour
{
    [SerializeField] 
    private GameObject _select;
    [SerializeField] 
    private GameObject _lock;

    [SerializeField] 
    private GameObject _selectButton;

    [SerializeField] 
    private GameObject _selected;

    [SerializeField] 
    private ChapterStorePanel _chapterStore;

    public void SetPanel(bool isUnlock,bool isSelected,int heroID)
    {
        _select.SetActive(isUnlock);
        _lock.SetActive(!isUnlock);
        _selected.SetActive(isSelected);
        _selectButton.SetActive(!isSelected);
        if (isUnlock)
        {
            var button = _selectButton.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                MessageBroker.Default.Publish(new PlaySoundEvent("SelectButton"));
                HeroLibrary.Instance.SelectedHero(heroID);
                SetSelected();
                _chapterStore.UpdateHeroContainer();
            });
        }
    }

    

    public void SetSelected()
    {
        _select.SetActive(true);
        _selectButton.SetActive(false);
        _selected.SetActive(true);
        _lock.SetActive(false);
    }

}
