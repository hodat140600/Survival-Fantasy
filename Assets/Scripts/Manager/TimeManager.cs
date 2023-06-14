//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
using System.Collections;
using UniRx;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    //#region Code c?a Chung CC
    //private Stopwatch _stopWatch;
    //[SerializeField][Tooltip("The time between 2 event")] private float _timeUnit;

    //private void Awake()
    //{
    //    _stopWatch = new Stopwatch();
    //}

    //[ContextMenu("Start Counter")]
    //public void StartCounter()
    //{
    //    _stopWatch = Stopwatch.StartNew();
    //    StartCoroutine(CountTimeUnit());
    //}

    //public void ResumeTimerCount()
    //{
    //    _stopWatch.Start();
    //}

    //public void PauseTimerCount()
    //{
    //    _stopWatch.Stop();
    //}

    //public void ResetTimerCount()
    //{
    //    StopAllCoroutines();
    //    _stopWatch.Reset();
    //}

    //IEnumerator CountTimeUnit()
    //{
    //    while (true)
    //    {
    //        yield return new WaitUntil(() => _stopWatch.IsRunning);
    //        yield return new WaitForSeconds(_timeUnit);

    //        UnityEngine.Debug.Log("Ticker event run " + _stopWatch.Elapsed.Seconds);
    //        MessageBroker.Default.Publish(new TickEvent { secondNumber = _stopWatch.Elapsed.Seconds });
    //    }
    //}
    //#endregion

    #region Stop watch ch?y b?ng c?m 
    [SerializeField][Tooltip("The time between 2 event")] private static float _timeUnit = 5f;

    private float elapsedRunningTime = 0f;
    //private float runningStartTime = 0f;
    //private float pauseStartTime = 0f;
    private float elapsedPausedTime = 0f;
    //private float totalElapsedPausedTime = 0f;
    private bool running = false;
    private bool paused = false;
    WaitForSeconds forSeconds = new WaitForSeconds(_timeUnit);
    [ContextMenu("Start Counter")]

    //void Update()
    //{
    //    if (running)
    //    {
    //        elapsedRunningTime = Time.time - runningStartTime - totalElapsedPausedTime;
    //    }
    //    else if (paused)
    //    {
    //        elapsedPausedTime = Time.time - pauseStartTime;
    //    }
        
    //}
    IEnumerator SendMessage()
    {
        while (true)
        {
            yield return forSeconds;
            elapsedRunningTime += _timeUnit;
            //Debug.Log("Ticker event run " + GetSeconds());
            MessageBroker.Default.Publish(new TickEvent { secondNumber = GetSeconds() });
        }
    }

    public void Begin()
    {
        //if (!running && !paused)
        //{
            //runningStartTime = Time.time;
            //running = true;
            StopAllCoroutines();
            StartCoroutine(SendMessage());
        //}
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    //public void Pause()
    //{
    //    if (running && !paused)
    //    {
    //        running = false;
    //        pauseStartTime = Time.time;
    //        paused = true;
    //    }
    //}

    //public void Unpause()
    //{
    //    if (!running && paused)
    //    {
    //        totalElapsedPausedTime += elapsedPausedTime;
    //        running = true;
    //        paused = false;
    //    }
    //}

    public void Reset()
    {
        elapsedRunningTime = 0f;
        //runningStartTime = 0f;
        //pauseStartTime = 0f;
        elapsedPausedTime = 0f;
        //totalElapsedPausedTime = 0f;
        running = false;
        paused = false;
    }

    public int GetMinutes()
    {
        return (int)(elapsedRunningTime / 60f);
    }

    public int GetSeconds()
    {
        return (int)(elapsedRunningTime);
    }

    public float GetMilliseconds()
    {
        return (float)(elapsedRunningTime - System.Math.Truncate(elapsedRunningTime));
    }

    public float GetRawElapsedTime()
    {
        return elapsedRunningTime;
    }

    #endregion
}
