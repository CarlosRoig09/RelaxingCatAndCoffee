using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using personalLibrary;
using UnityEngine.UI;
using TimerSystem;
using Utilities.Managers.PauseSystem;

public class CatForce : MonoBehaviour, IPausable
{
    private EnumLibrary.CatForceState _state;
    private EnumLibrary.CatForceState _stateBeforePause;
    private SpriteRenderer _spriteRenderer;
    private Color _baseColor;
    private bool _startedTimer;

    private Vector3 _intialScale;

    public float MaxForce { private get; set ;}
    public delegate void IFinishedTheAttack();
    public event IFinishedTheAttack OnFinishedTheAttack;
    public float AmountToScaleX;
    public float AmountToScaleY;
    public float MaxScaleX;
    public float WaitTimeTillDestroy;
    public float WaitBlossomTime;
    public float MinVectorDistance;
    public float MediumVectorDistance;
    public float MaxVectorDistance;
    public float MaxStrenght;
    public float MediumStrenght;
    public float BeforeMinStrenght;
    public float MinStrenght;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer= GetComponent<SpriteRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        OnFinishedTheAttack += GameObjectLibrary.Instance.CatBehaivourScript.WaitTillAtackFinish;
        _intialScale = transform.localScale;
        _baseColor = _spriteRenderer.color;
        PauseManager.Instance.RegisterPausable(this);
    }

    public void StartExpand()
    {
        gameObject.SetActive(true);
        _state = EnumLibrary.CatForceState.Expand;
        _startedTimer = false;
        transform.localScale = _intialScale;
        _spriteRenderer.color = _baseColor;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case EnumLibrary.CatForceState.Expand:
                Expand();
                break;
            case EnumLibrary.CatForceState.Pause:
                break;
            case EnumLibrary.CatForceState.Destroy:
                Destroy();
                break;

        }
    }

    private void Expand()
    {
        if (transform.localScale.x < MaxScaleX)
        {
            transform.localScale = new Vector3(transform.localScale.x + AmountToScaleX, transform.localScale.y + AmountToScaleY);
        }
        
        else 
        {
            if (!_startedTimer)
            {
                _startedTimer = true;
                TimerManager.Instance.CreateTimer(WaitTimeTillDestroy, onTimerComplete: () =>
                {
                    _state = EnumLibrary.CatForceState.Destroy;
                    _startedTimer = false;
                });
            }
        }
    }

    private void Destroy()
    {
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a - 0.1f);
        transform.localScale = new Vector3(transform.localScale.x + 0.1f * Time.deltaTime, transform.localScale.y + 0.06f*Time.deltaTime);
        if (_spriteRenderer.color.a <= 0)
        {
            gameObject.SetActive(false);
            OnFinishedTheAttack();
        }
    }

    //See the distance between the center and the object and give a direction and force depending of the resultant vector.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BlossomBehaivour>(out var blossomBehaivour))
        {
            blossomBehaivour.StopMovement(WaitBlossomTime);
            var vectorDistance = Mathf.Sqrt(Mathf.Pow(blossomBehaivour.transform.position.x - transform.position.x, 2) + Mathf.Pow(blossomBehaivour.transform.position.y - transform.position.y, 2));
            float potencia;
            if (vectorDistance < MinVectorDistance)
                potencia = MaxStrenght;

            else if (vectorDistance > MinVectorDistance && vectorDistance < MediumVectorDistance)
                potencia = MediumStrenght;
            else if (vectorDistance > MediumVectorDistance && vectorDistance < MediumVectorDistance)
                potencia = BeforeMinStrenght;
            else
                potencia = MinStrenght;
            
            var force = (MaxForce / (vectorDistance/potencia)) * new Vector2(blossomBehaivour.transform.position.x - transform.position.x, blossomBehaivour.transform.position.y - transform.position.y);
            blossomBehaivour.Rb2D.AddForce(force);
            blossomBehaivour.ChangeLayersCoroutine();
        }
    }

    public void OnPause()
    {
        _stateBeforePause = _state;
        _state = EnumLibrary.CatForceState.Pause;
    }

    public void OnResume()
    {
        _state = _stateBeforePause;
    }

    private void OnDestroy()
    {
        PauseManager.Instance.UnregisterPausable(this);
    }
}
