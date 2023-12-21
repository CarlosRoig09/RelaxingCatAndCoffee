using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
public class EnergyController : MonoBehaviour, IModificableValue
{
    [SerializeField]
    private int _maxEnergy;
    private int _energy;
    public int Value { get => _energy; set => _energy = value; }

    // Start is called before the first frame update
    void Start()
    {
        Value= _maxEnergy;
    }

    public bool IsEnergyExahusted()
    {
        if(_energy>_maxEnergy)
            _energy = _maxEnergy;
        return _energy <= 0;
    }
}
