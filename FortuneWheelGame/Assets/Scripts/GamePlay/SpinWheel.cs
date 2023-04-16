using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.UI;
using FortuneGame.Managers;
using FortuneGame.Core;

namespace FortuneGame.GamePlay
{
    public class SpinWheel : MonoBehaviour
    {
        private WheelConfigure _wheelConfigure;
        [SerializeField] private int currentIndex;
        [SerializeField] private Prize currentPrize;
        private bool isStopped = true;
        public float SecondSpeedDuration = 1.1f;
        public float spinPieceDuration = 1.2f;
        private Coroutine spinCoroutine;
        private Vector3 _firstItemPos;

        private void Start()
        {
            _wheelConfigure = GetComponentInParent<WheelConfigure>();
            _firstItemPos = _wheelConfigure.Prizes.transform.GetChild(0).transform.position;
            EventManager.SpinStarted += Spin;
        }

        private void Spin()
        {
            if (isStopped)
            {
                GetRandomPrize();
                spinCoroutine = StartCoroutine(nameof(CO_Spin));
            }
        }

        private void GetRandomPrize()
        {
            WheelInventory currentWheel = _wheelConfigure.GetWheelPrizes(GameManager.Instance.ActiveLevel);

            int randomNumber = Random.Range(0, 100);
            int _probalitiyCount = 0;
            currentIndex = 0;

            for (int i = 0; i < currentWheel.prizes.Length; i++)
            {
                if (_probalitiyCount <= randomNumber &&
                    randomNumber <= currentWheel.prizes[i].Probability + _probalitiyCount)
                {
                    currentIndex = i;
                    currentPrize = currentWheel.prizes[i];
                    break;
                }
                else
                {
                    _probalitiyCount += currentWheel.prizes[i].Probability;
                }
            }
        }

        IEnumerator CO_Spin()
        {
            isStopped = false;
            _wheelConfigure.CloseSpinButtons();

            for (int j = 0; j < Random.Range(1, 3); j++)
            {
                for (int i = 0; i < 15; i++)
                {
                    transform.Rotate(0, 0, -24);
                    yield return null;
                }
            }

            for (int j = 0; j < 1; j++)
            {
                for (int i = 0; i < 30; i++)
                {
                    transform.Rotate(0, 0, -12);
                    //yield return new WaitForSeconds(SecondSpeedDuration * Time.deltaTime);
                    yield return null;
                }
            }

            for (int i = 0; i < (currentIndex * 15) + 7; i++)
            {
                transform.Rotate(0, 0, -3);
                yield return null;
                //yield return new WaitForSeconds(spinPieceDuration*Time.deltaTime);
            }

            yield return new WaitForSeconds(.1f);

            for (int i = 0; i < 7; i++)
            {
                transform.Rotate(0, 0, 3);
                //yield return new WaitForSeconds(1.5f * Time.deltaTime);
                yield return null;
            }

            if (currentPrize.PrizeData.PrizeType != PrizeTypes.Death) CollectPrize();
            else EventManager.BombSelectedInvoke();

            isStopped = true;

        }

        public void CollectPrize()
        {
            GameObject tempImage = ObjectPool.Instance.GetFromPool("Image");

            tempImage.GetComponent<Image>().sprite = currentPrize.PrizeData.PrizeImage;
            tempImage.transform.SetParent(transform);
            tempImage.transform.localScale = Vector3.one;
            tempImage.transform.position = _firstItemPos;
            tempImage.transform.rotation = quaternion.Euler(Vector3.zero);

            EarningManager earningManager = EarningManager.Instance;
            earningManager.AddPrize(currentPrize.PrizeData, currentPrize.PrizeAmount);
            Vector3 targetPos = earningManager.LastUpdateChart.transform.position;

            tempImage.SetActive(true);
            tempImage.transform.DOMove(targetPos, .5f).OnComplete((() =>
            {
                GameManager.Instance.IncreaseLevel();
                ObjectPool.Instance.Deposit(tempImage);
            }));


        }


    }
}
