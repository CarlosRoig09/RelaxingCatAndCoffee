using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class PuntuationController : ModificableValue
{
    public void IsPuntuationBelow0()
    {
        if(Value<0)
            Value=0;
    }
}
