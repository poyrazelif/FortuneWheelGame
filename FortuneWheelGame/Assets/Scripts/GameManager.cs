using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager :Singleton<GameManager>
{
    private int activeLevel=0;
    [SerializeField]private Button _exitButton;
    [SerializeField] private Button _collectRewardsButton;
    [SerializeField] private Button _goBackButton;
    [SerializeField] private Button _spinButton;
    [SerializeField] private Button _giveUpButton;
    [SerializeField] private Button _reviveButton;

    [SerializeField] private GameObject _exitPanel;
    [SerializeField] private GameObject _bombPanel;

    public int ActiveLevel
    {
        get => activeLevel;
    }

    private void Start()
    {
        EventManager.PassedNextLevel += ExitButtonActivate;
    }

    private void OnValidate()
    {
        _exitButton.onClick.AddListener(ExitGameRequest);
        _collectRewardsButton.onClick.AddListener(CollectRewards);
        _goBackButton.onClick.AddListener(GoBack);
        _spinButton.onClick.AddListener(StartSpin);
        _giveUpButton.onClick.AddListener(GiveUp);
    }

    public void ExitGameRequest()
    {
        _exitPanel.transform.localScale = Vector3.zero;
        _exitPanel.SetActive(true);
        _exitPanel.transform.DOScale(Vector3.one, .2f);
    }

    private void StartSpin()
    {
        _exitButton.gameObject.SetActive(false);
        EventManager.SpinStartInvoke();
    }

    private void ExitButtonActivate()
    {
        _exitButton.gameObject.SetActive(true);
    } 

    private void GoBack()
    {
        _exitPanel.transform.DOScale(Vector3.zero, .2f).OnComplete(() =>
        {
            _exitPanel.SetActive(false);
        });

    }

    private void CollectRewards()
    {
        EventManager.CollectRequestInvoke();
    }

    private void GiveUp()
    {
        ResetLevels();
        EventManager.GameEnd();
        _bombPanel.SetActive(false);
    }
    

    public void IncreaseLevel()
    {
        activeLevel++;
        EventManager.NextLevel();
    }

    public void ResetLevels()
    {
        activeLevel = 0;
    }
    

}
