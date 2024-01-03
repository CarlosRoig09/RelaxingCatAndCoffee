using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CofeeBeanBehaivour : MonoBehaviour
{
    public Vector3 SpawnPosition;
    public Vector3 SpawnDirection;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Rigidbody2D _rb2D;
    // Start is called before the first frame update
    void Start()
    {
       transform.position = SpawnPosition;
        _rb2D= GetComponent<Rigidbody2D>();
    }

    public bool FollowDirection(Vector3 direction)
    {
        _rb2D.velocity = direction * _speed;
        return true;
    }

    public void Stop()
    {
        _rb2D.velocity = Vector3.zero;
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void CofeeMovement()
    {
        Physics.SyncTransforms();
        StartCoroutine(MoveBeanTillTime(new float[] {0.5f,0.3f},new Vector3[] {Vector3.up,SpawnDirection},0,0,FollowDirection));
    }
    private IEnumerator MoveBeanTillTime(float[] times, Vector3[] directions, int directionCount, int timeCount, Func<Vector3,bool> firstFunction)
    {
        firstFunction(directions[directionCount]);
        directionCount += 1;
        yield return new WaitForSecondsRealtime(times[timeCount]);
        timeCount+= 1;
        firstFunction(directions[directionCount]);
        directionCount+= 1;
        if (directions.Length>directionCount&&times.Length>timeCount)
        {
            StartCoroutine(MoveBeanTillTime(times, directions, directionCount, timeCount, firstFunction));
        }
        else
            DestroyGameObject();
    }
}
