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
    private bool _firstTime;
    private GameObject _pauseMenu;
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
        ClickButton(EnumLibrary.ButtonType.Shift);
        _firstTime= true;
        _pauseMenu = GameObject.Find("PausePanel");
        _pauseMenu.SetActive(false);
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
        if (_firstTime)
        {
            ClickButton(EnumLibrary.ButtonType.Shift);
            _firstTime = false;
        }
        _cofees[_cofeesCount].GetComponent<SpritesMethods>().ChangeSpriteToTheNextOne();
        _cofees[_cofeesCount].GetComponent<Animator>().SetBool("Smoke", true);
        _cofeesCount += 1;
    }

    public void RemoveCofeeHUD()
    {
        _cofees[_cofeesCount-1].GetComponent<SpritesMethods>().ChangeSpriteToTheNextOne();
        _cofees[_cofeesCount-1].GetComponent<Animator>().SetBool("Smoke", false);
        _cofeesCount -= 1;
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

    public void ClickButton(EnumLibrary.ButtonType buttonType)
    {
        GameObject[] buttonObject = new GameObject[0];
        switch(buttonType)
        {
            case EnumLibrary.ButtonType.A:
                buttonObject = new GameObject[] { GameObjectLibrary.Instance.AButton };
                break;
            case EnumLibrary.ButtonType.D:
                buttonObject = new GameObject[] { GameObjectLibrary.Instance.DButton };
                break;
            case EnumLibrary.ButtonType.Enter:
                buttonObject = GameObjectLibrary.Instance.EnterButton;
                break;
            case EnumLibrary.ButtonType.Shift:
                buttonObject = GameObjectLibrary.Instance.ShiftButton;
                break;
            case EnumLibrary.ButtonType.Esc:
                buttonObject = GameObjectLibrary.Instance.EscButton;
                break;
        }
        foreach(var button in buttonObject)
        {
            button.GetComponent<SpritesMethods>().ChangeSpriteToTheNextOne();
        }
    }

    public void MenuButton()
    {
        GameManager.Instance.LoadScene(EnumLibrary.Scene.GameMenu);
    }

    public void GameButton()
    {
        GameManager.Instance.LoadScene(EnumLibrary.Scene.GameScreen);
    }

    public void PauseButton()
    {
        LevelManager.Instance.PauseGame();
    }

    public void ResumeButton()
    {
        LevelManager.Instance.ResumeGame();
    }

    public void PauseMenu()
    {
        _pauseMenu.SetActive(true);
    }
    public void ClosePause()
    {
        _pauseMenu.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
