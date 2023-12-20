using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using personalLibrary;

public class CatBehaivour : MonoBehaviour, IHaveTheEvent
{
    [SerializeField]
    private float _speed;
    private Rigidbody2D _rb2D;
    private bool _reduceSpeed;
    private bool _velSign;
    [SerializeField] 
    private float _reduceSpeedValue;
    [SerializeField]
    private GameObject _catAttack;
    private CatAnimationController _catAnimCon;

    public event IHaveTheEvent.IHaveTheEvent IHTEvent;

    public EnumLibrary.TypeOfEvent Type { get => EnumLibrary.TypeOfEvent.AttackFinish; set => throw new System.NotImplementedException(); }

    // Start is called before the first frame update
    void Start()
    {
        _rb2D= GetComponent<Rigidbody2D>();
        _reduceSpeed = false;
        _catAnimCon= GetComponent<CatAnimationController>();
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
        _rb2D.velocity = new Vector2(_speed*direction.y,0);
        if (_rb2D.velocity.x < 0)
        {
            _velSign= false;
        }
        else _velSign= true;
    }

    public void StopMovement()
    {
        _reduceSpeed = true;
    }

    public void Attack()
    {
        Debug.Log("Miau Attack");
        _catAnimCon.StartMiauAttack();
    }

    public void SpawnExplosion()
    {
        Instantiate(_catAttack, transform.GetChild(0).transform);
    }

    public void WaitTillAtackFinish()
    {
        string placeholder = string.Empty;
        IHTEvent(placeholder);
    }
}
