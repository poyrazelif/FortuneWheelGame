using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using FortuneGame.Core;

namespace FortuneGame.Managers
{
    public class GameManager : Singleton<GameManager>
        {
            private int activeLevel = 0;
            [SerializeField] private Button _exitButton;
            [SerializeField] private Button _collectRewardsButton;
            [SerializeField] private Button _goBackButton;
            [SerializeField] private Button _spinButton;
            [SerializeField] private Button _giveUpButton;
            [SerializeField] private Button _reviveButton;

            [SerializeField] private GameObject _exitPanel;
            [SerializeField] private GameObject _bombPanel;
            [SerializeField] private GameObject _collectPanel;

            public int ActiveLevel
            {
                get => activeLevel;
            }

            private void Start()
            {
                _exitButton.gameObject.SetActive(false);
                EventManager.PassedNextLevel += ExitButtonActivate;
                EventManager.BombSelected += BombPanelActivate;
                EventManager.CollectRewardsRequest += CollectPanelActivate;

                _exitButton.onClick.AddListener(ExitGameRequest);
                _collectRewardsButton.onClick.AddListener(CollectRewards);
                _goBackButton.onClick.AddListener(GoBack);
                _spinButton.onClick.AddListener(StartSpin);
                _giveUpButton.onClick.AddListener(GiveUp);
                _reviveButton.onClick.AddListener(Revive);
            }

            private void OnValidate()
            {
                _exitButton.onClick.AddListener(ExitGameRequest);
                _collectRewardsButton.onClick.AddListener(CollectRewards);
                _goBackButton.onClick.AddListener(GoBack);
                _spinButton.onClick.AddListener(StartSpin);
                _giveUpButton.onClick.AddListener(GiveUp);
                _reviveButton.onClick.AddListener(Revive);
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
                _exitPanel.transform.DOScale(Vector3.zero, .2f).OnComplete(() => { _exitPanel.SetActive(false); });

            }

            private void CollectRewards()
            {
                EventManager.CollectRequestInvoke();
            }

            private void GiveUp()
            {
                ResetLevels();
                EventManager.GameEnd();
                CloseBombPanel();
            }

            private void CloseBombPanel()
            {
                _bombPanel.transform.DOScale(Vector3.zero, .2f).OnComplete(() => { _bombPanel.SetActive(false); });
            }

            public void BombPanelActivate()
            {
                _bombPanel.transform.localScale = Vector3.zero;
                _bombPanel.SetActive(true);
                ReviveButtonCheckEnoughMoney();
                _bombPanel.transform.DOScale(Vector3.one, .2f);

            }

            public void CollectPanelActivate()
            {
                _exitPanel.SetActive(false);
                _collectPanel.transform.localScale = Vector3.zero;
                _collectPanel.SetActive(true);
                _collectPanel.transform.DOScale(Vector3.one, .2f);
            }

            public void Revive()
            {
                EconomyManager.Instance.SpendMoney(20 + ActiveLevel * 10);
                EventManager.ReviveInvoke();
                CloseBombPanel();
            }

            public void ReviveButtonCheckEnoughMoney()
            {
                _reviveButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
                    (20 + ActiveLevel * 10).ToString();
                if (EconomyManager.Instance.CheckEnoughMoney(20 + ActiveLevel * 10))
                    _reviveButton.interactable = true;
                else
                {
                    _reviveButton.interactable = false;
                }
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
    }
