using System;
using UnityEngine;
using TMPro;
using FortuneGame.Managers;

namespace FortuneGame.GamePlay
{
    public class ZoneInfoPanelController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _SafeZoneText;
        [SerializeField] private TextMeshProUGUI _SuperZoneText;

        private void OnEnable()
        {
            EventManager.PassedNextLevel += UpdateTexts;
            EventManager.GameEnded += UpdateTexts;
        }
        private void OnDisable()
        {
            EventManager.PassedNextLevel -= UpdateTexts;
            EventManager.GameEnded -= UpdateTexts;
        }

        public void UpdateTexts()
        {
            int targetSafeLevel = (GameManager.Instance.ActiveLevel / 5 + 1) * 5;
            int targetSuperLevel = (GameManager.Instance.ActiveLevel / 30 + 1) * 30;
            _SafeZoneText.text = "Safe Zone" + targetSafeLevel;
            _SuperZoneText.text = "Super Zone" + targetSuperLevel;
        }
    }
}
