using UnityEngine;

[CreateAssetMenu(fileName = "BlossomData", menuName = "ScriptableObjects/Data/Enemy/BlossomData")]
public class BlossomData : ScriptableObject
{
    public string Name;
    public GameObject BlossomGameObject;
    public int PositivePuntuation;
    public int NegativePuntuation;
    public int EnergyLose;
    public float BaseMaxSppeedY;
    public float MaxSpeedX;
    public float MinSpeedX;
    public float MaxSpeedY;
    public float SpawnPercentage;  
}
