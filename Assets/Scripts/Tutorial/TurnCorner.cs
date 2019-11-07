using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public interface ISceneWasLoaded : IEventSystemHandler
{
    void OnSceneWasLoaded(int pattern);
}

public static class SceneManagerEx
{
    static public void LoadSceneWithArg(
         string sceneName,
         int pattern,
         LoadSceneMode mode)
    {
        UnityAction<Scene, LoadSceneMode> sceneLoaded = default;
        Action removeHandler = () =>
        {
            SceneManager.sceneLoaded -= sceneLoaded;
        };

        sceneLoaded = (loadedScene, sceneMode) =>
        {
            removeHandler();
            foreach (var root in loadedScene.GetRootGameObjects())
            {
                ExecuteEvents.Execute<ISceneWasLoaded>(root, null, (receiver, e) => receiver.OnSceneWasLoaded(pattern));
            }
        };

        SceneManager.sceneLoaded += sceneLoaded;
        SceneManager.LoadScene(sceneName, mode);
    }
}


public class TurnCorner : SingletonMonoBehaviour<TurnCorner>, ISceneWasLoaded
{
    public int sceneNum = 0; //0:right 1:left

    [SerializeField] NewRotate player;
    [SerializeField] GameObject[] points; //0:right 1:left

    private TutorialState tutorialState = TutorialState.Judging;

    private bool turnRight = false;
    private bool turnLeft = false;

    private int turnCount = 0;

    public bool hitFailureObject = false;


    public void OnSceneWasLoaded(int pattern)
    {
        sceneNum = pattern;
    }


    private void Start()
    {
        Init(sceneNum);
    }


    public void Init(int rightOrLeft)
    {
        if (rightOrLeft == 0)
        { //right

            GameManager.Instance.GameState = GameState.TurnRightTutorial;
            player.transform.position = points[0].transform.position;
            player.transform.LookAt(new Vector3(0, -90, 0));
            player.SpeedChange(6.0f);

            turnLeft = true;
            turnCount = 1;
        }
        else
        { //left

            GameManager.Instance.GameState = GameState.TurnLeftTutorial;
            player.transform.position = points[1].transform.position;
            player.SpeedChange(6.0f);

            turnLeft = false;
            turnCount = 0;
        }

        turnRight = false;
        hitFailureObject = false;
        tutorialState = TutorialState.Judging;

        this.ObserveEveryValueChanged(x => hitFailureObject)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => tutorialState = TutorialState.Failure);

        this.ObserveEveryValueChanged(x => player.turn)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => CheckTurn(player.turn));

        this.ObserveEveryValueChanged(x => tutorialState)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe(_ => CheckChangeScene());
    }

    private void CheckTurn(int turnNum)
    {

        if (turnNum == 0)
        {

        }
        else if (turnNum == 1 && turnCount == 1)
        { //right
            tutorialState = TutorialState.Success;
        }
        else if (turnNum == 2 && turnCount == 0) // left
        {
            turnCount += 1;
            turnLeft = true;
        }

    }

    private void CheckChangeScene()
    {
        if (tutorialState == TutorialState.Success)
        {
            Invoke("ReadyToStart", 2.0f);
        }
        else if (tutorialState == TutorialState.Failure)
        {
            if (!turnRight && !turnLeft)
            {
                Observable.Timer(TimeSpan.FromMilliseconds(200))
                      .Subscribe(_ => ReloadThisScene(1));
            }
            else if (!turnRight && turnLeft)
            {
                Observable.Timer(TimeSpan.FromMilliseconds(200))
                     .Subscribe(_ => ReloadThisScene(0));
            }
        }
    }

    private void ReadyToStart()
    {
        SceneManager.LoadScene("rotatedPathsLevel");
    }

    private void ReloadThisScene(int num)
    {
        SceneManagerEx.LoadSceneWithArg("tutorialTurn", num, LoadSceneMode.Single);
    }



}
