using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public class GemGetSingle : SingletonMonoBehaviour<GemGetSingle>
{

    [SerializeField] GameObject Tutorial1Set        = null;
    [SerializeField] GameObject GemGroupeRight      = null;
    [SerializeField] GameObject GemGroupeLeft = null;


    private bool getGemRight = false;
    private bool getGemLeft = false;

    private TutorialState tutorialState = TutorialState.Judging;


    public void Init(Vector3 point)
    {
        //デバッグ用
        //Invoke("onNextTutorial", 2.0f);

        getGemRight = false;
        getGemLeft = false;

        Tutorial1Set.SetActive(true);
        
            GemGroupeRight.SetActive(true);
            GemGroupeLeft.SetActive(true);

            this.ObserveEveryValueChanged(x => GemGroupeRight.transform.childCount)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => checkGemGetAndSet(true, getGemLeft));

            this.ObserveEveryValueChanged(x => GemGroupeLeft.transform.childCount)
                .Skip(1) //最初の一回の値の変動をスキップする
                .Subscribe(_ => checkGemGetAndSet(getGemRight, true));

            this.ObserveEveryValueChanged(x => TutorialManager.Instance.tutorialState)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => tutorialState = TutorialState.Failure);
        
  
    }


        /*this.ObserveEveryValueChanged(x => tutorialState)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => sendToManager());*/

    private void GoToTutorialObstacles()
    { SceneManager.LoadScene("tutorialObstacles"); }


    private void checkGemGetAndSet(bool getRight, bool getLeft) {
        getGemRight = getRight;
        getGemLeft = getLeft;

        if (getGemRight && getGemLeft) {
            tutorialState = TutorialState.Success;
      
        }

    }

    /*
    private void sendToManager()
    {
        Tutorial1Set.SetActive(false);
        if (tutorialState == TutorialState.Success){
            TutorialManager.Instance.changeGameState(GameState.ObstacleTutorial);
        }else if(tutorialState == TutorialState.Failure && count == 0) {
            count += 1;
            TutorialManager.Instance.changeGameState(GameState.GemGetSingleTutorial);
        }

    }*/
}
