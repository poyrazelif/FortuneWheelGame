using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using FortuneGame.GamePlay;
using FortuneGame.Core;

namespace FortuneGame.Managers
{
    public class Earning
    {
        public PrizeTypes EarningType;
        public int EarningTotalAmount;
        public GameObject EarningChart;
        public TextMeshProUGUI EarningText;
        public Image EarningImage;

    }

    public class EarningManager : Core.Singleton<EarningManager>
    {
        public List<Earning> Earnings = new List<Earning>();
        [SerializeField] private RectTransform startChartPos;
        [SerializeField] private int listSpace;
        public float earningChartSize = 0;

        private GameObject lastUpdatedChart;

        public GameObject LastUpdateChart
        {
            get { return lastUpdatedChart; }
        }

        private void Start()
        {
            EventManager.GameEnded += ResetEarnings;
        }



        public void AddPrize(PrizeData prizeData, int amount)
        {
            foreach (var earning in Earnings)
            {
                if (earning.EarningType == prizeData.PrizeType)
                {
                    lastUpdatedChart = earning.EarningChart;
                    earning.EarningTotalAmount += amount;
                    earning.EarningText.text = earning.EarningTotalAmount.ToString();
                    return;
                }
            }

            CreateChart(prizeData, amount);

        }

        public void CreateChart(PrizeData prizeData, int amount)
        {
            GameObject newChart = ObjectPool.Instance.GetFromPool("EarningChart");

            if (earningChartSize == 0)
            {
                earningChartSize = newChart.GetComponent<RectTransform>().sizeDelta.y;
            }

            newChart.transform.SetParent(transform);
            newChart.transform.localScale = Vector3.one;
            newChart.transform.position = startChartPos.position;
            lastUpdatedChart = newChart;

            Earning earning = new Earning();

            earning.EarningChart = newChart;
            earning.EarningImage = newChart.GetComponentInChildren<Image>();
            earning.EarningText = newChart.GetComponentInChildren<TextMeshProUGUI>();
            earning.EarningType = prizeData.PrizeType;
            earning.EarningTotalAmount = amount;

            earning.EarningImage.sprite = prizeData.PrizeImage;
            earning.EarningText.text = earning.EarningTotalAmount.ToString();

            Earnings.Add(earning);
            SortList();

        }

        public void SortList()
        {
            for (int i = 0; i < Earnings.Count; i++)
            {
                Earnings[i].EarningChart.transform.position = startChartPos.transform.position;
                Earnings[i].EarningChart.transform.position -= new Vector3(0,
                    i * (earningChartSize), 0);

                if (!Earnings[i].EarningChart.activeInHierarchy)
                    Earnings[i].EarningChart.SetActive(true);
            }
        }

        private void ResetEarnings()
        {
            ClearPanel();
            lastUpdatedChart = null;
            Earnings.Clear();
            SortList();

        }

        private void ClearPanel()
        {
            foreach (var earning in Earnings)
            {
                ObjectPool.Instance.Deposit(earning.EarningChart);
            }
        }
    }
}
