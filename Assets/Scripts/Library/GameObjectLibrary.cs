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

        public GameObject Energy { get; private set; }
        public Slider EnergySlider { get; private set; }
        public EnergyController EnergyControllerScript { get; private set; }
        public GameObject Puntuation { get; private set; }
        public Text PuntuationText { get; private set; }
        public PuntuationController PuntuationControllerScript { get; private set; }

        public GameObject CofeePanel { get; private set; }
        public GestionInventory GestionInventory { get; private set; }
        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
            {
                DontDestroyOnLoad(gameObject);
                _instance= this;
            }
            //Not in awake, this library have to wait to GameManager Orders.
            Cat = GameObject.FindGameObjectWithTag("Cat");
            CatBehaivourScript = Cat.GetComponent<CatBehaivour>();
            Energy = GameObject.FindGameObjectWithTag("Energy");
            EnergySlider = Energy.GetComponent<Slider>();
            EnergyControllerScript = Energy.GetComponent<EnergyController>();
            Puntuation = GameObject.FindGameObjectWithTag("Puntuation");
            PuntuationText = Puntuation.GetComponent<Text>();
            PuntuationControllerScript = Puntuation.GetComponent<PuntuationController>();
            CofeePanel = GameObject.FindGameObjectWithTag("Cofee");
            GestionInventory = CofeePanel.GetComponent<GestionInventory>();
        }
    }
}
