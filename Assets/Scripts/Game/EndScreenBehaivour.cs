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
            SendPuntuation(blossomBehaivour.GivePuntuation(EnumLibrary.PunType.Negative));
            SendEnergy(blossomBehaivour.LoseEnergy());
            blossomBehaivour.Destroy();
        }
    }

    private void SendPuntuation(int pun)
    {
        LevelManager.Instance.ModifyPuntuation(pun);
    }

    private void SendEnergy(int energy)
    {
        LevelManager.Instance.ModifyEnergy(energy);
    }
}
