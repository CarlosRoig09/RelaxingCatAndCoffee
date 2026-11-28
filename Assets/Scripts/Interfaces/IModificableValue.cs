using Unity.VisualScripting;
using UnityEngine;
public  abstract class ModificableValue : MonoBehaviour
{
    public int Value { get=> modValue; set=> modValue = value; }
    protected int modValue;
    [SerializeField]
    protected int startValue = 0;

    protected virtual void Start()
    {
        modValue = startValue;
    }
    public virtual void ModifyValue(int mod)
    {
        Value += mod;
    }

    public int GetValue() { return Value; }
}

