using personalLibrary;
using System.Collections;
using TimerSystem;
using UnityEngine;
using Utilities.Managers.PauseSystem;

public class BlossomBehaivour : MonoBehaviour, IPausable
{
    [SerializeField]
    private BlossomData _blossomData;
    public BlossomData BlossomData { get { return _blossomData; } set { _blossomData = value; } }
    private Rigidbody2D _rb2D;
    public Rigidbody2D Rb2D { get { return _rb2D; } }
    private int _sentido;
    private bool _stop;
    [SerializeField]
    private float _elevation;
    [SerializeField]
    private float _timeStoped;
    private float[] _parabolaEquation;

    private const float CHANGE_LAYER_TIME = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _sentido = 1;
        _stop = false;
        _parabolaEquation = CalculteParabolaEcuation(new float[] { 0, BlossomData.MaxSpeedY }, new float[] { BlossomData.MaxSpeedX, 0 });
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stop)
        {
            Movement();
        }
    }

    private void Movement()
    {
        _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x + BlossomData.MaxSpeedX * _sentido * Time.deltaTime, (_parabolaEquation[0] * Mathf.Pow(_rb2D.linearVelocity.x, 2) + _parabolaEquation[1] * _rb2D.linearVelocity.x + _parabolaEquation[2]));
        if (_rb2D.linearVelocity.x >= BlossomData.MaxSpeedX)
        {
            _sentido = -1;
            StopVertically();
        }

        if (_rb2D.linearVelocity.x <= BlossomData.MaxSpeedX * -1)
        {
            _sentido = 1;
            StopVertically();
        }
    }

    private float[] CalculteParabolaEcuation(float[] pointA, float[] pointC)
    {
        float a;
        float b;
        float c;
        c = pointA[1];
        b = 0;
        a = c / Mathf.Pow(pointC[0], 2);
        return new float[] { a, b, c };
    }

    private void StopVertically()
    {
        _stop = true;
        //Hardcoded for now, but the timestoped and the speed reduction should be variables on the scriptable object
        Vector3 velocityDuringTimePause = new Vector2(_rb2D.linearVelocity.x * 0.5f, _rb2D.linearVelocity.y * -1 * 0.25f) ;
        Vector3 initialVelocity = _rb2D.linearVelocity;
        float lerpSumXTime = Time.deltaTime / _timeStoped;
        float lerpValue = 0;
        TimerManager.Instance.CreateTimer(_timeStoped, onTimerUpdate:(float time)=> 
        {
            lerpValue += lerpSumXTime;
          _rb2D.linearVelocity = Vector2.Lerp(initialVelocity,velocityDuringTimePause, lerpValue);
        },
        onTimerComplete: () =>
        {
            _stop = false;
        });
    }

    public void StopMovement(float time)
    {
        float speedX = _rb2D.linearVelocity.x;
        float speedY = _rb2D.linearVelocity.y;
        _stop = true;
        _rb2D.linearVelocity = new Vector2(0, 0);
        TimerManager.Instance.CreateTimer(time, onTimerComplete: () => 
        {
           _rb2D.linearVelocity = new Vector2(speedX, speedY);
           _stop = false;
        });
    }

    //Why the layer needs to be changed
    public void ChangeLayersCoroutine()
    {
        gameObject.layer = 8;
        TimerManager.Instance.CreateTimer(CHANGE_LAYER_TIME, onTimerComplete: () =>
        {
            gameObject.layer = 6;
        });
    }
    public int GivePuntuation(EnumLibrary.PunType punType)
    {
        return punType switch
        {
            EnumLibrary.PunType.Positive => BlossomData.PositivePuntuation,
            EnumLibrary.PunType.Negative => BlossomData.NegativePuntuation,
            _ => 0,
        };
    }

    public int LoseEnergy()
    {
        return BlossomData.EnergyLose;
    }

    //Why destroy needs an object. Change it to object pull
    public object Destroy(object nothing)
    {
        Destroy(gameObject, 0.1f);
        return null;
    }

    public void OnPause()
    {
        throw new System.NotImplementedException();
    }

    public void OnResume()
    {
        throw new System.NotImplementedException();
    }
}
