using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZoneInfoPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _SafeZoneText;
    [SerializeField] private TextMeshProUGUI _SuperZoneText;

    private void Start()
    {
        EventManager.PassedNextLevel += UpdateTexts;
        EventManager.GameEnded += UpdateTexts;
    }

    public void UpdateTexts()
    {
        int targetSafeLevel = (GameManager.Instance.ActiveLevel / 5 + 1) * 5;
        int targetSuperLevel = (GameManager.Instance.ActiveLevel / 30 + 1) * 30;
        _SafeZoneText.text = "Safe Zone" + targetSafeLevel;
        _SuperZoneText.text = "Super Zone" + targetSuperLevel;
    }
}
