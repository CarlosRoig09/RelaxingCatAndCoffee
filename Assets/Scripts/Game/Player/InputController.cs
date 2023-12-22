using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using personalLibrary;
using Interfaces;
using Unity.VisualScripting;

public class InputController : MonoBehaviour, IWaitTheEvent
{
    private static InputController _instance;
    private PlayerInput _playerInput;
    [SerializeField]
    private InputActionAsset _actionAsset;
    private InputAction _onScroll;
    private InputAction _onLeftClick;
    private InputAction _onRightClick;
    private InputAction _onEscClick;
    private CatBehaivour _catBehaivour;
    private GestionInventory _gestionInventory;
    private float _lastKeyValue;

    public EnumLibrary.TypeOfEvent Type => EnumLibrary.TypeOfEvent.AttackFinish;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        try
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.actions = _actionAsset;
            _onScroll = _playerInput.actions["Movement"];
            _onLeftClick = _playerInput.actions["Attack"];
            _onRightClick = _playerInput.actions["Drink"];
            _onEscClick = _playerInput.actions["Pause"];
        }
        catch { Debug.LogError("ERROR: PlayerInput component is missing"); }
    }
    private void Start()
    {
        GameManager.Instance.OnStartGame += OnStartGame;
    }

    private void OnStartGame()
    {
        GameManager.Instance.SubscribeEvent(this);
        _catBehaivour = GameObjectLibrary.Instance.CatBehaivourScript;
        _gestionInventory = GameObjectLibrary.Instance.GestionInventory;
        InputAction.CallbackContext context = new InputAction.CallbackContext();
        OnEscClick(context);
        OnEscClickSecond(context);
        SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel, EnumLibrary.Inputs.OnEscClick });
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
                case EnumLibrary.Inputs.OnEscClick:
                    _onEscClick.started += OnEscClick;
                    _onEscClick.canceled += OnEscCanceled;
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
                    break;
                case EnumLibrary.Inputs.OnScroll:
                    _onScroll.started -= OnScroll;
                    break;
                case EnumLibrary.Inputs.OnScrollCancel:
                    _onScroll.canceled -= OnScrollCancel;
                    break;
                case EnumLibrary.Inputs.OnEscClick:
                    _onEscClick.started-= OnEscClick;
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
            DesubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel});
            _catBehaivour.Attack();
    }
    void OnLeftCanceled(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Enter);
    }

    void OnEscClick(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Esc);
        LevelManager.Instance.PauseGame();
        _onEscClick.canceled += OnEscCanceled;
        DesubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnEscClick,EnumLibrary.Inputs.OnLeftClick});
        _onLeftClick.canceled -= OnLeftCanceled;
        _onEscClick.started += OnEscClickSecond;
        _onLeftClick.canceled -= OnLeftCanceled;
    }

    void OnEscClickSecond(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Esc);
        _onLeftClick.canceled += OnLeftCanceled;
        LevelManager.Instance.ResumeGame();
        _onEscClick.started-= OnEscClickSecond;
        _onEscClick.canceled -= OnEscCanceled;
    }

    void OnEscCanceled(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Esc);
    }

    public void MethodForEvent(object value)
    {
        Debug.Log("SuscribeteAAAAAAAAA");
        SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel, EnumLibrary.Inputs.OnLeftClick});
    }
}
