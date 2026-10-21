using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using personalLibrary;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class InputController : MonoBehaviour, IWaitTheEvent
{
    private static InputController _instance;
    private PlayerInput _playerInput;
    private Vector3 _catInitialPosition;
    [SerializeField]
    private InputActionAsset _actionAsset;
    [SerializeField]
    private InputActionAsset _mobileActionAsset;
    private InputAction _onMovement;
    private InputAction _onLeftClick;
    private InputAction _onRightClick;
    private InputAction _onEscClick;


    private InputAction _onStartMovement;
    [SerializeField]
    private Button _attackButton;
    [SerializeField]
    private Button _pauseButton;
    [SerializeField]
    private Button _coffeeButton;

    private Vector2 _touchPosition;
    private float _yTouchScreenLimit = 3.07f;
    private float _xTouchScreenLimit = 7.54f;
    private CatBehaivour _catBehaivour;
    private GestionInventory _gestionInventory;
    private float _lastKeyValue;

    private bool _attacking = false;

    private bool _isOnMobile;

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
        _isOnMobile = BBApplication.isTouchInterface;
        try
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.actions = _actionAsset;
            if(!_isOnMobile){
            _onMovement = _playerInput.actions["Movement"];
            _onLeftClick = _playerInput.actions["Attack"];
            _onRightClick = _playerInput.actions["Drink"];
            }
            else
            {
              _onStartMovement  = _playerInput.actions["TouchStartMove"];
              _onMovement = _playerInput.actions["TouchMove"];
               
            }
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
        if (_isOnMobile)
        {
            _catInitialPosition = _catBehaivour.transform.position;
        }
        SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel, EnumLibrary.Inputs.OnEscClick });
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubscribeEvents(EnumLibrary.Inputs[] inputs)
    {
        if(!_isOnMobile){
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
                    _onMovement.started += OnMovement;
                    break;
                case EnumLibrary.Inputs.OnScrollCancel:
                    _onMovement.canceled += OnMovementCancel;
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
        else
        {
            foreach (EnumLibrary.Inputs input in inputs)
        {
            switch (input)
            {
                case EnumLibrary.Inputs.OnLeftClick:
                        _attackButton.onClick.AddListener(OnEnterClick);
                        break;
                case EnumLibrary.Inputs.OnRightClick:
                       _coffeeButton.onClick.AddListener(OnShiftClick);
                       break; 
                case EnumLibrary.Inputs.OnScroll:
                    _onStartMovement.started += OnTouchStartMovement;
                    break;
                case EnumLibrary.Inputs.OnScrollCancel:
                    _onStartMovement.canceled += OnMovementCancel;
                    break;
                case EnumLibrary.Inputs.OnEscClick:
                     _pauseButton.onClick.AddListener(OnEscButtonClick);
                      break;
                default:
                    Debug.LogError("Error: This input doesn't exist");
                    break;
            }
        }
        }
    }

    public void DesubscribeEvents(EnumLibrary.Inputs[] inputs)
    {
        if(!_isOnMobile){
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
                    _onMovement.started -= OnMovement;
                    break;
                case EnumLibrary.Inputs.OnScrollCancel:
                    _onMovement.canceled -= OnMovementCancel;
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
        else
        {
            foreach (EnumLibrary.Inputs input in inputs)
        {
            switch (input)
            {
                case EnumLibrary.Inputs.OnLeftClick:
                    _attackButton.onClick.RemoveAllListeners();
                    break;
                case EnumLibrary.Inputs.OnRightClick:
                    _coffeeButton.onClick.RemoveAllListeners();
                    break;
                case EnumLibrary.Inputs.OnScroll:
                    _onStartMovement.started -= OnTouchStartMovement;
                    break;
                case EnumLibrary.Inputs.OnScrollCancel:
                    _onStartMovement.canceled -= OnMovementCancel;
                    break;
                case EnumLibrary.Inputs.OnEscClick:
                        _pauseButton.onClick.RemoveAllListeners();
                    break;
                default:
                    Debug.LogError("Error: This input doesn't exist");
                    break;
            }
        }
        }
    }

    void OnMovement(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().x>0)
        {
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.D);
        }
        else { 
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.A);
            }
        Vector3 screenToWorlPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        _lastKeyValue = screenToWorlPosition.x>_catInitialPosition.x ? 1:-1;
        
        _catBehaivour.Movement(_lastKeyValue*Vector2.right);
    }

    void OnTouchStartMovement(InputAction.CallbackContext context)
    {
        if (!_attacking)
        {
            Vector2 touchScreenPosition = _onMovement.ReadValue<Vector2>();
            _touchPosition = Camera.main.ScreenToWorldPoint(touchScreenPosition);
            if (Mathf.Abs(_touchPosition.x) < _xTouchScreenLimit && Mathf.Abs(_touchPosition.y) < _yTouchScreenLimit)
            {
                if (_touchPosition.x > 0)
                {
                    UIManager.Instance.ClickButton(EnumLibrary.ButtonType.D);
                }
                else
                {
                    UIManager.Instance.ClickButton(EnumLibrary.ButtonType.A);
                }
                _lastKeyValue = _touchPosition.x > _catInitialPosition.x ? 1 : -1;
                _catBehaivour.Movement(_lastKeyValue * Vector2.right);
            }
        }
    }
    void OnMovementCancel(InputAction.CallbackContext context)
    {
        Debug.Log("Canceled");
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

    void OnShiftClick()
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
    void OnEnterClick()
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

    void OnEscButtonClick()
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Esc);
        LevelManager.Instance.PauseGame();
        //_onEscClick.canceled += OnEscCanceled;
        DesubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnEscClick, EnumLibrary.Inputs.OnLeftClick });
        _pauseButton.onClick.RemoveAllListeners();
        _pauseButton.onClick.AddListener(OnEscButtonClickSecond);
    }

    void OnEscClickSecond(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Esc);
        _onLeftClick.canceled += OnLeftCanceled;
        LevelManager.Instance.ResumeGame();
        _onEscClick.started-= OnEscClickSecond;
        _onEscClick.canceled -= OnEscCanceled;
    }
    void OnEscButtonClickSecond()
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Esc);
        LevelManager.Instance.ResumeGame();
        _pauseButton.onClick.RemoveAllListeners();
        _pauseButton.onClick.AddListener(OnEscButtonClick);
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
