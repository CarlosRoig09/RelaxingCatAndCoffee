using UnityEngine;
using UnityEngine.InputSystem;
using personalLibrary;
using Interfaces;
using UnityEngine.UI;
using TimerSystem;
using NUnit.Framework;
public class InputController : MonoBehaviour, IWaitTheEvent
{
    private static InputController _instance;
    private PlayerInput _playerInput;
    private Vector2 _catInitialPosition;
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
    private InGameButtonsHandle _attackButton;
    [SerializeField]
    private InGameButtonsHandle _moveButtonA;
    [SerializeField]
    private InGameButtonsHandle _moveButtonD;
    [SerializeField]
    private InGameButtonsHandle _pauseButton;
    [SerializeField]
    private InGameButtonsHandle _coffeeButton;

    private float _delayButtonNotTouch = 0.2f;
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
        foreach (EnumLibrary.Inputs input in inputs)
        {
            switch (input)
            {
                case EnumLibrary.Inputs.OnLeftClick:
                    if(!_isOnMobile){
                    _onLeftClick.started += OnLeftClick;
                    _onLeftClick.canceled += OnLeftCanceled;
                    }
                    _attackButton.onClickButtonStart.AddListener(OnEnterClick);
                    _attackButton.onClickButtonCancelled.AddListener(OnEnterCancelled);
                    break;
                case EnumLibrary.Inputs.OnRightClick:
                    if(!_isOnMobile){
                    _onRightClick.performed+= OnRightClick;
                    _onRightClick.canceled += OnRightCanceled;
                    }
                    _coffeeButton.onClickButtonStart.AddListener(OnShiftClick);
                    _coffeeButton.onClickButtonCancelled.AddListener(OnShiftCanceled);
                    break; 
                case EnumLibrary.Inputs.OnScroll:
                    if(!_isOnMobile)
                        _onMovement.started += OnMovement;
                    else
                        _onStartMovement.started += OnTouchStartMovement;
                    _moveButtonA.onClickButtonStart.AddListener(OnMovementLeft);
                    _moveButtonD.onClickButtonStart.AddListener(OnMovementRight);
                    break;
                case EnumLibrary.Inputs.OnScrollCancel:
                    if(!_isOnMobile)
                        _onMovement.canceled += OnMovementCancel;
                    else
                        _onStartMovement.canceled += OnMovementCancel;
                    _moveButtonA.onClickButtonCancelled.AddListener(OnMovementCancel);
                    _moveButtonD.onClickButtonCancelled.AddListener(OnMovementCancel);
                    break;
                case EnumLibrary.Inputs.OnEscClick:
                    if(!_isOnMobile){
                        _onEscClick.started += OnEscClick;
                        _onEscClick.canceled += OnEscCanceled;
                    }
                    _pauseButton.onClickButtonStart.AddListener(OnEscButtonClick);
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
                    if(!_isOnMobile){
                    _onLeftClick.started -= OnLeftClick;
                    _onLeftClick.canceled -= OnLeftCanceled;
                    }
                    _attackButton.onClickButtonStart.RemoveListener(OnEnterClick);
                    _attackButton.onClickButtonCancelled.RemoveListener(OnEnterCancelled);
                    break;
                case EnumLibrary.Inputs.OnRightClick:
                    if(!_isOnMobile){
                    _onRightClick.performed-= OnRightClick;
                    _onRightClick.canceled -= OnRightCanceled;
                    }
                    _coffeeButton.onClickButtonStart.RemoveListener(OnShiftClick);
                    _coffeeButton.onClickButtonCancelled.RemoveListener(OnShiftCanceled);
                    break; 
                 case EnumLibrary.Inputs.OnScroll:
                    if(!_isOnMobile)
                        _onMovement.started -= OnMovement;
                    else
                        _onStartMovement.started -= OnTouchStartMovement;
                    _moveButtonA.onClickButtonStart.RemoveListener(OnMovementLeft);
                    _moveButtonD.onClickButtonStart.RemoveListener(OnMovementRight);
                    break;
                case EnumLibrary.Inputs.OnScrollCancel:
                    if(!_isOnMobile)
                        _onMovement.canceled -= OnMovementCancel;
                    else
                        _onStartMovement.canceled -= OnMovementCancel;
                    _moveButtonA.onClickButtonCancelled.RemoveListener(OnMovementCancel);
                    _moveButtonD.onClickButtonCancelled.RemoveListener(OnMovementCancel);
                    break;
                case EnumLibrary.Inputs.OnEscClick:
                    if(!_isOnMobile){
                        _onEscClick.started -= OnEscClick;
                        _onEscClick.canceled -= OnEscCanceled;
                    }
                    _pauseButton.onClickButtonStart.RemoveListener(OnEscButtonClick);
                    break;
                default:
                    Debug.LogError("Error: This input doesn't exist");
                    break;
            }
        }
    }

    void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>().normalized;
        float directionX = direction.x;
        if (directionX>0)
        {
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.D);
        }
        else { 
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.A);
            }
        _lastKeyValue = directionX;
        _catBehaivour.Movement(_lastKeyValue*Vector2.right);
    }

    void OnMovementLeft()
    {
        float directionX = -1;
         if (directionX>0)
        {
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.D);
        }
        else { 
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.A);
            }
        _lastKeyValue = directionX;
        _catBehaivour.Movement(_lastKeyValue*Vector2.right);
    }

    void OnMovementRight()
    {
         float directionX = 1;
         if (directionX>0)
        {
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.D);
        }
        else { 
            UIManager.Instance.ClickButton(EnumLibrary.ButtonType.A);
            }
        _lastKeyValue = directionX;
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

    void OnMovementCancel()
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

    void OnShiftClick()
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Shift);
        _gestionInventory.UseCofee();
    }

    void OnRightCanceled(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Shift);
    }

    void OnShiftCanceled()
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

    void OnEnterCancelled()
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Enter);
    }

    void OnEscClick(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Esc);
        LevelManager.Instance.PauseGame();
        DesubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnEscClick,EnumLibrary.Inputs.OnLeftClick});
        _onEscClick.started += OnEscClickSecond;
    }

    void OnEscButtonClick()
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Esc);
        LevelManager.Instance.PauseGame();
        DesubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnEscClick, EnumLibrary.Inputs.OnLeftClick });
        _pauseButton.onClickButtonStart.RemoveAllListeners();
        _pauseButton.onClickButtonStart.AddListener(OnEscButtonClickSecond);
        if (!_isOnMobile)
        {
            _onEscClick.started += OnEscClickSecond;
        }
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
        _pauseButton.RemoveAllListeners();
        _pauseButton.AddListener(OnEscButtonClick);
    }

    void OnEscCanceled(InputAction.CallbackContext context)
    {
        UIManager.Instance.ClickButton(EnumLibrary.ButtonType.Esc);
    }

    public void MethodForEvent(object value)
    {
        SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel, EnumLibrary.Inputs.OnLeftClick});
    }
}
