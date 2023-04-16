using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FortuneGame.Managers;
using Unity.VisualScripting;

namespace FortuneGame.GamePlay
{
    public class ZoneMapController : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        [SerializeField] private GameObject _content;
        [SerializeField] private float scrollSnapAmount;
        [SerializeField] private TextMeshProUGUI[] _zoneTexts = new TextMeshProUGUI[21];
        private int _levelIndex = 1;
        private int _tourCount = 0;

        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
            for (int i = 0; i < _content.transform.childCount; i++)
            {
                _zoneTexts[i] = _content.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
            }

            
            ResetMapText();
        }

        private void OnEnable()
        {
            EventManager.PassedNextLevel += SnapToNext;
            EventManager.GameEnded += ResetMapText;
            EventManager.GameEnded += ResetMapPosition;
        }

        private void OnDisable()
        {
            EventManager.PassedNextLevel -= SnapToNext;
            EventManager.GameEnded -= ResetMapText;
            EventManager.GameEnded -= ResetMapPosition;
        }

        public void SnapToNext()
        {
            _levelIndex++;
            float targetValue = _scrollRect.horizontalNormalizedPosition + .1f;
            DOTween.To(() => _scrollRect.horizontalNormalizedPosition,
                x => _scrollRect.horizontalNormalizedPosition = x, targetValue, .5f).OnComplete(
                () =>
                {
                    if (_levelIndex == 16)
                    {
                        _scrollRect.horizontalNormalizedPosition = 0;
                        _levelIndex = 6;
                        _tourCount++;
                        UpdateMapTexts();
                    }
                });
        }

        public void UpdateMapTexts()
        {
            for (int i = 0; i < _zoneTexts.Length; i++)
            {
                _zoneTexts[i].text = (i + 1 + (_tourCount * 10)).ToString();
            }
        }

        public void ResetMapText()
        {
            for (int i = 0; i < _zoneTexts.Length; i++)
            {
                _zoneTexts[i].text = (i + 1).ToString();
            }
        }

        public void ResetMapPosition()
        {
            _scrollRect.horizontalNormalizedPosition = -0.5f;
        }
    }
}
