using UnityEngine;
public  abstract class ModificableValue : MonoBehaviour
{
    int Value { get; set; }

    public void ModifyValue(int mod)
    {
        Value += mod;
    }

    public int GetValue() { return Value; }
}

