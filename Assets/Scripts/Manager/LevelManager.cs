using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using personalLibrary;
using TimerSystem;
using UnityEngine;
using Utilities;
using Utilities.Managers.PauseSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class LevelManager : MonoBehaviour, IWaitTheEvent
{
    private PuntuationController _punC;
    private EnergyController _enC;
    [SerializeField]
    private int _maxEnergyValue;
    private static LevelManager _instance;
    private CatAnimationController _catAnimationController;
    private SpawnerBehaivour _spawnerBehaivour;
    [SerializeField]
    private int _punDifDividing;
    [SerializeField]
    private int[] _difFaseQuotient;
 
    [SerializeField]
    private GameObject _coffeeBean;

    private bool _isPaused = false;

    private Timer _stopMomentumTimer = null;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Level Manager is NULL");
            }
            return _instance;
        }
    }

    public EnumLibrary.TypeOfEvent Type { get => EnumLibrary.TypeOfEvent.AddACofee; set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
        {
            //DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _punC = GameObjectLibrary.Instance.PuntuationControllerScript;
        _enC = GameObjectLibrary.Instance.EnergyControllerScript;
        _catAnimationController = GameObjectLibrary.Instance.CatAnimationControllerScript;
        _enC.Value = _maxEnergyValue;
        UIManager.Instance.ModifyPunHUD(_punC.Value);
        UIManager.Instance.ModifyEnergyHUD(_enC.Value);
        GameManager.Instance.SubscribeEvent(this);
        _spawnerBehaivour = GameObject.Find("Spawner").GetComponent<SpawnerBehaivour>();
    }

    void OnEnable()
    {
         if (_isPaused)
        {
            ResumeGame(true);
        }
    }

    void OnDisable()
    {
        if (_isPaused)
        {
            ResumeGame(true);
        }
    }

    public void PauseGame(bool activeUI = true, bool pauseTimer = true)
    {
        if (pauseTimer && _stopMomentumTimer != null)
            _stopMomentumTimer.PauseTimer();
        _isPaused = true;
        GameObjectLibrary.Instance.InputManager.DesubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnEscClick, EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel, EnumLibrary.Inputs.OnEscClick });
        if(activeUI)
            UIManager.Instance.PauseMenu();
        PauseManager.Instance.PauseGame();
    }

    public void ResumeGame(bool desactivateUI = true, bool pauseTimer = true)
    {
        if (pauseTimer && _stopMomentumTimer != null)
            _stopMomentumTimer.ResumeTimer();
        _isPaused = false;
        GameObjectLibrary.Instance.InputManager.SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnEscClick });
        if(desactivateUI)
            UIManager.Instance.ClosePause();
        PauseManager.Instance.ResumeGame();
    }

    public void DelayGame(float delay)
    {
        Time.timeScale = delay;
    }



    public void OnEndScreenReach(float time, BlossomBehaivour blossomBehaivour, int puntuation, int energy)
    {
        GameObjectLibrary.Instance.InputManager.DesubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnEscClick, EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel, EnumLibrary.Inputs.OnEscClick });
        StartCoroutine(StopMomentum(time, puntuation, energy, blossomBehaivour));
    }

    //Provisional Impact Frame
    private IEnumerator StopMomentum(float time, int puntuation, int energy, BlossomBehaivour blossomBehaivour)
    {
        PauseGame();
        yield return new WaitForSeconds(time);
        ModifyEnergy(energy);
        ModifyPuntuation(puntuation);
        ObjectPoolManager.Instance.DesactivatePooledObject(blossomBehaivour, blossomBehaivour.gameObject);
        ResumeGame();
        GameObjectLibrary.Instance.InputManager.SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnEscClick, EnumLibrary.Inputs.OnScroll, EnumLibrary.Inputs.OnScrollCancel, EnumLibrary.Inputs.OnEscClick });
    }


    private void NotifyHitToThePlayer(float mod)
    {
        if (mod < 0)
        {
            AudioManager.instance.Play("GetHit");
            _catAnimationController.ChangeLayer(1, 1);
            TimerManager.Instance.CreateTimer(0.3f, onTimerComplete:()=> ChangePlayerStates(1, 0));
        }
        else
        {
            _catAnimationController.ChangeLayer(2, 1);
            TimerManager.Instance.CreateTimer(0.3f, onTimerComplete: () => ChangePlayerStates(2, 0));
        }
    }

    private void ChangePlayerStates(int layer, float weight)
    {
        _catAnimationController.ChangeLayer(layer, weight);
    }
    public float SpawnTimeByPun()
    {
        return 0;
    }

    public float SpeedIncreaseByPun()
    {
        return 0;
    }

    public void ModifyEnergy(int modEn)
    {
        NotifyHitToThePlayer(modEn);
        _enC.ModifyValue(modEn);
        UIManager.Instance.ModifyEnergyHUD(_enC.Value);
        if (_enC.IsEnergyExahusted())
        {
            CallGameOver();
        }
    }

    public void ModifyPuntuation(int modPun)
    {
        _punC.ModifyValue(modPun);
        _punC.IsPuntuationBelow0();
        UIManager.Instance.ModifyPunHUD(_punC.Value);
    }

    public void CallGameOver()
    {
        GameManager.Instance.GameOver();
    }

    public void MethodForEvent(object value)
    {
        GameObjectLibrary.Instance.InputManager.SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnRightClick });
        GameManager.Instance.DeSubscribeEvent(this);
    }
}