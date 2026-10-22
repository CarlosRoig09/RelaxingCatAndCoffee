using System;
using UnityEngine;
namespace TimerSystem{
public enum TimerType
{
    Ascend,
    Descend
}
[Serializable]
public class Timer
{
    #region TimerData
    //Every set should have a specific check fuction no cover all logical posibilities.
   public TimerType Type {get=> _type; set=> _type = value;}
   private TimerType _type;
   public float TotalTime{get=> _totalTime; set=>_totalTime =value;}
   private float _totalTime;
   public float CurrentTime {get=> _currentTime;}
   private float _currentTime;
   public float EndTime{get=> _endTime;}
   private float _endTime;
   public float InitTime{get=> _initTime;}
   private float _initTime; 
   public bool IsRunning {get=>_isRunning;}
   public bool IsFinished{get=>!_isRunning;}
   private bool _isRunning = false;
   public bool IsPaused {get=>_isPaused;}
   private bool _isPaused = false;
   public Action OnTimerStart{get=>_onTimerStart;set=>_onTimerStart = value;}
   public Action _onTimerStart;
   public Action OnTimerComplete{get=>_onTimerComplete; set=>_onTimerComplete = value;}
    private Action _onTimerComplete;
    public Action<float> OnTimerUpdate{get=>_onTimerUpdate; set=>_onTimerUpdate = value;}
    private Action<float> _onTimerUpdate;
    private const float TIME_ZERO = 0f;

    #endregion
    public Timer()
    {
      
    }
   public Timer(float time, TimerType type, Action onTimerStart, Action<float> onTimerUpdate, System.Action onTimerComplete)
   {
     _totalTime = time;
     _initTime = type==TimerType.Descend? _totalTime : TIME_ZERO;
     _endTime = type==TimerType.Descend? TIME_ZERO : _totalTime;
     _currentTime = _initTime;
    _onTimerStart = onTimerStart;
     _onTimerUpdate = onTimerUpdate;
     _onTimerComplete = onTimerComplete;
     _type = type;
   }

   public void StartTimer()
   {
     _isRunning = true;
     _isPaused = false;
     _initTime = _type==TimerType.Descend? _totalTime : TIME_ZERO;
     _endTime = _type==TimerType.Descend? TIME_ZERO : _totalTime;
     _currentTime = _initTime; 
     _onTimerStart?.Invoke();
   }

   //Check if timer is finished;
   public void UpdateTimer(float frameTime)
   {
        if (_type == TimerType.Ascend)
        {
            _currentTime+=frameTime;
            if (_currentTime >= _endTime)
            {
                _currentTime = _endTime;
                _isRunning = false;
                _onTimerComplete?.Invoke();
                return;
            }
        }
        else
        {
            _currentTime-=frameTime;
            if (_currentTime <= _endTime)
            {
                _currentTime = _endTime;
                _isRunning = false;
                _onTimerComplete?.Invoke();
                return;
            }
        }
        _onTimerUpdate?.Invoke(_currentTime);
   }
   
   public void ResumeTimer()
   {
      _isPaused = false;  
   }

   public void PauseTimer()
   {
     _isPaused = true;
   }

   public void StopTimer()
   {
     _isRunning = false;
   }
}
}
