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
    private CatForce _catAttack;

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
                _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x - _reduceSpeedValue, 0);
                if (_rb2D.linearVelocity.x <= 0)
                {
                    _reduceSpeed = false;
                    _rb2D.linearVelocity = Vector2.zero;
                }
            }
            else
            {
                _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x + _reduceSpeedValue, 0);
                if (_rb2D.linearVelocity.x >= 0)
                {
                    _reduceSpeed = false;
                    _rb2D.linearVelocity = Vector2.zero;
                }
            }
        }
    }

    public void Movement(Vector2 direction)
    {
        _reduceSpeed = false;
        _rb2D.linearVelocity = _speed*direction;
        if (_rb2D.linearVelocity.x < 0)
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
      if(_catAttack!=null)
        {
            _catAttack.AmountToScaleX = _amountToScaleX;
            _catAttack.AmountToScaleY = _amountToScaleY;
            _catAttack.WaitTimeTillDestroy = _waiTimeTillDestroy;
            _catAttack.WaitBlossomTime = _waitBlossomTime;
            _catAttack.MinVectorDistance = _minVectorDistance;
            _catAttack.MediumVectorDistance = _mediumVectorDistance;
            _catAttack.MaxVectorDistance = _maxVectorDistance;
            _catAttack.MinStrenght = _minStrenght;
            _catAttack.MaxStrenght = _maxStrenght;
            _catAttack.MediumStrenght = _mediumStrenght;
            _catAttack.BeforeMinStrenght = _beforeMinStrenght;
            _catAttack.MaxForce = _maxForce;
            _catAttack.StartExpand();
        }
    }

    public void WaitTillAtackFinish()
    {
        string placeholder = string.Empty;
        IHTEvent(placeholder);
    }
}
