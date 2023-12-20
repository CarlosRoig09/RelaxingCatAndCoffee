using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class PuntuationController : MonoBehaviour, IModificableValue
{
    [SerializeField]
    private int _puntuation;
    public int Value { get => _puntuation; set => _puntuation = value; }

    // Start is called before the first frame update
    void Start()
    {
        _puntuation= 0;
    }

    public void IsPuntuationBelow0()
    {
        if(Value<0)
            Value=0;
    }
}
