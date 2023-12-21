using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesMethods : MonoBehaviour
{
    [SerializeField]
    private int _spritescount;
    [SerializeField]
    private Sprite[] _sprites;
    public void ChangeSpriteToTheNextOne()
    {
        _spritescount += 1;
        if(_spritescount<_sprites.Length)
        {
            GetComponent<SpriteRenderer>().sprite = _sprites[_spritescount];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = _sprites[0];
            _spritescount = 0;
        }
    }
}
