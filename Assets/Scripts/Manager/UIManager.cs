using personalLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UI Manager is NULL");
            }
            return _instance;
        }
    }
    private Text _textPuntuation;
    private Slider _energySlider;
    private GameObject[] _cofees;
    private int _cofeesCount;
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
        _textPuntuation = GameObjectLibrary.Instance.Puntuation.GetComponent<Text>();
        _energySlider = GameObjectLibrary.Instance.Energy.GetComponent<Slider>();
        _cofees = new GameObject[GameObjectLibrary.Instance.CofeePanel.transform.childCount];
        for(int i = 0; i < _cofees.Length; i++)
        {
            _cofees[i] = GameObjectLibrary.Instance.CofeePanel.transform.GetChild(i).gameObject;
            _cofees[i].SetActive(false);
        }
        _cofeesCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModifyPunHUD(int puntuation)
    {
        _textPuntuation.text = puntuation.ToString();
    }

    public void ModifyEnergyHUD(float energy)
    {
        energy /= 100;
        _energySlider.value += energy;
    }

    public void AddCofeeHUD()
    {
        _cofees[_cofeesCount].SetActive(true);
        _cofeesCount += 1;
    }

    public void RemoveCofeeHUD()
    {
        _cofees[_cofeesCount-1].SetActive(false);
        _cofeesCount-= 1;
    }
}
