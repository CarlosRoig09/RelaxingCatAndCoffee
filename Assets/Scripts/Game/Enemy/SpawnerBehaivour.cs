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
     GameObject gameObject = Instantiate(blossom.BlossomGameObject, _spawnPoints[Random.Range(0,_spawnPoints.Length)].transform.position, Quaternion.identity);
        gameObject.GetComponent<BlossomBehaivour>().BlossomData= Instantiate(blossom);
        _time = 0;
       _maxTime = LevelManager.Instance.SpawnTimeByPun();
        Debug.Log("MaxTime: " + _maxTime);
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
