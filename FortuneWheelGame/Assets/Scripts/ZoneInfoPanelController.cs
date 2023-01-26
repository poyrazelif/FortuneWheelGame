using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZoneInfoPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _SafeZoneText;
    [SerializeField] private TextMeshProUGUI _SuperZoneText;
    private GameManager _gameManager;

    private void Start()
    {
       _gameManager= GameManager.Instance;
       EventManager.PassedNextLevel += UpdateTexts;
       EventManager.GameEnded += UpdateTexts;
    }

    public void UpdateTexts()
    {
        int targetSafeLevel = (_gameManager.ActiveLevel / 5 + 1) * 5;
        int targetSuperLevel = (_gameManager.ActiveLevel / 30 + 1) * 30;
        _SafeZoneText.text = "Safe Zone" + targetSafeLevel;
        _SuperZoneText.text = "Super Zone" + targetSuperLevel;
    }
}
