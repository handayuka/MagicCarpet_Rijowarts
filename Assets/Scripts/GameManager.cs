using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }
    

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    protected GameManager()
    {
        GameState = GameState.Start;
        CanRotate = false;
        CanStep = true;
    }

    public GameState GameState { get; set; }

    public bool CanRotate { get; set; }
    public bool CanStep { get; set; }
    

    public void Die()
    {
            UIManager.Instance.SetStatus(Constants.StatusDeadTapToStart);
            this.GameState = GameState.Dead; 
    }

    public void Goal()
    {
        UIManager.Instance.SetStatus(Constants.StatusGoal);
        this.GameState = GameState.Goal;
    }

}



