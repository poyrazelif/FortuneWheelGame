using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class WheelInventory
{
    public Prize[] prizes = new Prize[8];
}

[System.Serializable]
public class Prize
{
    public PrizeData PrizeData;
    public int PrizeAmount;
    public int Probability;
}
public class WheelConfigure : MonoBehaviour
{
    public GameObject Prizes;
    public List<WheelInventory> WheelLevels = new List<WheelInventory>();
    [SerializeField]private GameObject spinsParent;
    [SerializeField]private GameObject indicatorsParent;
    
    private TextMeshProUGUI[] _prizeTexts = new TextMeshProUGUI[8];
    private Image[] _prizeImages = new Image[8];

    private void Start()
    {
        for (int i = 0; i < Prizes.transform.childCount; i++)
        {
            _prizeTexts[i]= Prizes.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
            _prizeImages[i] = Prizes.transform.GetChild(i).GetComponent<Image>();
        }
        ConfigureWheel(30);
    }

    private void ConfigureWheel(int Level)
    {
        for (int i = 0; i < indicatorsParent.transform.childCount; i++)
        {
            indicatorsParent.transform.GetChild(i).gameObject.SetActive(false);
            spinsParent.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (Level % 30 == 0)
        {
            indicatorsParent.transform.GetChild(2).gameObject.SetActive(true);
            spinsParent.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (Level % 5 == 0)
        {
            indicatorsParent.transform.GetChild(1).gameObject.SetActive(true);
            spinsParent.transform.GetChild(1).gameObject.SetActive(true);
        }
        else{
            indicatorsParent.transform.GetChild(0).gameObject.SetActive(true);
            spinsParent.transform.GetChild(0).gameObject.SetActive(true);
            
        }
        WheelInventory currentWheelPrizes = WheelLevels[Level];
        
        for (int i = 0; i < _prizeTexts.Length; i++)
        {
            _prizeTexts[i].text = "X" + currentWheelPrizes.prizes[i].PrizeAmount;
            _prizeImages[i].sprite = currentWheelPrizes.prizes[i].PrizeData.PrizeImage;
        }
        
    }
    
}

public enum PrizeTypes
{
    Cash,
    Gold,
    ConsGrenadeElectric,
    ConsGrenadeM67,
    ConsGrenadeSnowball,
    ConsHealthshot2,
    ConsHealthshot2Adrenaline,
    ConsMedkitEaster,
    ConsC4,
    ConsGrenadeEmp,
}
