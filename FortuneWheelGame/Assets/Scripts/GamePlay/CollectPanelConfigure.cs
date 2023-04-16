using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using FortuneGame.Core;
using FortuneGame.Managers;

namespace FortuneGame.GamePlay
{
    public class CollectPanelConfigure : MonoBehaviour
    {
        private EarningManager _earningManager;
        private Earning currentEarning;
        [SerializeField] private TextMeshProUGUI cardPrizeText;
        [SerializeField] private TextMeshProUGUI cardAmountText;
        [SerializeField] private Image cardPrizeImage;
        [SerializeField] private GameObject card;
        private float showTime = .5f;
        private bool canPass = true;
        private int sortNum = 0;
        private Coroutine waitCoroutine;

        private void Start()
        {
            EventManager.GameEnded += ResetConfigurePanel;
        }

        private void OnEnable()
        {
            canPass = true;
            ConfigureCard();
        }

        private void Update()
        {
            if (Input.touchCount > 0 && canPass)
            {
                if (sortNum != 0)
                    CardCloseAnim();
                ConfigureCard();
                CardOpenAnim();
            }
        }

        IEnumerator CO_WaitTime()
        {
            canPass = false;
            yield return new WaitForSeconds(showTime);
            canPass = true;
        }

        public void ClosePanel()
        {
            gameObject.transform.DOScale(Vector3.zero, .2f).OnComplete(() => { gameObject.SetActive(false); });
        }

        private Earning GetEarningInOrder()
        {
            _earningManager = EarningManager.Instance;
            if (sortNum < _earningManager.Earnings.Count)
            {
                return _earningManager.Earnings[sortNum];
            }

            return null;
        }

        private void CardCloseAnim()
        {
            card.transform.localScale = Vector3.one;
            card.transform.DOScale(Vector3.zero, .2f);
        }

        private void CardOpenAnim()
        {
            if (waitCoroutine != null)
                StopCoroutine(waitCoroutine);
            waitCoroutine = StartCoroutine(CO_WaitTime());
            card.transform.localScale = Vector3.zero;
            card.transform.DOScale(Vector3.one, .2f);
        }


        private void ConfigureCard()
        {
            currentEarning = GetEarningInOrder();
            if (currentEarning == null)
            {
                EventManager.GameEnd();
                ClosePanel();

            }
            else
            {
                if (currentEarning.EarningType == PrizeTypes.Cash)
                {
                    EconomyManager.Instance.AddMoney(currentEarning.EarningTotalAmount);
                }

                cardPrizeImage.sprite = currentEarning.EarningImage.sprite;
                cardPrizeText.text = currentEarning.EarningType.ToString();
                cardAmountText.text = "x" + currentEarning.EarningTotalAmount;
                sortNum++;
            }
        }

        public void ResetConfigurePanel()
        {
            currentEarning = null;
            sortNum = 0;
            if (waitCoroutine != null)
            {
                StopCoroutine(waitCoroutine);
            }

            ;
            canPass = true;
        }
    }
}
