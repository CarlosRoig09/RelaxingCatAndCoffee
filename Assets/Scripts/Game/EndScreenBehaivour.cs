using personalLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenBehaivour : MonoBehaviour
{
    private const float END_SCREEN_TIME = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BlossomBehaivour blossomBehaivour))
        {
            LevelManager.Instance.OnEndScreenReach(END_SCREEN_TIME, blossomBehaivour, blossomBehaivour.GivePuntuation(EnumLibrary.PunType.Negative),blossomBehaivour.LoseEnergy());
        }
    }
}
