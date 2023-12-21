using UnityEngine;
using UnityEngine.SceneManagement;
using personalLibrary;
using Interfaces;
using System.Linq;
using UnityEngine.UI;

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
    public event ChangeScene OnChangeScene;
    private bool _menuActions;
    private bool _gameOverActions;
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
            }
        }
        else if (_scene == EnumLibrary.Scene.GameMenu)
        {
            if (!_menuActions)
            {
                _menuActions = true;
                GameObject.Find("Game").GetComponent<Button>().onClick.AddListener(UIManager.Instance.GameButton);
                GameObject.Find("Exit").GetComponent<Button>().onClick.AddListener(UIManager.Instance.ExitGame);
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

    public void GameOver()
    {
        Debug.Log("Game Over");
        LoadScene(EnumLibrary.Scene.GameOverScreen);
    }

    public void LoadScene(EnumLibrary.Scene escena)
    {
        switch (escena)
        {
            case EnumLibrary.Scene.GameScreen:
                _calledStartGame = false;
                SceneManager.LoadScene("GameScene");
                break;
            case EnumLibrary.Scene.GameMenu:
                _menuActions = false;
                SceneManager.LoadScene("Menú");
                break;
            case EnumLibrary.Scene.GameOverScreen:
                _gameOverActions= false;
                SceneManager.LoadScene("GameOver");
                break;
            default:
                break;
       }

    }
}