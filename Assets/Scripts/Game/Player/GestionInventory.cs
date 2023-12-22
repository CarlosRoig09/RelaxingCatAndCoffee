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
    private Inventory _cloneInventory;
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
         _cloneInventory = Instantiate(_inventory);
        _cloneInventory.Cofees = new Queue<CofeeData>();
        GameManager.Instance.SubscribeEvent(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddCofee(CofeeData cofeeData)
    {
            _cloneInventory.Cofees.Enqueue(cofeeData);
            UIManager.Instance.AddCofeeHUD();
        ComprobeCapacity();
    }

    private bool ComprobeCapacity()
    {
        if (_cloneInventory.Cofees.Count < _cloneInventory.Limit)
        {
            IHTEvent(false);
            return true;
        }
        else
        {
            IHTEvent(true);
            return false;
        }
    }

    public void UseCofee()
    {
        if (_cloneInventory.Cofees.Count > 0)
        {
            AudioManager.instance.Play("cofeeDrink");
            LevelManager.Instance.ModifyEnergy(_cloneInventory.Cofees.Peek().Energy);
            var destroyCofee = _cloneInventory.Cofees.Dequeue();
            Destroy(destroyCofee);
            UIManager.Instance.RemoveCofeeHUD();
            ComprobeCapacity();
        }
    }
}
