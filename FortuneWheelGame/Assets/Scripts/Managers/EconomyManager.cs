using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FortuneGame.Core;

namespace FortuneGame.Managers
{
    public class EconomyManager : Singleton<EconomyManager>
    {
        [SerializeField] private Image coinIcon;
        [SerializeField] private TextMeshProUGUI currentMoneyText;

        private void Start()
        {
            currentMoneyText.text = PlayerPrefs.GetInt("CurrentMoney", 0).ToString();
        }

        public void AddMoney(int amount)
        {
            int lastMoney = PlayerPrefs.GetInt("CurrentMoney", 0);
            PlayerPrefs.SetInt("CurrentMoney", lastMoney + amount);
            MoneyTextAnim(lastMoney);
            EventManager.MoneyChangedInvoke();
        }

        public void SpendMoney(int amount)
        {
            if (CheckEnoughMoney(amount))
            {
                int lastMoney = PlayerPrefs.GetInt("CurrentMoney", 0);
                PlayerPrefs.SetInt("CurrentMoney", lastMoney - amount);
                MoneyTextAnim(lastMoney);
                EventManager.MoneyChangedInvoke();
            }
        }

        public bool CheckEnoughMoney(int amount)
        {
            if (PlayerPrefs.GetInt("CurrentMoney", 0) >= amount)
                return true;
            else
            {
                return false;
            }
        }

        private void MoneyTextAnim(float lastMoney)
        {
            DOTween.To(() => lastMoney, x => lastMoney = x, PlayerPrefs.GetInt("CurrentMoney", 0), 1)
                .OnUpdate(() => currentMoneyText.text = lastMoney.ToString("F0"));
        }
    }
}
