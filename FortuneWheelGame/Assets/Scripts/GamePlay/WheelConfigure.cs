using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FortuneGame.Core;
using FortuneGame.Managers;
using UnityEngine.Serialization;

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

    [System.Serializable]
    public class WheelSkin
    {
        public int SkinRepeatFactor;
        public GameObject IndicatorSkin;
        public GameObject SpinSkin;
        public GameObject SpinButtonSkin;
    }

    public class WheelConfigure : MonoBehaviour
    {
        public GameObject PrizeTemplatesParent;
        public GameObject SpinsParent;
        
        [SerializeField] private GameObject indicatorsParent;
        [SerializeField] private GameObject spinButtons;
        [SerializeField] private List<WheelInventory> wheelLevels = new();
        [SerializeField] private List<WheelSkin> wheelSkins = new();
        
        private TextMeshProUGUI[] _prizeTexts = new TextMeshProUGUI[8];
        private Image[] _prizeImages = new Image[8];
        private Sequence sequence;
        private float wheelAnimPunchDuration=0.5f;
        private float wheelAnimPunchPower=0.2f;
        
        
        private void Start()
        {
            ExtractTemplatePrizeComponents();
            SortWheelSkins();
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
        private void ExtractTemplatePrizeComponents()
        {
            for (int i = 0; i < PrizeTemplatesParent.transform.childCount; i++)
            {
                _prizeTexts[i] = PrizeTemplatesParent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
                _prizeImages[i] = PrizeTemplatesParent.transform.GetChild(i).GetComponent<Image>();
            }
        }

        private void CloseAllWheel()
        {
            for (int i = 0; i < wheelSkins.Count; i++)
            {
                wheelSkins[i].IndicatorSkin.SetActive(false);
                wheelSkins[i].SpinSkin.SetActive(false);
                wheelSkins[i].SpinButtonSkin.SetActive(false);
            }
        }

        private void SortWheelSkins()
        {
            WheelSkin temp;
            for (int i = 0; i < wheelSkins.Count-1; i++)
            {
                for (int j = i; j < wheelSkins.Count; j++)
                {
                    if (wheelSkins[i].SkinRepeatFactor < wheelSkins[j].SkinRepeatFactor)
                    {
                        temp = wheelSkins[j];
                        wheelSkins[j] = wheelSkins[i];
                        wheelSkins[i] = temp;
                    }
                }
            }
        }

        private void ConfigureWheel()
        {
            int level = GameManager.Instance.ActiveLevel;
            SpinsParent.transform.rotation = Quaternion.Euler(Vector3.zero);
            CloseAllWheel();
            
            for (int i = 0; i < wheelSkins.Count; i++)
            {
                if ((level + 1) % wheelSkins[i].SkinRepeatFactor == 0)
                {
                    wheelSkins[i].IndicatorSkin.SetActive(true);
                    wheelSkins[i].SpinSkin.SetActive(true);
                    wheelSkins[i].SpinButtonSkin.SetActive(true);
                    break;
                }
            }
            WheelInventory currentWheelPrizes = GetWheelPrizes(level);

            for (int i = 0; i < _prizeTexts.Length; i++)
            {
                _prizeTexts[i].text = "X" + currentWheelPrizes.prizes[i].PrizeAmount;
                _prizeImages[i].sprite = currentWheelPrizes.prizes[i].PrizeData.PrizeImage;
            }
        }

        public void WheelAnim()
        {
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOPunchScale(Vector3.one * wheelAnimPunchPower, wheelAnimPunchDuration));
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
            if (Level >= wheelLevels.Count)
            {
                int equalLevel = Level % wheelLevels.Count;
                return wheelLevels[equalLevel];
            }
            return wheelLevels[Level];
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
