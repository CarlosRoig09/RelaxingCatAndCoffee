using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using personalLibrary;

public class BlossomBehaivour : MonoBehaviour
{
    [SerializeField]
    private BlossomData _blossomData;
    public BlossomData BlossomData { get { return _blossomData; } set { _blossomData = value; } }
    private Rigidbody2D _rb2D;
    public Rigidbody2D Rb2D { get { return _rb2D; } }
    private bool _sentido;
    private bool _stop;
    [SerializeField]
    private float _elevation;
    [SerializeField]
    private float _timeStoped;
    private float[] _parabolaEquation;
    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _sentido = true;
        _rb2D.velocity = new Vector2(BlossomData.MaxSpeedX, BlossomData.MaxSpeedY);
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
        if (_sentido)
        {
            _rb2D.velocity = new Vector2(_rb2D.velocity.x + BlossomData.MaxSpeedX / 100, _parabolaEquation[0] * Mathf.Pow(_rb2D.velocity.x, 2) + _parabolaEquation[1] * _rb2D.velocity.x + _parabolaEquation[2]);
        }
        else
        {
            _rb2D.velocity = new Vector2(_rb2D.velocity.x + BlossomData.MinSpeedX / 100, _parabolaEquation[0] * Mathf.Pow(_rb2D.velocity.x, 2) + _parabolaEquation[1] * _rb2D.velocity.x + _parabolaEquation[2]);
        }
        if (_rb2D.velocity.x >= BlossomData.MaxSpeedX)
        {
            _sentido = false;
            StopVertically();
            //_rb2D.velocity = new Vector2(_maxSpeedX, 0);
        }

        if (_rb2D.velocity.x <= BlossomData.MinSpeedX)
        {
            _sentido = true;
            StopVertically();
            //_rb2D.velocity = new Vector2(_minSpeedX, 0);
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
        _rb2D.velocity = new Vector2(_rb2D.velocity.x, 0);
        StartCoroutine(WaitTime(_timeStoped));
    }

    private IEnumerator WaitTime(float time)
    {
        _stop = true;
        _rb2D.velocity = new Vector2(_rb2D.velocity.x/2, 0);
        yield return new WaitForSeconds(time);
        _rb2D.velocity = new Vector2(_rb2D.velocity.x, 0);
        _stop = false;
    }

    public void StopAllForce(float time)
    {
        StartCoroutine(WaitTimeStop(time));
    }

    private IEnumerator WaitTimeStop(float time)
    {
        float speedX = _rb2D.velocity.x;
        float speedY = _rb2D.velocity.y;
        _stop = true;
        _rb2D.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(time);
        _rb2D.velocity = new Vector2(speedX, speedY);
        _stop = false;
    }
    public void ChangeLayersCoroutine()
    {
        StartCoroutine(WaitTillChangeLayer(0.5f));
    }
    private IEnumerator WaitTillChangeLayer(float time)
    {
        gameObject.layer = 8;
        yield return new WaitForSeconds(time);
        gameObject.layer = 6;
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

    public object Destroy(object nothing)
    {
        StopAllCoroutines();
        Destroy(gameObject,0.1f);
        return null;
    }
}
