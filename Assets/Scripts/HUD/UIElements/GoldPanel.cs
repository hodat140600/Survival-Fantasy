using TMPro;
using UnityEngine;
using UniRx;

namespace Manager.HUD.UIElements
{
    public class GoldPanel : MonoBehaviour
    {
        public TextMeshProUGUI goldCountText;

        private void Start()
        {
            MessageBroker.Default.Receive<UpdateGoldEvent>()
                .Subscribe(updateGoldEvent => UpdateGoldCount(updateGoldEvent.gold))
                .AddTo(gameObject);
        }

        public void UpdateGoldCount(int goldCount)
        {
            goldCountText.text = goldCount.ToString();
        }
    }
}