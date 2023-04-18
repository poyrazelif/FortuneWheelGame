using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FortuneGame.GamePlay;
using FortuneGame.Core;
using UnityEngine.Serialization;

namespace FortuneGame.Managers
{
    public class Earning
    {
        public PrizeTypes EarningType;
        public int EarningTotalAmount;
        public GameObject EarningCard;
        public TextMeshProUGUI EarningText;
        public Image EarningImage;
    }

    public class EarningManager : Singleton<EarningManager>
    {
        public List<Earning> Earnings = new();
        private GameObject lastUpdatedCard;
        public GameObject LastUpdateCard => lastUpdatedCard;
        
        [FormerlySerializedAs("startChartPos")] [SerializeField] private RectTransform cardSortStartPos;
        [FormerlySerializedAs("earningChartSize")] [SerializeField]private float earningCardSize = 0;
        
        private void OnEnable()
        {
            EventManager.GameEnded += ResetEarnings;
        }

        private void OnDisable()
        {
            EventManager.GameEnded -= ResetEarnings;
        }

        public void AddPrize(PrizeData prizeData, int amount)
        {
            foreach (var earning in Earnings)
            {
                //Check if already have this earning
                if (earning.EarningType == prizeData.PrizeType)
                {
                    lastUpdatedCard = earning.EarningCard;
                    earning.EarningTotalAmount += amount;
                    earning.EarningText.text = earning.EarningTotalAmount.ToString();
                    return;
                }
            }
            //if haven't this earning; create a new card
            ConfigureNewCard(prizeData, amount);
        }

        private void ConfigureNewCard(PrizeData prizeData, int amount)
        {
            GameObject newCard = GetNewCardTemplate();
            Earning earning = new Earning
            {
                EarningCard = newCard,
                EarningImage = newCard.GetComponentInChildren<Image>(),
                EarningText = newCard.GetComponentInChildren<TextMeshProUGUI>(),
                EarningType = prizeData.PrizeType,
                EarningTotalAmount = amount
            };
            earning.EarningImage.sprite = prizeData.PrizeImage;
            earning.EarningText.text = earning.EarningTotalAmount.ToString();
            Earnings.Add(earning);
            SortCards();
        }

        private GameObject GetNewCardTemplate()
        {
            GameObject newCard = ObjectPool.Instance.GetFromPool("EarningCard");

            if (earningCardSize == 0)
                earningCardSize = 1.5f * newCard.GetComponent<RectTransform>().sizeDelta.y;
            
            newCard.transform.SetParent(transform);
            newCard.transform.localScale = Vector3.one;
            //newCard.transform.position = cardSortStartPos.position;
            
            lastUpdatedCard = newCard;
            return newCard;
        }

        private void SortCards()
        {
            for (int i = 0; i < Earnings.Count; i++)
            {
                Earnings[i].EarningCard.transform.position = cardSortStartPos.transform.position;
                Earnings[i].EarningCard.transform.position -= new Vector3(0,
                    i * (earningCardSize), 0);

                if (!Earnings[i].EarningCard.activeInHierarchy)
                    Earnings[i].EarningCard.SetActive(true);
            }
        }

        private void ResetEarnings()
        {
            ClearPanel();
            lastUpdatedCard = null;
            Earnings.Clear();
            SortCards();
        }

        private void ClearPanel()
        {
            foreach (var earning in Earnings)
            {
                ObjectPool.Instance.Deposit(earning.EarningCard);
            }
        }
    }
}
