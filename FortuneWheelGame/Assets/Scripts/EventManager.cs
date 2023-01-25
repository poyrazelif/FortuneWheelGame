using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //public static event Action<ProductData> onSelectedObjChanged;

    //public static event Action<GameObject> onProductPanelSpawnedObj;

    /*public static void SelectedObjectChanged(ProductData productData)
    {
        onSelectedObjChanged?.Invoke(productData);
    }*/

    /*public static void ProductPanelSpawnedObject(GameObject gameObject)
    {
        onProductPanelSpawnedObj?.Invoke(gameObject);
    }*/
    public delegate void OnGameEnd();
    public static event OnGameEnd GameEnded;

    public delegate void OnStartSpin();
    public static event OnStartSpin SpinStarted;

    public delegate void OnPassedNextLevel();
    public static event OnPassedNextLevel PassedNextLevel;
    
    public delegate void OnCollectRequest();
    public static event OnCollectRequest CollectRewardsRequest;
    
    public delegate void OnSpinFinish();
    public static event OnSpinFinish SpinFinished;
    
    public static void GameEnd()
    {
        GameEnded?.Invoke();
    }
    
    public static void SpinStartInvoke()
    {
        SpinStarted?.Invoke();
    }
    
    public static void NextLevel()
    {
        PassedNextLevel?.Invoke();
    }
    
    public static void CollectRequestInvoke()
    {
        CollectRewardsRequest?.Invoke();
    }
    
    public static void SpinFinishInvoke()
    {
        SpinFinished?.Invoke();
    }

   
    
    

    
}
