using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FortuneGame.Managers;
using Unity.VisualScripting;
using UnityEngine.Serialization;

namespace FortuneGame.GamePlay
{
    public class ZoneMapController : MonoBehaviour
    {
        [SerializeField] private GameObject scrollViewContent;
        private readonly TextMeshProUGUI[] _zoneTexts = new TextMeshProUGUI[21];
        private ScrollRect _scrollRect;
        private int _levelIndex = 1;
        private int _tourCount;
        
        private void Start()
        {
            _scrollRect = GetComponent<ScrollRect>();
            for (int i = 0; i < scrollViewContent.transform.childCount; i++)
            {
                _zoneTexts[i] = scrollViewContent.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
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
                    if (_levelIndex != 16) return;
                    _scrollRect.horizontalNormalizedPosition = 0;
                    _levelIndex = 6;
                    _tourCount++;
                    UpdateMapTexts();
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
