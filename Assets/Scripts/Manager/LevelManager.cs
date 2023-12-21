using personalLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using Newtonsoft.Json.Linq;

public class LevelManager : MonoBehaviour, IHaveTheEvent
{
    private PuntuationController _punC;
    private EnergyController _enC;
    [SerializeField]
    private PercentagesAndTime _pAndTime;
    private static LevelManager _instance;
    private BlossomData _specialBlossom;
    private CatAnimationController _catAnimationController;

    public event IHaveTheEvent.IHaveTheEvent IHTEvent;

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

    public EnumLibrary.TypeOfEvent Type { get => EnumLibrary.TypeOfEvent.EmergencyState; set => throw new System.NotImplementedException(); }

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
        _enC.Value = 100;
        UIManager.Instance.ModifyPunHUD(_punC.Value);
        UIManager.Instance.ModifyEnergyHUD(_enC.Value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmergencyState()
    {

    }

    public void NotifyHitToThePlayer(float damage)
    {

    }

    public void GetSpecialBlossom(BlossomData blossomData)
    {
        _specialBlossom = blossomData;
    }
    public float SpawnTimeByPun()
    {
        _specialBlossom.SpawnPercentage = 10;
       int iteration = 0;
        float comparePuntuation = _punC.Value/50;
        int[] marge = new int[] { 0, 1, 2, 4, 16, 24};
        bool endFor = false;
        for (int i = 1; i < marge.Length-1&&!endFor;i++)
        {
            if (comparePuntuation >= marge[i] && comparePuntuation < marge[i + 1])
            {
                iteration += i;
                _specialBlossom.SpawnPercentage += 10;
                endFor = true;
            }
            else if (marge[i] == marge[marge.Length-1])
            {
                iteration+= i;
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
            for (int y = i + 1; y < clonePAndTime.Percentages.Count;y++)
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
        if(!_enC.IsEnergyExahusted())
        {
            ((IModificableValue)_enC).ModifyValue(modEn);
            UIManager.Instance.ModifyEnergyHUD(_enC.Value);
        }
        else
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
}
