using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCorner : SingletonMonoBehaviour<TurnCorner>
{
    public void Init(int rightOrLeft)
    {
        if(rightOrLeft == 0){ //right
            Debug.Log("@@@turnRight");
            //デバッグ用
            //Invoke("onNextTutorial", 2.0f);
        }
        else{ //left
            Debug.Log("@@@turnLeft");
            //デバッグ用
            //Invoke("readyToStart", 2.0f);
        }
    }
    public TutorialState judgeTutorialState()
    {
        return TutorialState.Failure;
    }

    private void onNextTutorial()
    {
        TutorialManager.Instance.changeGameState(GameState.TurnLeftTutorial);
    }

    private void readyToStart()
    {
        TutorialManager.Instance.changeGameState(GameState.Start);
    }
}
