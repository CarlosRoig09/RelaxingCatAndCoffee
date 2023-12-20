using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
    private Animator _anim;
    private Rigidbody2D _rb2d;
    private float _idleTime;
    [SerializeField]
    private float _idleMaxTime;
    // Start is called before the first frame update
    void Start()
    {
        _anim= GetComponent<Animator>();
        _rb2d=GetComponent<Rigidbody2D>();
        _idleTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetFloat("Movement",_rb2d.velocity.x);
        if (_rb2d.velocity.x == 0)
        {
            _anim.SetBool("Idle", true);
            if(_idleTime<_idleMaxTime)
                _idleTime += Time.deltaTime;
            else
                _idleTime= 0;
        }
        else
        {
            _anim.SetBool("Idle", false);
            _idleTime= 0;
        }
        _anim.SetFloat("Time", _idleTime);
    }

    public void StartMiauAttack()
    {
        _anim.SetBool("Attack",true);
    }

    public void EndMiauAttack()
    {
        _anim.SetBool("Attack", false);
    }
}
