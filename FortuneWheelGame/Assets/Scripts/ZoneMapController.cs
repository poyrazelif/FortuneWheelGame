using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ZoneMapController : MonoBehaviour
{
    private ScrollRect _scrollRect;
    [SerializeField]private GameObject _content;
    [SerializeField] private float scrollSnapAmount;
    [SerializeField] private TextMeshProUGUI[] _zoneTexts = new TextMeshProUGUI[21];
    private int _levelIndex=1;
    private int _tourCount = 0;

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        for (int i = 0; i < _content.transform.childCount; i++)
        {
           _zoneTexts[i]= _content.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
        }
        ResetMapText();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SnapToNext();
        }
        if (Input.GetMouseButtonDown(1))
        { 
            _scrollRect.horizontalNormalizedPosition = 0f;
        }
    }

    public void SnapToNext()
    {   
        _levelIndex++;
        DOTween.CompleteAll();
        float targetValue =_scrollRect.horizontalNormalizedPosition + .1f;
        DOTween.To(()=> _scrollRect.horizontalNormalizedPosition, x=> _scrollRect.horizontalNormalizedPosition = x, targetValue, .5f).OnComplete(
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
            _zoneTexts[i].text = (i+1+(_tourCount*10)).ToString();
        }
    }

    public void ResetMapText()
    {
        for (int i = 0; i < _zoneTexts.Length; i++)
        {
            _zoneTexts[i].text = (i+1).ToString();
        }
    }
}
