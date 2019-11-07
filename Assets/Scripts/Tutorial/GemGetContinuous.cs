using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGetContinuous : SingletonMonoBehaviour<GemGetContinuous>
{
    public void Init(){

        Debug.Log("@@@gemContinuous");
        //デバッグ用
        //Invoke("onNextTutorial", 2.0f);
    }

    private TutorialState judgeTutorialState()
    {
        return TutorialState.Failure;
    }

    private void onNextTutorial()
    {
        TutorialManager.Instance.changeGameState(GameState.ObstacleTutorial);
    }

}
