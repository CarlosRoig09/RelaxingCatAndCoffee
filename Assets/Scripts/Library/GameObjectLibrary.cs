using UnityEngine;
using UnityEngine.UI;
namespace personalLibrary
{
    public class GameObjectLibrary : MonoBehaviour
    {
        private static GameObjectLibrary _instance;

        public static GameObjectLibrary Instance
        {
            get
            {
                if(_instance==null)
                {
                    Debug.Log("GameObjectLibrary is NULL");
                }
                return _instance;
            }
        }
        public  GameObject Cat { get; private set; }
        public  CatBehaivour CatBehaivourScript {get; private set;}
        public CatAnimationController CatAnimationControllerScript { get; private set;}
        public GameObject Energy { get; private set; }
        public Slider EnergySlider { get; private set; }
        public EnergyController EnergyControllerScript { get; private set; }
        public TMPro.TMP_Text PuntuationText { get; private set; }
        public GameObject Puntuation { get; private set; }
        public PuntuationController PuntuationControllerScript { get; private set; }

        public GameObject CofeePanel { get; private set; }
        public GestionInventory GestionInventory { get; private set; }

        public GameObject AButton { get; private set; }
        public GameObject DButton { get; private set; }
        public GameObject[] EnterButton { get; private set; }
        public GameObject[] ShiftButton { get; private set; }
        public InputController InputManager { get; private set; }
        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
            {
                //DontDestroyOnLoad(gameObject);
                _instance = this;
            }
            //Not in awake, this library have to wait to GameManager Orders.
            Cat = GameObject.FindGameObjectWithTag("Cat");
            CatBehaivourScript = Cat.GetComponent<CatBehaivour>();
            CatAnimationControllerScript = Cat.GetComponent<CatAnimationController>();
            Energy = GameObject.FindGameObjectWithTag("Energy");
            EnergySlider = Energy.GetComponent<Slider>();
            EnergyControllerScript = Energy.GetComponent<EnergyController>();
            Puntuation = GameObject.FindGameObjectWithTag("Puntuation");
            PuntuationText = Puntuation.GetComponent<TMPro.TMP_Text>();
            PuntuationControllerScript = Puntuation.GetComponent<PuntuationController>();
            CofeePanel = GameObject.FindGameObjectWithTag("Cofee");
            GestionInventory = CofeePanel.GetComponent<GestionInventory>();
            AButton = GameObject.FindGameObjectWithTag("Abutton");
            DButton = GameObject.FindGameObjectWithTag("Dbutton");
            EnterButton = GameObject.FindGameObjectsWithTag("Enterbutton");
            ShiftButton = GameObject.FindGameObjectsWithTag("Shiftbutton");
            InputManager= GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputController>();
        }
    }
}
