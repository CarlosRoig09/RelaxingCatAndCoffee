using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
public class EnergyController : ModificableValue
{
    public bool IsEnergyExahusted()
    {
        if(modValue>startValue)
            modValue = startValue;
        return modValue <= 0;
    }
}
