using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public class GemGetSingle : SingletonMonoBehaviour<GemGetSingle>
{

    [SerializeField] GameObject Tutorial1Set        = null;
    [SerializeField] GameObject GemGroupeRight      = null;
    [SerializeField] GameObject GemGroupeLeft       = null;
    [SerializeField] GameObject GemGroupeRightYobi  = null;
    [SerializeField] GameObject GemGroupeLeftYobi   = null;


    private bool getGemRight = false;
    private bool getGemLeft = false;

    private TutorialState tutorialState = TutorialState.Judging;

    private int count = 0;

    public void Init(Vector3 point)
    {
        //デバッグ用
        //Invoke("onNextTutorial", 2.0f);

        getGemRight = false;
        getGemLeft = false;

        Tutorial1Set.SetActive(true);
        if(count == 0){
            GemGroupeRight.SetActive(true);
            GemGroupeLeft.SetActive(true);
            GemGroupeRightYobi.SetActive(false);
            GemGroupeLeftYobi.SetActive(false);

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
        else if(count == 1){
            GemGroupeRight.SetActive(false);
            GemGroupeLeft.SetActive(false);
            GemGroupeRightYobi.SetActive(true);
            GemGroupeLeftYobi.SetActive(true);

            this.ObserveEveryValueChanged(x => GemGroupeRightYobi.transform.childCount)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => checkGemGetAndSet(true, getGemLeft));

            this.ObserveEveryValueChanged(x => GemGroupeLeftYobi.transform.childCount)
                .Skip(1) //最初の一回の値の変動をスキップする
                .Subscribe(_ => checkGemGetAndSet(getGemRight, true));

            this.ObserveEveryValueChanged(x => TutorialManager.Instance.tutorialState)
            .Skip(1) //最初の一回の値の変動をスキップする
                .Subscribe(_ => tutorialState = TutorialState.Success);
        }


        this.ObserveEveryValueChanged(x => tutorialState)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => sendToManager());


    }

    private void GoToTutorialObstacles()
    { SceneManager.LoadScene("tutorialObstacles"); }


    private void checkGemGetAndSet(bool getRight, bool getLeft) {
        getGemRight = getRight;
        getGemLeft = getLeft;

        if (getGemRight && getGemRight) {
            tutorialState = TutorialState.Success;
            Invoke("GoToTutorialObstacles", 0.5f);
        }

    }


    private void sendToManager()
    {
        Tutorial1Set.SetActive(false);
        if (tutorialState == TutorialState.Success){
            TutorialManager.Instance.changeGameState(GameState.ObstacleTutorial);
        }else if(tutorialState == TutorialState.Failure && count == 0) {
            count += 1;
            TutorialManager.Instance.changeGameState(GameState.GemGetSingleTutorial);
        }

    }
}
