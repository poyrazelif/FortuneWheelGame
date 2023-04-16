using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;

namespace FortuneGame.Managers
{
    public class GameManager : Core.Singleton<GameManager>
        {
            [SerializeField] private Button exitButton;
            [SerializeField] private Button collectRewardsButton;
            [SerializeField] private Button goBackButton;
            [SerializeField] private Button spinButton;
            [SerializeField] private Button giveUpButton;
            [SerializeField] private Button reviveButton;

            [FormerlySerializedAs("_exitPanel")] [SerializeField] private GameObject exitPanel;
            [FormerlySerializedAs("_bombPanel")] [SerializeField] private GameObject bombPanel;
            [FormerlySerializedAs("_collectPanel")] [SerializeField] private GameObject collectPanel;
            
            private int activeLevel = 0;
            public int ActiveLevel
            {
                get => activeLevel;
            }

            private void Start()
            {
                exitButton.gameObject.SetActive(false);
                
                exitButton.onClick.AddListener(ExitGameRequest);
                collectRewardsButton.onClick.AddListener(CollectRewards);
                goBackButton.onClick.AddListener(GoBack);
                spinButton.onClick.AddListener(StartSpin);
                giveUpButton.onClick.AddListener(GiveUp);
                reviveButton.onClick.AddListener(Revive);
            }

            private void OnEnable()
            {
                EventManager.PassedNextLevel += ExitButtonActivate;
                EventManager.BombSelected += BombPanelActivate;
                EventManager.CollectRewardsRequest += CollectPanelActivate;
            }

            private void OnDisable()
            {
                EventManager.PassedNextLevel -= ExitButtonActivate;
                EventManager.BombSelected -= BombPanelActivate;
                EventManager.CollectRewardsRequest -= CollectPanelActivate;
            }

            private void OnValidate()
            {
                exitButton = exitButton.GetComponent<Button>();
                collectRewardsButton=collectRewardsButton.GetComponent<Button>();
                goBackButton=goBackButton.GetComponent<Button>();
                spinButton=spinButton.GetComponent<Button>();
                giveUpButton=giveUpButton.GetComponent<Button>();
                reviveButton=reviveButton.GetComponent<Button>();
            }


            public void ExitGameRequest()
            {
                exitPanel.transform.localScale = Vector3.zero;
                exitPanel.SetActive(true);
                exitPanel.transform.DOScale(Vector3.one, .2f);
            }

            private void StartSpin()
            {
                exitButton.gameObject.SetActive(false);
                EventManager.SpinStartInvoke();
            }

            private void ExitButtonActivate()
            {
                exitButton.gameObject.SetActive(true);
            }

            private void GoBack()
            {
                exitPanel.transform.DOScale(Vector3.zero, .2f).OnComplete(() => { exitPanel.SetActive(false); });

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
                bombPanel.transform.DOScale(Vector3.zero, .2f).OnComplete(() => { bombPanel.SetActive(false); });
            }

            public void BombPanelActivate()
            {
                bombPanel.transform.localScale = Vector3.zero;
                bombPanel.SetActive(true);
                ReviveButtonCheckEnoughMoney();
                bombPanel.transform.DOScale(Vector3.one, .2f);

            }

            public void CollectPanelActivate()
            {
                exitPanel.SetActive(false);
                collectPanel.transform.localScale = Vector3.zero;
                collectPanel.SetActive(true);
                collectPanel.transform.DOScale(Vector3.one, .2f);
            }

            public void Revive()
            {
                EconomyManager.Instance.SpendMoney(20 + ActiveLevel * 10);
                EventManager.ReviveInvoke();
                CloseBombPanel();
            }

            public void ReviveButtonCheckEnoughMoney()
            {
                reviveButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
                    (20 + ActiveLevel * 10).ToString();
                if (EconomyManager.Instance.CheckEnoughMoney(20 + ActiveLevel * 10))
                    reviveButton.interactable = true;
                else
                {
                    reviveButton.interactable = false;
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
