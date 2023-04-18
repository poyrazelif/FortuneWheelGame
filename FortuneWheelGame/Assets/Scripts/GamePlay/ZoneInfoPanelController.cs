using System;
using UnityEngine;
using TMPro;
using FortuneGame.Managers;
using UnityEngine.Serialization;

namespace FortuneGame.GamePlay
{
    public class ZoneInfoPanelController : MonoBehaviour
    {
        [FormerlySerializedAs("_SafeZoneText")] [SerializeField] private TextMeshProUGUI safeZoneText;
        [FormerlySerializedAs("_SuperZoneText")] [SerializeField] private TextMeshProUGUI superZoneText;

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
            safeZoneText.text = "Safe Zone" + targetSafeLevel;
            superZoneText.text = "Super Zone" + targetSuperLevel;
        }
    }
}
