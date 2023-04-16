using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FortuneGame.Managers;

namespace FortuneGame.GamePlay
{
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
        public GameObject spinsParent;

        [SerializeField] private GameObject indicatorsParent;
        [SerializeField] private GameObject spinButtons;

        private TextMeshProUGUI[] _prizeTexts = new TextMeshProUGUI[8];
        private Image[] _prizeImages = new Image[8];
        private Sequence sequence;

        private void Start()
        {
            for (int i = 0; i < Prizes.transform.childCount; i++)
            {
                _prizeTexts[i] = Prizes.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
                _prizeImages[i] = Prizes.transform.GetChild(i).GetComponent<Image>();
            }

            ConfigureWheel();
        }

        private void OnEnable()
        {
            EventManager.PassedNextLevel += ConfigureWheel;
            EventManager.PassedNextLevel += WheelAnim;
            EventManager.GameEnded += ConfigureWheel;
            EventManager.Revived += ConfigureWheel;
        }

        private void OnDisable()
        {
            EventManager.PassedNextLevel -= ConfigureWheel;
            EventManager.PassedNextLevel -= WheelAnim;
            EventManager.GameEnded -= ConfigureWheel;
            EventManager.Revived -= ConfigureWheel;
        }

        private void ConfigureWheel()
        {
            int Level = GameManager.Instance.ActiveLevel;
            spinsParent.transform.rotation = Quaternion.Euler(Vector3.zero);

            for (int i = 0; i < indicatorsParent.transform.childCount; i++)
            {
                indicatorsParent.transform.GetChild(i).gameObject.SetActive(false);
                spinsParent.transform.GetChild(i).gameObject.SetActive(false);
            }

            if ((Level + 1) % 30 == 0)
            {
                indicatorsParent.transform.GetChild(2).gameObject.SetActive(true);
                spinsParent.transform.GetChild(2).gameObject.SetActive(true);
                spinButtons.transform.GetChild(2).gameObject.SetActive(true);
            }
            else if ((Level + 1) % 5 == 0)
            {
                indicatorsParent.transform.GetChild(1).gameObject.SetActive(true);
                spinsParent.transform.GetChild(1).gameObject.SetActive(true);
                spinButtons.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                indicatorsParent.transform.GetChild(0).gameObject.SetActive(true);
                spinsParent.transform.GetChild(0).gameObject.SetActive(true);
                spinButtons.transform.GetChild(0).gameObject.SetActive(true);

            }

            WheelInventory currentWheelPrizes = GetWheelPrizes(Level);

            for (int i = 0; i < _prizeTexts.Length; i++)
            {
                _prizeTexts[i].text = "X" + currentWheelPrizes.prizes[i].PrizeAmount;
                _prizeImages[i].sprite = currentWheelPrizes.prizes[i].PrizeData.PrizeImage;
            }
        }

        public void WheelAnim()
        {
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOPunchScale(Vector3.one * 0.2f, .5f));
        }

        public void CloseSpinButtons()
        {
            for (int i = 0; i < spinButtons.transform.childCount; i++)
            {
                spinButtons.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public WheelInventory GetWheelPrizes(int Level)
        {
            if (Level >= WheelLevels.Count)
            {
                int equalLevel = Level % WheelLevels.Count;
                return WheelLevels[equalLevel];
            }
            else
            {
                return WheelLevels[Level];
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
        Death,
    }
}
