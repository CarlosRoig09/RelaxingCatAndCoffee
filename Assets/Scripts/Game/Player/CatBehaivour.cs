using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using personalLibrary;

public class CatBehaivour : MonoBehaviour, IHaveTheEvent
{
    [Header("CatMovement")]

    [SerializeField]
    private float _speed;
    private Rigidbody2D _rb2D;
    private bool _reduceSpeed;
    private bool _velSign;
    [SerializeField] 
    private float _reduceSpeedValue;

    [Header("CatAnimation")]

    private CatAnimationController _catAnimCon;
    private SpriteRenderer _catSpriteRenderer;

    [Header("CatAttack")]

    [SerializeField]
    private GameObject _catAttack;
    [SerializeField]
    private float _amountToScaleX;
    [SerializeField]
    private float _amountToScaleY;
    [SerializeField]
    private float _maxScaleX;
    [SerializeField]
    private float _waiTimeTillDestroy;
    [SerializeField]
    private float _waitBlossomTime;
    [SerializeField]
    private float _minVectorDistance;
    [SerializeField]
    private float _mediumVectorDistance;
    [SerializeField]
    private float _maxVectorDistance;
    [SerializeField]
    private float _maxStrenght;
    [SerializeField]
    private float _mediumStrenght;
    [SerializeField]
    private float _beforeMinStrenght;
    [SerializeField]
    private float _minStrenght;
    [SerializeField]
    private float _maxForce;

    public event IHaveTheEvent.IHaveTheEvent IHTEvent;

    public EnumLibrary.TypeOfEvent Type { get => EnumLibrary.TypeOfEvent.AttackFinish; set => throw new System.NotImplementedException(); }

    // Start is called before the first frame update
    void Start()
    {
        _rb2D= GetComponent<Rigidbody2D>();
        _reduceSpeed = false;
        _catAnimCon= GetComponent<CatAnimationController>();
        _catSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_reduceSpeed)
        {
            if (_velSign)
            {
                _rb2D.velocity = new Vector2(_rb2D.velocity.x - _reduceSpeedValue, 0);
                if (_rb2D.velocity.x <= 0)
                {
                    _reduceSpeed = false;
                    _rb2D.velocity = Vector2.zero;
                }
            }
            else
            {
                _rb2D.velocity = new Vector2(_rb2D.velocity.x + _reduceSpeedValue, 0);
                if (_rb2D.velocity.x >= 0)
                {
                    _reduceSpeed = false;
                    _rb2D.velocity = Vector2.zero;
                }
            }
        }
    }

    public void Movement(Vector2 direction)
    {
        _reduceSpeed = false;
        _rb2D.velocity = new Vector2(_speed*direction.x,0);
        if (_rb2D.velocity.x < 0)
        {
            _catSpriteRenderer.flipX = true;
            _velSign = false;
        }
        else { _velSign = true; _catSpriteRenderer.flipX = false; }
    }

    public void StopMovement()
    {
        _reduceSpeed = true;
    }

    public void Attack()
    {
        StopMovement();
        _catAnimCon.StartMiauAttack();
        AudioManager.instance.Play("Meow");
    }

    public void SpawnExplosion()
    {
      if(Instantiate(_catAttack, transform.GetChild(0).transform)
            .TryGetComponent<CatForce>(out var catForceScript))
        {
            catForceScript.AmountToScaleX = _amountToScaleX;
            catForceScript.AmountToScaleY = _amountToScaleY;
            catForceScript.WaitTimeTillDestroy = _waiTimeTillDestroy;
            catForceScript.WaitBlossomTime = _waitBlossomTime;
            catForceScript.MinVectorDistance = _minVectorDistance;
            catForceScript.MediumVectorDistance = _mediumVectorDistance;
            catForceScript.MaxVectorDistance = _maxVectorDistance;
            catForceScript.MinStrenght = _minStrenght;
            catForceScript.MaxStrenght = _maxStrenght;
            catForceScript.MediumStrenght = _mediumStrenght;
            catForceScript.BeforeMinStrenght = _beforeMinStrenght;
            catForceScript.MaxForce = _maxForce;
        }
    }

    public void WaitTillAtackFinish()
    {
        string placeholder = string.Empty;
        IHTEvent(placeholder);
    }
}
