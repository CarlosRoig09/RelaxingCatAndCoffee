using Interfaces;
using Newtonsoft.Json.Linq;
using personalLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionInventory : MonoBehaviour, IWaitTheEvent, IHaveTheEvent
{
    [SerializeField]
    private Inventory _inventory;
    public EnumLibrary.TypeOfEvent Type => EnumLibrary.TypeOfEvent.AddACofee;

    EnumLibrary.TypeOfEvent IHaveTheEvent.Type { get => EnumLibrary.TypeOfEvent.StopCofeeProduction; set => throw new System.NotImplementedException(); }

    public event IHaveTheEvent.IHaveTheEvent IHTEvent;

    public void MethodForEvent(object value)
    {
        AddCofee((CofeeData)value);
    }

    // Start is called before the first frame update
    void Start()
    {
        _inventory= new Inventory();
        _inventory.Cofees = new Queue<CofeeData>();
        GameManager.Instance.SubscribeEvent(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddCofee(CofeeData cofeeData)
    {
        if (_inventory.Cofees.Count < _inventory.Limit)
        {
            _inventory.Cofees.Enqueue(cofeeData);
            UIManager.Instance.AddCofeeHUD();
        }
        else
        {
            object placeholder = "";
            IHTEvent(placeholder);
        }
    }

    public void UseCofee()
    {
        if (_inventory.Cofees.Count > 0)
        {
            LevelManager.Instance.ModifyEnergy(_inventory.Cofees.Peek().Energy);
            var destroyCofee = _inventory.Cofees.Dequeue();
            Destroy(destroyCofee);
        }
    }
}
