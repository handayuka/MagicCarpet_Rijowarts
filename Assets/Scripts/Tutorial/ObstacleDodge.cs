using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ObstacleDodge : SingletonMonoBehaviour<ObstacleDodge>
{

    [SerializeField] GameObject TutorialSet2 = null;
    [SerializeField] GameObject ColaGroupe = null;

    private TutorialState tutorialState = TutorialState.Judging;

    private int count = 0;

    public void Init()
    {
        Debug.Log("@@@obstacle");
        //デバッグ用
        //Invoke("onNextTutorial", 2.0f);

        this.ObserveEveryValueChanged(x => UIManager.Instance.life)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => checkLife());

        this.ObserveEveryValueChanged(x => TutorialManager.Instance.tutorialState)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => tutorialState = TutorialState.Success);

        this.ObserveEveryValueChanged(x => tutorialState)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => sendToManager());

        TutorialSet2.SetActive(true);
        if (count == 0)
        {
            ColaGroupe.SetActive(true);
        }
        else
        {
            ColaGroupe.SetActive(false);
        }
    }

    private void checkLife(){
        if(UIManager.Instance.life == 0 && count == 0) {
            tutorialState = TutorialState.Failure;
        }else if(UIManager.Instance.life == 0 && count == 1) {
            tutorialState = TutorialState.Success;
        }
    }

    private void sendToManager()
    {
        TutorialSet2.SetActive(false);

        if (tutorialState == TutorialState.Success)
        {
            TutorialManager.Instance.changeGameState(GameState.GemGetContinuousTutorial);
        }
        else if (tutorialState == TutorialState.Failure && count == 0)
        {
            count += 1;
            TutorialManager.Instance.changeGameState(GameState.ObstacleTutorial);
        }
    }
}
