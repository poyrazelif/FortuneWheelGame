using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Earning
{
    public PrizeTypes EarningType;
    public int EarningTotalAmount;
    public GameObject EarningChart;
    public TextMeshProUGUI EarningText;
    public Image EarningImage;

}
public class EarningManager : Singleton<EarningManager>
{
    public List<Earning> Earnings = new List<Earning>();
    private ObjectPool _objectPool;
    [SerializeField] private RectTransform startChartPos;
    [SerializeField] private int listSpace;

    private GameObject lastUpdatedChart;
    public GameObject LastUpdateChart
    {
        get { return lastUpdatedChart; }
    }

    private void Start()
    {
        _objectPool = ObjectPool.Instance;
    }

    public void AddPrize(PrizeData prizeData,int amount)
    {
        foreach (var earning in Earnings)
        {
            if (earning.EarningType == prizeData.PrizeType)
            {
                lastUpdatedChart = earning.EarningChart;
                earning.EarningTotalAmount += amount;
                earning.EarningText.text =  earning.EarningTotalAmount.ToString();
                return;
            }
        }
        
        CreateChart(prizeData,amount);
        
    }

    public void CreateChart(PrizeData prizeData,int amount)
    {
        GameObject newChart= _objectPool.GetFromPool("EarningChart");
        newChart.transform.parent =transform;
        newChart.transform.position=startChartPos.position;
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
            Earnings[i].EarningChart.transform.position = new Vector3(startChartPos.transform.position.x,
                startChartPos.transform.position.y - (i * (listSpace+100)),
                startChartPos.transform.position.z);
            
           if(!Earnings[i].EarningChart.activeInHierarchy) 
               Earnings[i].EarningChart.SetActive(true);
        }
    }
}
