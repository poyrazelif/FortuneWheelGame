using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager :Singleton<GameManager>
{
    private int activeLevel=0;

    public int ActiveLevel
    {
        get => activeLevel;
    }

    public void IncreaseLevel()
    {
        activeLevel++;
        EventManager.NextLevel();
    }

    public void ResetLevels()
    {
        activeLevel = 0;
    }
}
