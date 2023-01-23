using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpinWheel : MonoBehaviour
{
    private WheelConfigure _wheelConfigure;
    [SerializeField]private int currentIndex;
    [SerializeField] private Prize currentPrize;
    private bool isStopped = true;
    public float SecondSpeedDuration=1.1f;
    public float spinPieceDuration=1.3f;
    private Coroutine spinCoroutine;
    private GameManager _gameManager;

    private void Start()
    {
        _wheelConfigure = GetComponentInParent<WheelConfigure>();
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && isStopped)
            {
                Debug.Log("aaaa");
                GetRandomPrize();
                spinCoroutine = StartCoroutine(nameof(CO_Spin));
                _gameManager.IncreaseLevel();
            }
        }
    }

    private void GetRandomPrize()
    {
        WheelInventory currentWheel = _wheelConfigure.WheelLevels[_gameManager.ActiveLevel];
        
        int randomNumber = Random.Range(0, 100);
        Debug.Log(randomNumber+"randomnumber");
        int _probalitiyCount = 0;
        currentIndex = 0;
        
        for (int i = 0; i < currentWheel.prizes.Length; i++)
        {
            if (_probalitiyCount <= randomNumber && randomNumber <= currentWheel.prizes[i].Probability + _probalitiyCount)
            {
                Debug.Log("gg");
                    currentIndex = i;
                currentPrize= currentWheel.prizes[i];
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
        for (int j = 0; j < Random.Range(1, 3); j++)
        {
            for (int i = 0; i < 90 ; i++)
            {
                transform.Rotate(0, 0, -4);
                yield return null;
            }
        }

        for (int j = 0; j < Random.Range(1, 2); j++)
        {
            for (int i = 0; i < 180 ; i++)
            {
                transform.Rotate(0, 0, -2);
                yield return new WaitForSeconds(SecondSpeedDuration * Time.deltaTime);
            }
        }
        
        for (int i = 0; i < (currentIndex*45)+20; i++)
        {
            transform.Rotate(0, 0, -1);
            yield return new WaitForSeconds(spinPieceDuration*Time.deltaTime);
        }
        
        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < 20; i++)
        {
            transform.Rotate(0, 0, 1);
            yield return new WaitForSeconds(1.5f * Time.deltaTime);
        }
        isStopped = true;

    }
    
   
}
