using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using personalLibrary;

public class SpawnerBehaivour : MonoBehaviour
{
    [SerializeField]
    private BlossomData[] blossoms;
    [SerializeField]
    private GameObject[] _spawnPoints;
    private float _time;
    private float _maxTime;
    // Start is called before the first frame update
    void Start()
    {
        _time = 0;
        _maxTime = 0;
        foreach (var specialBlossom in blossoms)
        {
            specialBlossom.MaxSpeedY = specialBlossom.BaseMaxSppeedY;
            if (specialBlossom.Name == "Special")
            {
                specialBlossom.SpawnPercentage = 10;
                LevelManager.Instance.GetSpecialBlossom(specialBlossom);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_time >= _maxTime)
        {
            SpawnBlossom(BlossomRandomizer());
        }
        else
            _time += Time.deltaTime;
    }
    private void SpawnBlossom(BlossomData blossom)
    {
     GameObject gameObject = Instantiate(blossom.BlossomGameObject, new Vector3(Random.Range(_spawnPoints[0].transform.position.x, _spawnPoints[1].transform.position.x), _spawnPoints[0].transform.position.y), Quaternion.identity);
        gameObject.GetComponent<BlossomBehaivour>().BlossomData= Instantiate(blossom);
        _time = 0;
       _maxTime = LevelManager.Instance.SpawnTimeByPun();
        Debug.Log("Max Time: " + _maxTime);
    }

    public void SpeedUp()
    {
        foreach(var blossom in blossoms)
        {
            blossom.MaxSpeedY -= 0.1f;
        }
    }

    private BlossomData BlossomRandomizer()
    {
        float[] BlossomPercentages = new float[blossoms.Length];
        for(int i = 0; i < BlossomPercentages.Length; i++)
        {
            BlossomPercentages[i] = blossoms[i].SpawnPercentage;
        }
        return blossoms[RandomMethods.ReturnARandomObject(BlossomPercentages,0,blossoms.Length,0)];
    }
}
