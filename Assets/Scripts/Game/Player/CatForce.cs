using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using personalLibrary;
using UnityEngine.UI;

public class CatForce : MonoBehaviour
{
    private EnumLibrary.CatForceState _state;
    private SpriteRenderer _spriteRenderer;
    private bool _startedCoroutine;
    public float MaxForce { private get; set ;}
    public delegate void IFinishedTheAttack();
    public event IFinishedTheAttack OnFinishedTheAttack;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer= GetComponent<SpriteRenderer>();
        _state = EnumLibrary.CatForceState.Expand;
        MaxForce = 500;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startedCoroutine = false;
        OnFinishedTheAttack += GameObjectLibrary.Instance.CatBehaivourScript.WaitTillAtackFinish;
    }

    // Update is called once per frame
    void Update()
    {
        if(_state== EnumLibrary.CatForceState.Expand)
        Expand();
        else
            Destroy();
    }

    private void Expand()
    {
        if (transform.localScale.x < 5.23f)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.1f, transform.localScale.y + 0.06f);
        }
        
        else 
        {
            if (!_startedCoroutine)
            {
                StartCoroutine(WaitTime(0.1f));
            }
        }
    }

    private void Destroy()
    {
        Debug.Log("Me estoy destruyendo");
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a - 0.01f);
        transform.localScale = new Vector3(transform.localScale.x + 0.1f*Time.deltaTime, transform.localScale.y + 0.06f*Time.deltaTime);
        if (_spriteRenderer.color.a <= 0)
        {
            Destroy(gameObject);
            OnFinishedTheAttack();
        }

    }

    private IEnumerator WaitTime(float time)
    {
        _startedCoroutine= true;
        yield return new WaitForSeconds(time);
        _state = EnumLibrary.CatForceState.Destroy;
        _startedCoroutine= false;
    }

    //See the distance between the center and the object and give a direction and force depending of the resultant vector.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<BlossomBehaivour>(out var blossomBehaivour))
        {
            blossomBehaivour.StopAllForce(0.3f);
            var force = (MaxForce / (Mathf.Sqrt(Mathf.Pow(blossomBehaivour.transform.position.x - transform.position.x,2)+Mathf.Pow(blossomBehaivour.transform.position.y - transform.position.y,2)))) * new Vector2(blossomBehaivour.transform.position.x - transform.position.x, blossomBehaivour.transform.position.y - transform.position.y);
            blossomBehaivour.Rb2D.AddForce(force);
            blossomBehaivour.gameObject.layer = 8;
        }
    }

}