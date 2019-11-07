using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if(GameManager.Instance.GameState != GameState.Playing){
            //UIManager.Instance.SetStatus(Constants.StatusDeadTapToStart);
            this.GameState = GameState.Dead;
            Invoke("GoToGameOver", 1.5f);
        }
            
    }

    void GoToGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }


    public void Goal()
    {
        UIManager.Instance.SetStatus(Constants.StatusGoal);
        this.GameState = GameState.Goal;
    }

}



