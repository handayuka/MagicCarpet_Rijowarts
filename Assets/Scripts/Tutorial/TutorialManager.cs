using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


public enum TutorialState {
    Judging,
    Success,
    Failure,
    Goal
}

public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
{
    [SerializeField] GameObject player = null;
    [SerializeField] GameObject[] points = null; //0:GemAndObstacle 1:TurnRight 2:TurnLeft 3:Start


    public TutorialState tutorialState = TutorialState.Judging;
    private GameState gameState = GameState.Start;

    void Start()
    {
        this.ObserveEveryValueChanged(x => gameState)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => startTutorialsAndMain());

        //デバッグ用
        gameState = GameState.GemGetSingleTutorial;
        GameManager.Instance.GameState = gameState;
    }


    void startTutorialsAndMain()
    {
        switch (gameState)
        {
            case GameState.Playing:
                break;

            case GameState.GemGetSingleTutorial:
                player.transform.position = points[0].transform.position;
                tutorialState = TutorialState.Judging;
                GemGetSingle.Instance.Init(points[0].transform.position);
                Debug.Log("@@@SingleInit");
                break;

            case GameState.GemGetContinuousTutorial:
                player.transform.position = points[0].transform.position;
                tutorialState = TutorialState.Judging;
                GemGetContinuous.Instance.Init();
                break;

            case GameState.ObstacleTutorial:
                player.transform.position = points[0].transform.position;
                tutorialState = TutorialState.Judging;
                ObstacleDodge.Instance.Init();
                break;

            case GameState.TurnRightTutorial:
                player.transform.position = points[1].transform.position;
                tutorialState = TutorialState.Judging;
                TurnCorner.Instance.Init(0);
                break;

            case GameState.TurnLeftTutorial:
                player.transform.position = points[2].transform.position;
                tutorialState = TutorialState.Judging;
                //player.transform.forward = player.transform.right;
                TurnCorner.Instance.Init(1);
                break;

            case GameState.Start:
                player.transform.position = points[3].transform.position;
                /*ゲームスタートの処理*/
                break;

            default://デフォルト（それ以外の時全部）の場合
                break;
        }
    }

        public void changeGameState(GameState _gamestate)
        {
            gameState = _gamestate;
            GameManager.Instance.GameState = gameState;
        }
    }

