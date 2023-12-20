using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : ScriptableObject
{
    public string Name { get;  set; }
    public int Puntuation { get; set; }
    public bool Finished { get; set; }
}
