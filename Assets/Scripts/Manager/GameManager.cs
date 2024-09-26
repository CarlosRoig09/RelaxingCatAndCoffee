using UnityEngine;
using UnityEngine.SceneManagement;
using personalLibrary;
using Interfaces;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum GameFinish
{
    Win,
    Lose
}
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is NULL");
            }
            return _instance;
        }
    }
    public delegate void StartGame();
    public event StartGame OnStartGame;
    private EnumLibrary.Scene _scene;
    private bool _calledStartGame;
    public delegate void ChangeScene(string scene);
    private bool _menuActions;
    private bool _gameOverActions;
    private int _puntuation;
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
        _calledStartGame = false;
        _menuActions = false;
        _gameOverActions = false;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBetweenScene();
        if (_scene == EnumLibrary.Scene.GameScreen)
        {
           if (!_calledStartGame)
           {
                GameObject.Find("Continue").GetComponent<Button>().onClick.AddListener(UIManager.Instance.ResumeButton);
                GameObject.Find("ReturnToMenu").GetComponent<Button>().onClick.AddListener(UIManager.Instance.MenuButton);
                _calledStartGame = true;
                OnStartGame();
           }
        }
        else if (_scene== EnumLibrary.Scene.GameOverScreen)
        {
            if (!_gameOverActions)
            {
                _gameOverActions = true;
                GameObject.Find("Retry").GetComponent<Button>().onClick.AddListener(UIManager.Instance.GameButton);
                GameObject.Find("Menu").GetComponent<Button>().onClick.AddListener(UIManager.Instance.MenuButton);
                UIManager.Instance.ShowPuntuationGameOver(_puntuation);
            }
        }
        else if (_scene == EnumLibrary.Scene.GameMenu)
        {
            if (!_menuActions)
            {
                _menuActions = true;
                GameObject.Find("Escape").GetComponent<Button>().onClick.AddListener(UIManager.Instance.HideSettingsPopUp);
                UIManager.Instance.HideSettingsPopUp();
                GameObject.Find("Game").GetComponent<Button>().onClick.AddListener(UIManager.Instance.GameButton);
                GameObject.Find("Exit").GetComponent<Button>().onClick.AddListener(UIManager.Instance.ExitGame);
                GameObject.Find("Settings").GetComponent<Button>().onClick.AddListener(UIManager.Instance.ShowSettingsPopUp);
            }
        }
    }
    void ChangeBetweenScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "GameScene":
            case "demo":
                _scene = EnumLibrary.Scene.GameScreen;
                break;
            case "Menú":
                _scene = EnumLibrary.Scene.GameMenu;
                break;
            case "GameOver":
                _scene= EnumLibrary.Scene.GameOverScreen;
                break;
            default:
                break;
        }
    }
    public void SubscribeEvent(IWaitTheEvent waitTheEvent)
    {
        IHaveTheEvent[] eventors = FindObjectsOfType<MonoBehaviour>(true).OfType<IHaveTheEvent>().ToArray();
        foreach (IHaveTheEvent eventor in eventors)
        {
                if (eventor.Type == waitTheEvent.Type)
                {
                    eventor.IHTEvent +=waitTheEvent.MethodForEvent ;
                }
        }
    }

    public void DeSubscribeEvent(IWaitTheEvent waitTheEvent)
    {
        IHaveTheEvent[] eventors = FindObjectsOfType<MonoBehaviour>(true).OfType<IHaveTheEvent>().ToArray();
        foreach (IHaveTheEvent eventor in eventors)
        {
            if (eventor.Type == waitTheEvent.Type)
            {
                eventor.IHTEvent -= waitTheEvent.MethodForEvent;
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        LoadScene(EnumLibrary.Scene.GameOverScreen);
    }

    public void ExitGameScene()
    {
        _puntuation = GameObjectLibrary.Instance.PuntuationControllerScript.Value;
    }

    public void LoadScene(EnumLibrary.Scene escena)
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            ExitGameScene();
        }
        switch (escena)
        {
            case EnumLibrary.Scene.GameScreen:
                _calledStartGame = false;
                _puntuation = 0;
                AudioManager.instance.Stop("MenuTheme");
                AudioManager.instance.Play("Theme");
                SceneManager.LoadScene("GameScene");
                break;
            case EnumLibrary.Scene.GameMenu:
                AudioManager.instance.Stop("Theme");
                AudioManager.instance.Play("MenuTheme");
                _menuActions = false;
                SceneManager.LoadScene("Menú");
                break;
            case EnumLibrary.Scene.GameOverScreen:
                AudioManager.instance.Stop("Theme");
                _gameOverActions = false;
                SceneManager.LoadScene("GameOver");
                break;
            default:
                break;
       }

    }
}