using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
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
    
    public delegate void OnBombSelected();
    public static event OnBombSelected BombSelected;
    
    public delegate void OnMoneyChange();
    public static event OnMoneyChange MoneyChanged;
    
    public delegate void OnRevive();
    public static event OnRevive Revived;
    
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
    
    public static void BombSelectedInvoke()
    {
        BombSelected?.Invoke();
    }
    
    public static void MoneyChangedInvoke()
    {
        MoneyChanged?.Invoke();
    }
    public static void ReviveInvoke()
    {
        Revived?.Invoke();
    }
    

   
    
    

    
}
