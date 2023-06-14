using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Manager.HUD.UIElements
{
    public class SettingPanel : MonoBehaviour
    {   
        [SerializeField]
        private Button settingButton;
        
        [SerializeField]
        private GameObject settingContainer;

        private bool _isOpenned;
        void Start()
        {
            _isOpenned = false;
        }
        
        public void OnSettingButtonClick()
        {
            Vector3 vector = _isOpenned ? Vector3.zero : Vector3.one;
            DOTween.To(() => settingContainer.transform.localScale, transformLocalScale => settingContainer.transform.localScale = transformLocalScale,
                vector, 0.02f).OnComplete(
                () =>
                {
                    _isOpenned = !_isOpenned;
                });
        }

    }
}