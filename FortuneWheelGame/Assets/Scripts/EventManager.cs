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
    public delegate void OnBombSelected();

    public static event OnBombSelected BombSelected;

    public delegate void OnPrizeSelected();

    public static event OnPrizeSelected PrizeSelected;

    public delegate void OnPassedNextLevel();

    public static event OnPassedNextLevel PassedNextLevel;

    public static void NextLevel()
    {
        PassedNextLevel?.Invoke();
    }

    
}
