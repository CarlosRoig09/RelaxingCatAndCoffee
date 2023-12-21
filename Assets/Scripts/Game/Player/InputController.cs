using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using personalLibrary;
using Interfaces;

public class InputController : MonoBehaviour, IWaitTheEvent
{
    private PlayerInput _playerInput;
    private InputAction _onScroll;
    private InputAction _onLeftClick;
    private InputAction _onRightClick;
    private CatBehaivour _catBehaivour;
    private GestionInventory _gestionInventory;
    private float _lastKeyValue;

    public EnumLibrary.TypeOfEvent Type => EnumLibrary.TypeOfEvent.AttackFinish;

    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            _playerInput = GetComponent<PlayerInput>();
            _onScroll = _playerInput.actions["Movement"];
            _onLeftClick = _playerInput.actions["Attack"];
            _onRightClick = _playerInput.actions["Drink"];
        }
        catch { Debug.LogError("ERROR: PlayerInput component is missing"); }
    }
    private void Start()
    {
        GameManager.Instance.SubscribeEvent(this);
        SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel});
        _catBehaivour = GameObjectLibrary.Instance.CatBehaivourScript;
        _gestionInventory = GameObjectLibrary.Instance.GestionInventory;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubscribeEvents(EnumLibrary.Inputs[] inputs)
    {
        foreach (EnumLibrary.Inputs input in inputs)
        {
            switch (input)
            {
                case EnumLibrary.Inputs.OnLeftClick:
                    _onLeftClick.started += OnLeftClick;
                    _onLeftClick.canceled += OnLeftCanceled;
                    break;
                case EnumLibrary.Inputs.OnRightClick:
                    _onRightClick.performed+= OnRightClick;
                    _onRightClick.canceled += OnRightCanceled;
                    break; 
                case EnumLibrary.Inputs.OnScroll:
                    _onScroll.started += OnScroll;
                    break;
                case EnumLibrary.Inputs.OnScrollCancel:
                    _onScroll.canceled += OnScrollCancel;
                    break;
                default:
                    Debug.LogError("Error: This input doesn't exist");
                    break;
            }
        }
    }

    public void DesubscribeEvents(EnumLibrary.Inputs[] inputs)
    {
        foreach (EnumLibrary.Inputs input in inputs)
        {
            switch (input)
            {
                case EnumLibrary.Inputs.OnLeftClick:
                    _onLeftClick.started -= OnLeftClick;
                    break;
                case EnumLibrary.Inputs.OnRightClick:
                    _onRightClick.performed -= OnRightClick;
                    _onRightClick.canceled -= OnRightCanceled;
                    break;
                case EnumLibrary.Inputs.OnScroll:
                    _onScroll.started -= OnScroll;
                    break;
                case EnumLibrary.Inputs.OnScrollCancel:
                    _onScroll.canceled -= OnScrollCancel;
                    break;
                default:
                    Debug.LogError("Error: This input doesn't exist");
                    break;
            }
        }
    }

    void OnScroll(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().x>0)
        {
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.D);
        }
        else { 
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.A);
            }
        _lastKeyValue = context.ReadValue<Vector2>().x;
        _catBehaivour.Movement(context.ReadValue<Vector2>());
    }

    void OnScrollCancel(InputAction.CallbackContext context)
    {
        if (_lastKeyValue> 0)
        {
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.D);
        }
        else
        {
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.A);
        }
        _catBehaivour.StopMovement();

    }

    void OnRightClick(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Shift);
        _gestionInventory.UseCofee();
    }

    void OnRightCanceled(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Shift);
    }

    void OnLeftClick(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Enter);
            Debug.Log("LeftClick");
            DesubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel });
            _catBehaivour.Attack();
    }
    void OnLeftCanceled(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Enter);
    }

    public void MethodForEvent(object value)
    {
        Debug.Log("SuscribeteAAAAAAAAA");
        SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel, EnumLibrary.Inputs.OnLeftClick});
    }
}
