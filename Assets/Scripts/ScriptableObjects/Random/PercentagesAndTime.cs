using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlossomData", menuName = "ScriptableObjects/Data/Time/PercentagesAndTimeSO")]
public class PercentagesAndTime : ScriptableObject
{
    public List<float> Percentages = new List<float>();
    public List<float> Time = new List<float>();
}
