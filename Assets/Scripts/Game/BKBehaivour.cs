using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using personalLibrary;
using Interfaces;

public class BKBehaivour : MonoBehaviour, IWaitTheEvent, IHaveTheEvent
{
    [SerializeField]
    private int _id;
    private int cofeeCount;
    private bool countCofee;
    [SerializeField]
    private int _obtainCofee;
    [SerializeField]
    private CofeeData _cofeeData;
    [SerializeField]
    private Sprite[] _sprites;
    private int _spritecount;
    private SpriteRenderer _spriteRenderer;
    public event IHaveTheEvent.IHaveTheEvent IHTEvent;

    public EnumLibrary.TypeOfEvent Type => EnumLibrary.TypeOfEvent.StopCofeeProduction;

    EnumLibrary.TypeOfEvent IHaveTheEvent.Type { get => EnumLibrary.TypeOfEvent.AddACofee; set => throw new System.NotImplementedException(); }

    // Start is called before the first frame update
    void Start()
    {
        cofeeCount= 0;
        countCofee= true;
        GameManager.Instance.SubscribeEvent(this);
        _spritecount = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out BlossomBehaivour blossomBehaivour))
        {
            blossomBehaivour.gameObject.GetComponent<Collider2D>().enabled= false;
            var puntuation = blossomBehaivour.GivePuntuation(EnumLibrary.PunType.Positive);
            SendPuntuation(puntuation);
            _spritecount += puntuation/10;
            if (_spritecount < _sprites.Length)
                _spriteRenderer.sprite = _sprites[_spritecount];
            if (puntuation < 30)
                AudioManager.instance.Play("obtainedBlossom");
            else
                AudioManager.instance.Play("especialObtained");

            blossomBehaivour.Destroy(null);
            CountCofee(puntuation);
            if(cofeeCount==_obtainCofee)
            {
                SendCofee();
            }
        }
    }

    private void SendPuntuation(int pun)
    {
        LevelManager.Instance.ModifyPuntuation(pun);
    }

    private void CountCofee(int puntuation)
    {
        if(countCofee)
        {
            cofeeCount += puntuation/10;
            if(cofeeCount>_obtainCofee)
                cofeeCount=_obtainCofee;
            UIManager.Instance.CountCofee(_id, cofeeCount, _obtainCofee);
        }
    }

    private void SendCofee()
    {
        AudioManager.instance.Play("CofeeObtained");
        IHTEvent(Instantiate(_cofeeData));
        cofeeCount = 0;
    }

    private void StopCofeeCount(bool limit)
    {
        countCofee = !limit;
        cofeeCount= 0;
        UIManager.Instance.CountCofee(_id, cofeeCount, _obtainCofee);
    }

    public void MethodForEvent(object value)
    {
        StopCofeeCount((bool)value);
    }
}
