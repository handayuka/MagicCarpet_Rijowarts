using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TimeManager : SingletonMonoBehaviour<TimeManager>
{

    private float time   = 0.0f;
    private bool onTimer = false;
 

    void Start()
    {
        this.ObserveEveryValueChanged(x => time)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => UIManager.Instance.UpdateTimeText());
    }

    // Update is called once per frame
    void Update()
    {
        if(onTimer){
            time += Time.deltaTime;
        }
    }

    public float getTime() {
        return time;
    }

    public void startTimer() {
        onTimer = true;
    }

    public void stopTimer(){
        onTimer = false;
    }
}
