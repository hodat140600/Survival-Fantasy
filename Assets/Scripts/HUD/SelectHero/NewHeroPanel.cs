using UniRx;
using TMPro;
using UnityEngine;

public class NewHeroPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    public void SetName(string name)
    {
        nameText.text = name;
    }
    public void OnAccpectButton()
    {
        MessageBroker.Default.Publish(new PlaySoundEvent("UpgradeCharacter"));
        gameObject.SetActive(false);
    }
}
