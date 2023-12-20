using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CofeeData", menuName = "ScriptableObjects/Data/Item/CofeeData")]
public class CofeeData : ScriptableObject
{
    public int Energy;
    public float Countdown;
    public float Time; 
}
