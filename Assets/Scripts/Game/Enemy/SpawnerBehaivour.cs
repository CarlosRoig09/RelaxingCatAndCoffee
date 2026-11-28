using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using personalLibrary;
using TimerSystem;
using Utilities;

public class SpawnerBehaivour : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _spawnPoints;
    private float _time;
    private float _maxTime;

    [SerializeField]
    private float speedUpY = 0.2f;
    private Timer _spawnTimer;
    [SerializeField]
    private float maxSpawnTime = 5f;
    [SerializeField]
    private float maxExtraSpeedY = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _maxTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_spawnTimer == null)
        {
            _spawnTimer = TimerManager.Instance.CreateTimer(_maxTime, onTimerComplete: () =>
            {
                SpawnBlossom();
                _spawnTimer = null;
            });
        }
    }
    private void SpawnBlossom()
    {
       BlossomBehaivour blossomBehaivour = ObjectPoolManager.Instance.GiveAPooledObjectFromARandomList<BlossomBehaivour>(RandomSpawnPosition());
    if (blossomBehaivour == null)
        return;
        float puntuationEvaluate = LevelManager.Instance.CurrentPuntuationCurve();
        blossomBehaivour.MaxSpeedY = blossomBehaivour.BaseSpeedY - (maxExtraSpeedY - maxExtraSpeedY * puntuationEvaluate);
        _maxTime = maxSpawnTime * puntuationEvaluate;
        Debug.Log("Max Time: " + _maxTime);
    }

    private Vector2? RandomSpawnPosition()
    {
        return new Vector3(Random.Range(_spawnPoints[0].transform.position.x, _spawnPoints[1].transform.position.x), _spawnPoints[0].transform.position.y);
    }

    //public void SpeedUp()
    //{
    //    foreach (var blossom in blossoms)
    //    {
    //        blossom.MaxSpeedY -= speedUpY;
    //    }
    //}


    //public void RecoverBaseSpeed()
    //{
    //    foreach (var blossom in blossoms)
    //    {
    //        blossom.MaxSpeedY = blossom.BaseMaxSpeedY;
    //    }
    //}
}
