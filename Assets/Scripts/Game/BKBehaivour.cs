using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using personalLibrary;
using Interfaces;

public class BKBehaivour : MonoBehaviour, IWaitTheEvent, IHaveTheEvent
{
    private int cofeeCount;
    private bool countCofee;
    [SerializeField]
    private int _obtainCofee;
    [SerializeField]
    private CofeeData _cofeeData;

    public event IHaveTheEvent.IHaveTheEvent IHTEvent;

    public EnumLibrary.TypeOfEvent Type => EnumLibrary.TypeOfEvent.StopCofeeProduction;

    EnumLibrary.TypeOfEvent IHaveTheEvent.Type { get => EnumLibrary.TypeOfEvent.AddACofee; set => throw new System.NotImplementedException(); }

    // Start is called before the first frame update
    void Start()
    {
        cofeeCount= 0;
        countCofee= true;
        GameManager.Instance.SubscribeEvent(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out BlossomBehaivour blossomBehaivour))
        {
            SendPuntuation(blossomBehaivour.GivePuntuation(EnumLibrary.PunType.Positive));
            blossomBehaivour.Destroy();
            CountCofee();
            if(cofeeCount==_obtainCofee)
            {
                SendCofee();
            }
        }
    }

    private void SendPuntuation(int pun)
    {
        LevelManager.Instance.ModifyPuntuation(pun);
    }

    private void CountCofee()
    {
        if(countCofee)
        {
            cofeeCount += 1;
        }
    }

    private void SendCofee()
    {
        IHTEvent(Instantiate(_cofeeData));
        cofeeCount = 0;
    }

    private void StopCofeeCount()
    {
        countCofee = false;
        cofeeCount= 0; 
    }

    public void MethodForEvent(object value)
    {
        StopCofeeCount();
    }
}
