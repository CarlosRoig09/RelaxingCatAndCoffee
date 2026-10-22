using System;
using System.Collections.Generic;
using UnityEngine;
namespace TimerSystem{
public class TimerManager : AbstractSingleton<TimerManager>
{
    public List<Timer> ActiveTimers {get=>_activeTimers;}
    private List<Timer> _activeTimers = new List<Timer>();
    private List<Timer> _timersToRemove = new List<Timer>();
    // Update is called once per frame
    void Update()
    {
       UpdateTimers(Time.deltaTime);
    }

    public Timer CreateTimer(float time, TimerType timerType = TimerType.Descend, Action onTimerStart = null, Action<float> onTimerUpdate = null, Action onTimerComplete = null)
    {
     if(time<0){
        Debug.LogError("The time value given was negative and time can't be negative.");
        return null;
     }
        Timer currentTimer = new Timer(time,timerType,onTimerStart,onTimerUpdate,onTimerComplete);
        _activeTimers.Add(currentTimer);
      return currentTimer;   
    }

    public void StartTimer(Timer timerToStart)
    {
        if (timerToStart == null)
        {
            Debug.LogError("Timer given to start is null.");                
            return;
        }
        if (!_activeTimers.Contains(timerToStart))
        {
            _activeTimers.Add(timerToStart);
        }
        timerToStart.StartTimer();      
    }

    private void UpdateTimers(float deltaTime)
    {
      foreach(Timer activeTimer in _activeTimers)
      {
        if (activeTimer.IsRunning && !activeTimer.IsPaused)
        {
          activeTimer.UpdateTimer(deltaTime);
          if (!activeTimer.IsRunning)
            {
              _timersToRemove.Add(activeTimer);        
            }          
        }       
      }
      if(_timersToRemove.Count>0){
      RemoveNotRunningTimers();   
      } 
    }
    private void RemoveNotRunningTimers()
    {
       foreach(Timer timerToRemove in _timersToRemove)
        {
            _activeTimers.Remove(timerToRemove);    
        }
        _timersToRemove.Clear();     
    }

    public void ModifyTimer(Timer timer,float? newTime = null, TimerType? timerType = null, Action onTimerStart = null, Action<float> onTimerUpdate = null, Action onTimerComplete = null)
    {
        if(timer==null)
            return;
        if (newTime != null)
        {
           timer.TotalTime = (float)newTime;
           timer.StartTimer();     
        }
        if (timerType != null)
        {
           timer.Type = (TimerType)timerType;
           timer.StartTimer();           
        }
        if (onTimerStart != null)
        {
           timer.OnTimerStart = onTimerStart;     
        }
        if (onTimerUpdate != null)
        {
           timer.OnTimerUpdate = onTimerUpdate;     
        }
        if (onTimerComplete != null)
        {
            timer.OnTimerComplete = onTimerComplete;    
        }      
    }
}
}
