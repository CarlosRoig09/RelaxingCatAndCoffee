using personalLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenBehaivour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BlossomBehaivour blossomBehaivour))
        {
            StartCoroutine(LevelManager.Instance.StopTillTime(0.5f,new System.Func<object, object>[] {SendPuntuation,SendEnergy, blossomBehaivour.Destroy},new object[] { blossomBehaivour.GivePuntuation(EnumLibrary.PunType.Negative), blossomBehaivour.LoseEnergy(),null}));
        }
    }

    private object SendPuntuation(object pun)
    {
        LevelManager.Instance.ModifyPuntuation((int)pun);
        return null;
    }

    private object SendEnergy(object energy)
    {
        LevelManager.Instance.ModifyEnergy((int)energy);
        return null;
    }
}
