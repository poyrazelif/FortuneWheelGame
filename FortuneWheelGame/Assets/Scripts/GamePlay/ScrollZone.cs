using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneGame.GamePlay
{
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
         if (Input.touchCount > 0)
         {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
               SnapToNext();
            }
         }
      }

      public void SnapToNext()
      {

         _scrollRect.horizontalNormalizedPosition = 0;

      }


   }
}
