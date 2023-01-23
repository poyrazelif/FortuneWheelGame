using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Prize ",menuName = "Prize")]
public class PrizeData : ScriptableObject
{
    public Sprite PrizeImage;
    public PrizeTypes PrizeType;
}
