using personalLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using Newtonsoft.Json.Linq;

public class LevelManager : MonoBehaviour, IWaitTheEvent
{
    private PuntuationController _punC;
    private EnergyController _enC;
    [SerializeField]
    private PercentagesAndTime _pAndTime;
    private static LevelManager _instance;
    private BlossomData _specialBlossom;
    private CatAnimationController _catAnimationController;
    private SpawnerBehaivour _spawnerBehaivour;
 
    [SerializeField]
    private GameObject _coffeeBean;
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
        _enC.Value = 100;
        UIManager.Instance.ModifyPunHUD(_punC.Value);
        UIManager.Instance.ModifyEnergyHUD(_enC.Value);
        GameManager.Instance.SubscribeEvent(this);
        _spawnerBehaivour = GameObject.Find("Spawner").GetComponent<SpawnerBehaivour>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseGame()
    {
        UIManager.Instance.PauseMenu();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        GameObjectLibrary.Instance.InputManager.SubscribeEvents(new EnumLibrary.Inputs[] { EnumLibrary.Inputs.OnLeftClick, EnumLibrary.Inputs.OnEscClick });
        UIManager.Instance.ClosePause();
        Time.timeScale = 1;
    }

    public void DelayGame(float delay)
    {
        Time.timeScale = delay;
    }

    public void StopGameByTime(float time)
    {
        StartCoroutine(StopTillTime(time));
    }

    public void CofeeObatinedAnimation(Vector3 initialPosition)
    {
       var bean = Instantiate(_coffeeBean, initialPosition, Quaternion.identity);
        var beanBehaivour = bean.GetComponent<CofeeBeanBehaivour>();
        beanBehaivour.SpawnPosition = initialPosition;
        beanBehaivour.SpawnDirection = UIManager.Instance.Cofees[UIManager.Instance.CofesCount].transform.position;
        beanBehaivour.CofeeMovement();
        StartCoroutine(StopTillTime(1f));
    }



    private IEnumerator StopTillTime(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSeconds(time);
        Time.timeScale = 1;
    }


    public void EmergencyState()
    {

    }

    public void NotifyHitToThePlayer(float mod)
    {
        if (mod < 0)
        {
            AudioManager.instance.Play("GetHit");
            _catAnimationController.ChangeLayer(1, 1);
            StartCoroutine(ChangePlayerStates(0.3f, 1, 0));
        }
        else
        {
            _catAnimationController.ChangeLayer(2, 1);
            StartCoroutine(ChangePlayerStates(0.3f, 2, 0));
        }
    }

    private IEnumerator ChangePlayerStates(float time, int layer, float weight)
    {
        yield return new WaitForSeconds(time);
        _catAnimationController.ChangeLayer(layer, weight);
    }

    public void GetSpecialBlossom(BlossomData blossomData)
    {
        _specialBlossom = blossomData;
    }
    public float SpawnTimeByPun()
    {
        _specialBlossom.SpawnPercentage = 20;

        int iteration = 0;
        float comparePuntuation = _punC.Value / 50;
        int[] marge = new int[] { 0, 1, 2, 4, 16, 24 };

        bool endFor = false;
        _spawnerBehaivour.RecoverBaseSpeed();
        for (int i = 1; i < marge.Length && !endFor; i++)
        {
            if (marge[i] == marge[marge.Length - 1] && comparePuntuation >= marge[i])
            {
                iteration += i;
                _specialBlossom.SpawnPercentage += 50;
                for(int y = 0; y < i; y++)
                {
                    _spawnerBehaivour.SpeedUp();
                }
            }
            else if (comparePuntuation >= marge[i] && comparePuntuation < marge[i + 1])
            {
                iteration += i;
                _specialBlossom.SpawnPercentage += 10;
                endFor = true;
                for (int y = 0; y < i; y++)
                {
                    _spawnerBehaivour.SpeedUp();
                }
            }

        }
        var orderedPercentagesAndTime = CurrenOrderByIterations(iteration);
        return orderedPercentagesAndTime.Time[RandomMethods.ReturnARandomObject(orderedPercentagesAndTime.Percentages.ToArray(), 0, orderedPercentagesAndTime.Percentages.Count, 0)];
    }

    private PercentagesAndTime CurrenOrderByIterations(int iterations)
    {
        var clonePAndTime = Instantiate(_pAndTime);
        for (int i = clonePAndTime.Percentages.Count - (iterations + 1); i < clonePAndTime.Percentages.Count; i++)
        {
            for (int y = i + 1; y < clonePAndTime.Percentages.Count; y++)
            {
                if (clonePAndTime.Percentages[i] < clonePAndTime.Percentages[y])
                {
                    (clonePAndTime.Percentages[i], clonePAndTime.Percentages[y]) = (clonePAndTime.Percentages[y], clonePAndTime.Percentages[i]);
                }
                else
                    return clonePAndTime;
            }
        }
        return clonePAndTime;
    }

    public void ModifyEnergy(int modEn)
    {
        NotifyHitToThePlayer(modEn);
        ((IModificableValue)_enC).ModifyValue(modEn);
        UIManager.Instance.ModifyEnergyHUD(_enC.Value);
        if (_enC.IsEnergyExahusted())
        {
            CallGameOver();
        }
    }

    public void ModifyPuntuation(int modPun)
    {
        ((IModificableValue)_punC).ModifyValue(modPun);
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