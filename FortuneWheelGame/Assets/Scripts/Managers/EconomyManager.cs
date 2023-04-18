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
        [SerializeField] private TextMeshProUGUI currentMoneyText;
        private int CurrentMoney => PlayerPrefs.GetInt("CurrentMoney", 0);

        private void Start()
        {
            currentMoneyText.text = CurrentMoney.ToString();
        }

        public void AddMoney(int amount)
        {
            int lastMoney = CurrentMoney;
            PlayerPrefs.SetInt("CurrentMoney", CurrentMoney + amount);
            MoneyTextAnim(lastMoney);
            EventManager.MoneyChangedInvoke();
        }

        public void SpendMoney(int amount)
        {
            if (!CheckEnoughMoney(amount)) return;
            int lastMoney =CurrentMoney;
            PlayerPrefs.SetInt("CurrentMoney", CurrentMoney - amount);
            MoneyTextAnim(lastMoney);
            EventManager.MoneyChangedInvoke();
        }

        public bool CheckEnoughMoney(int amount)
        {
            return CurrentMoney >= amount;
        }

        private void MoneyTextAnim(float lastMoney)
        {
            DOTween.To(() => lastMoney, x => lastMoney = x, CurrentMoney, 1)
                .OnUpdate(() => currentMoneyText.text = lastMoney.ToString("F0"));
        }
    }
}
