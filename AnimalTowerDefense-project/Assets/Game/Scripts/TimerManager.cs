using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATD;

public class TimerManager : Singleton<TimerManager>
{
    public struct Timer
    {
        public delegate void TimerCallback(string Name);
        public string Name;
        public float timer;
        private TimerCallback _callback;

        public Timer(string name, TimerCallback callback)
        {
            Name = name;
            timer = 0f;
            _callback = callback;
        }

        public void CallCallback()
        {
            _callback(Name);
        }
    }

    public List<Timer> Timers { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Timers = new List<Timer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Timers.Count > 0)
        {
            for( int i = 0; i < Timers.Count; i++)
            {
                if(Timers[i].timer > 0)
                {
                    Timer t_time = Timers[i];
                    t_time.timer -= Time.deltaTime;
                    Timers[i] = t_time;
                }
                else
                {
                    Timers[i].CallCallback();
                    Timers.Remove(Timers[i]);
                }
            }
        }
    }

    public Timer? AddTimer(string Name, float timer, Timer.TimerCallback callback)
    {
        if(!Timers.Exists(x => x.Name == Name))
        {
            Timer t_timer = new Timer(Name, callback);
            t_timer.timer = timer;
            Timers.Add(t_timer);
            return t_timer;
        }
        else
        {
            Debug.LogError("Timer is already exist");
        }
        return null;
    }

    public void RemoveTimer(string name)
    {
        if (Timers.Exists(x => x.Name == name))
        {
            Timers.Remove(Timers.Find(x => x.Name == name));
        }
    }

    public bool Exist(string Name)
    {
        return Timers.Exists(x => x.Name == Name);
    }

    protected override void Init()
    {

    }
}
