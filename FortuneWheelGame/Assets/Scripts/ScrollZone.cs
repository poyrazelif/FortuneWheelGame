using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollZone : MonoBehaviour
{
   private ScrollRect _scrollRect;
   [SerializeField] private float scrollSnapAmount;

   private void Start()
   {
      _scrollRect = GetComponent<ScrollRect>();
   }

   public void Update()
   {
      if (Input.GetMouseButtonDown(0))
      {
         SnapToNext();
      }
      if (Input.GetMouseButtonDown(1))
      { Debug.Log("sss");
          _scrollRect.horizontalNormalizedPosition = 1;
      }
   }

   public void SnapToNext()
   {
     
      _scrollRect.horizontalNormalizedPosition = 0;
      
   }
   
   
}
