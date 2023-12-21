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
    private GameObject[] _cofees;
    private int _cofeesCount;
    [SerializeField]
    private Sprite[] _buttonA;
    [SerializeField] 
    private Sprite[] _buttonD;
    [SerializeField]
    private Sprite[] _buttonEnter;
    [SerializeField]
    private Sprite[] _buttonShift;
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
        GameManager.Instance.OnStartGame += OnStartGame;
    }

    void OnStartGame()
    {
        _cofees = new GameObject[GameObjectLibrary.Instance.CofeePanel.transform.childCount];
        for (int i = 0; i < _cofees.Length; i++)
        {
            _cofees[i] = GameObjectLibrary.Instance.CofeePanel.transform.GetChild(i).gameObject;
        }
        _cofeesCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModifyPunHUD(int puntuation)
    {
        GameObjectLibrary.Instance.PuntuationText.text = puntuation.ToString();
    }

    public void ModifyEnergyHUD(float energy)
    {
        energy /= 100;
        GameObjectLibrary.Instance.EnergySlider.value = energy;
    }

    public void AddCofeeHUD()
    {
        _cofees[_cofeesCount].GetComponent<SpritesMethods>().ChangeSpriteToTheNextOne();
        _cofeesCount += 1;
    }

    public void RemoveCofeeHUD()
    {
        _cofees[_cofeesCount-1].GetComponent<SpritesMethods>().ChangeSpriteToTheNextOne();
        _cofeesCount-= 1;
    }

    public void CountCofee(int id,int count, int maxCount)
    {
        int limit;
        switch (id)
        {
            case 0:
                limit = _cofees.Length-2;
                break;
            case 1:
                limit = _cofees.Length-1;
                break;
            default:
                limit = _cofees.Length-2;
                break;
        }
        _cofees[limit].GetComponent<TMP_Text>().text = count + "/" + maxCount;
    }

    public void ClickButton()
    {

    }

    public void MenuButton()
    {
        GameManager.Instance.LoadScene(EnumLibrary.Scene.GameMenu);
    }

    public void GameButton()
    {
        GameManager.Instance.LoadScene(EnumLibrary.Scene.GameScreen);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
