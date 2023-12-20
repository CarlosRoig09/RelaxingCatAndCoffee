using personalLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using Newtonsoft.Json.Linq;

public class LevelManager : MonoBehaviour
{
    private PuntuationController _punC;
    private EnergyController _enC;
    [SerializeField]
    private PercentagesAndTime _pAndTime;
    private static LevelManager _instance;
    private BlossomData _specialBlossom;

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
    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _punC = GameObjectLibrary.Instance.PuntuationControllerScript;
        _enC = GameObjectLibrary.Instance.EnergyControllerScript;
        UIManager.Instance.ModifyPunHUD(_punC.Value);
        UIManager.Instance.ModifyEnergyHUD(_enC.Value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetSpecialBlossom(BlossomData blossomData)
    {
        _specialBlossom = blossomData;
    }
    public float SpawnTimeByPun()
    {
        int iteration = 0;
        int comparePuntuation = _punC.Value/50;
        int[] marge = new int[] { 0, 1, 2, 8, 20, 40 };
        bool endFor = false;
        for (int i = 0; i < marge.Length-1&&!endFor;i++)
        {
            if (comparePuntuation >= marge[i] && comparePuntuation < marge[i + 1])
            {
                iteration += i + 1;
                endFor = true;
            }
        }
        var orderedPercentagesAndTime = CurrenOrderByIterations(iteration);
        return orderedPercentagesAndTime.Time[RandomMethods.ReturnARandomObject(orderedPercentagesAndTime.Percentages.ToArray(), 0, orderedPercentagesAndTime.Percentages.Count, 0)]; 
    }

    private PercentagesAndTime CurrenOrderByIterations(int iterations)
    {
        _specialBlossom.SpawnPercentage = 30;
        var clonePAndTime = Instantiate(_pAndTime);
        for (int i = clonePAndTime.Percentages.Count - 1; i > 0; i--)
        {
            for (int y = i -1; y >= 0;)
            {
                if (iterations > 0)
                {
                    (clonePAndTime.Percentages[i], clonePAndTime.Percentages[y]) = (clonePAndTime.Percentages[y], clonePAndTime.Percentages[i]);
                    iterations -= 1;
                    _specialBlossom.SpawnPercentage += 10;
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
