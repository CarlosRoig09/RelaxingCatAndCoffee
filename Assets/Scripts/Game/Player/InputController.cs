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
        SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnRightClick, EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel });
        _catBehaivour = GameObjectLibrary.Instance.CatBehaivourScript;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubscribeEvents(EnumLibrary.Inputs[] inputs)
    {
        Debug.Log("EventSub");
        foreach (EnumLibrary.Inputs input in inputs)
        {
            switch (input)
            {
                case EnumLibrary.Inputs.OnLeftClick:
                    _onLeftClick.started += OnLeftClick;
                    break;
                case EnumLibrary.Inputs.OnRightClick:
                    _onRightClick.performed+= OnRightClick;
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
        Debug.Log("EventSub");
        foreach (EnumLibrary.Inputs input in inputs)
        {
            switch (input)
            {
                case EnumLibrary.Inputs.OnLeftClick:
                    _onLeftClick.started -= OnLeftClick;
                    break;
                case EnumLibrary.Inputs.OnRightClick:
                    _onRightClick.performed -= OnRightClick;
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
        _catBehaivour.Movement(context.ReadValue<Vector2>());
    }

    void OnScrollCancel(InputAction.CallbackContext context)
    {
        _catBehaivour.StopMovement();

    }

    void OnRightClick(InputAction.CallbackContext context)
    {
        Debug.Log("RightClick");
    }

    void OnLeftClick(InputAction.CallbackContext context)
    {
        Debug.Log("LeftClick");
        _catBehaivour.Attack();
        DesubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel});
    }

    public void MethodForEvent(object value)
    {
        SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel });
    }
}
