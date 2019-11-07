//最新版
﻿using UnityEngine;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using System.Collections;

public class NewRotate : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;//移動方向を零ベクトルと設定
    private Vector3 rotateDirection = Vector3.zero;
    private CharacterController controller;//衝突判定用
    private Animator anim;//アニメーション用
    public Transform CharacterGO;//キャラクター位置用
    private bool isRotate = false;//回転判定用
    private Quaternion targetRotation;//回転後の最終的な向き

    public float gravity = 20f;//重力　ジャンプ用
    public float JumpSpeed = 10.0f;//ジャンプスピード　ジャンプ用
    public float Speed = 6.0f;//スピード　移動用
    public int RotateSpeed = 2;//回転速度　回転用

    bool inInSwipeArea;//移動エリア判定用

    IInputDetector inputDetector = null;//矢印キー入力用

    public SerialHandler serialHandler;//Arduino通信用

    float sensorRight;//超音波センサ右
    float sensorLeft;//超音波センサ左


    //ゲーム開始時
    void Start()
    {
        moveDirection = transform.forward;//移動方向をキャラクターのz軸方向(前方方向)の単位ベクトルに設定
        moveDirection = transform.TransformDirection(moveDirection);//移動方向をローカル空間からワールド空間へ変換
        moveDirection *= Speed;//単位ベクトルにスピード倍する

        UIManager.Instance.ResetScore();//スコアを０に
        UIManager.Instance.ResetLife();//ライフを０に

        UIManager.Instance.SetStatus(Constants.StatusTapToStart);//ステータス(TapToStart)の表示

        GameManager.Instance.GameState = GameState.Start;//ゲーム状態をStartに設定

        //型定義した関数に入れたいもの入れてる
        anim = CharacterGO.GetComponent<Animator>();//animはキャラクターのアニメーションと定義？
        inputDetector = GetComponent<IInputDetector>();//inputDetectorをキー入力と定義
        controller = GetComponent<CharacterController>();//controllerを衝突判定と定義

        serialHandler.OnDataReceived += OnDataReceived;//Arduino通信用

        AudioManager.Instance.PlayBGM("mainBGM");

    }

    //フレーム更新時
    void Update()
    {
        switch (GameManager.Instance.GameState)//ゲーム状態による場合分け
        {
            case GameState.GemGetSingleTutorial:
                if(Input.GetKeyDown(KeyCode.Space)){
                    GameManager.Instance.GameState = GameState.Playing;
                    TutorialManager.Instance.changeGameState(GameState.Playing);
                }
                break;

            case GameState.GemGetContinuousTutorial:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameManager.Instance.GameState = GameState.Playing;
                    TutorialManager.Instance.changeGameState(GameState.Playing);
                }
                break;

            case GameState.ObstacleTutorial:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameManager.Instance.GameState = GameState.Playing;
                    TutorialManager.Instance.changeGameState(GameState.Playing);
                }
                break;

            case GameState.TurnRightTutorial:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameManager.Instance.GameState = GameState.Playing;
                    TutorialManager.Instance.changeGameState(GameState.Playing);
                }
                break;

            case GameState.TurnLeftTutorial:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameManager.Instance.GameState = GameState.Playing;
                    TutorialManager.Instance.changeGameState(GameState.Playing);
                }
                break;

            case GameState.Start://ゲーム状態がスタートの場合
                if (Input.GetMouseButtonUp(0))//マウスのボタンが離された時trueを返す
                {
                    anim.SetBool(Constants.AnimationStarted, true);//AnimationStartedっていうアニメーションをtrueに変更
                    //var i=8;は int i=8;と同じ
                    var instance = GameManager.Instance;//instanceにGameManagerのInstanceを入れてる　インスタンスって何に使うの？
                    instance.GameState = GameState.MainPlaying;//インスタンスのゲーム状態をプレイ中に
                    UIManager.Instance.SetStatus(string.Empty);//ゲーム状態の表示を空に
                    TimeManager.Instance.startTimer();//Timerカウントスタート
                }
                break;

            case GameState.Playing://ゲーム状態がプレイ中の場合
                //UIManager.Instance.IncreaseScore(0.001f);//１フレームごとに点数追加

                //Debug.Log("@@@" + moveDirection);
                if (!isRotate)
                {
                    Move();
                }

                //スコアごとにスピード変更
                /*if(UIManager.Instance.score > 400)
                {
                    Speed = 9.0f;
                    moveDirection = transform.forward;//移動方向をキャラクターのz軸方向(前方方向)の単位ベクトルに設定
                    moveDirection = transform.TransformDirection(moveDirection);//移動方向をローカル空間からワールド空間へ変換
                    moveDirection *= Speed;//単位ベクトルにスピード倍する
                }

                if (UIManager.Instance.score > 900)
                {
                    Speed = 12.0f;
                    moveDirection = transform.forward;//移動方向をキャラクターのz軸方向(前方方向)の単位ベクトルに設定
                    moveDirection = transform.TransformDirection(moveDirection);//移動方向をローカル空間からワールド空間へ変換
                    moveDirection *= Speed;//単位ベクトルにスピード倍する
                }*/

                break;

            case GameState.MainPlaying:
                if (!isRotate)
                {
                    Move();
                }
                break;

            case GameState.Dead://ゲーム状態が死の場合
                anim.SetBool(Constants.AnimationStarted, false);//AnimationStartedっていうアニメーションをfalseに変更
                if(Input.GetMouseButtonUp(0))//マウスのボタンが離された時trueを返す
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//リスタート
                }
                break;

            case GameState.Goal://ゲーム状態がゴールの場合
                anim.SetBool(Constants.AnimationStarted, false);//AnimationStartedっていうアニメーションをfalseに変更
                if (Input.GetMouseButtonUp(0))//マウスのボタンが離された時trueを返す
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//リスタート
                }
                break;
            default://デフォルト（それ以外の時全部）の場合
                break;
        }
    }

    private void Move() {
        CheckHeight();//高さチェック関数
        Jump();//ジャンプ関数
        Rotate();//回転関数
        Step();//横移動関数

        moveDirection.y -= gravity * Time.deltaTime;//重力を場に発生させる
        controller.Move(moveDirection * Time.deltaTime);//衝突しないなら動く
    }

    //走る速度変更
    public void SpeedChange(float speed){
        Speed = speed;
        moveDirection = Vector3.forward;
        moveDirection = transform.TransformDirection(moveDirection);//移動方向をローカル空間からワールド空間へ変換
        moveDirection *= Speed;
        Debug.Log("@@@speed: " + Speed);
    }

    //回転速度変更
    public void RotateSpeedChange()
    {
        RotateSpeed++;
        Debug.Log("@@@rotatespeed: " + RotateSpeed);
    }
 
 


    //Arduino通信用
    void OnDataReceived(string message)
    {
        var data = message.Split(
                new string[] { "\t" }, System.StringSplitOptions.None);
        if (data.Length < 2) return;

        try
        {
            sensorLeft = float.Parse(data[0]);
            sensorRight = float.Parse(data[1]);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    //関数定義
    //高さチェック関数
    private void CheckHeight()
    {
        //数字いずれ変えないと
        if(transform.position.y < -10)//キャラクターのy座標が-10を下回ると
        {
            GameManager.Instance.Die();//ゲームオーバー
        }
    }

    //ジャンプ関数
    private void Jump()
    {
        var inputDirection = inputDetector.DetectInputDirection();//inputDirectionにキー入力の方向を追加

        if(controller.isGrounded//キャラクターが地面に接地している場合
            && inputDirection.HasValue//何かしらのキー入力がある場合
            && inputDirection == InputDirection.Top//キー入力が上キーの場合
            )
        {
            moveDirection.y = JumpSpeed;//z軸進行方向をJumpSpeed値にする（そのうち重力に負ける）
            anim.SetBool(Constants.AnimationJump, true);//AnimationJumpっていうアニメーションをtrueに変更
        }
        else
        {
            anim.SetBool(Constants.AnimationJump, false);//AnimationJumpっていうアニメーションをfalseに変更
        }
    }


    private IEnumerator RotateCharacter(int rightOrLeft)
    {

        for (int count = 0; count <= 90; count++)
        {
            this.transform.Rotate(0, rightOrLeft, 0);

            if (count % RotateSpeed == 0)
            {
                yield return new WaitForSeconds(0.001f);
            }   
        }
        moveDirection = Vector3.forward;
        moveDirection = transform.TransformDirection(moveDirection);//移動方向をローカル空間からワールド空間へ変換
        moveDirection *= Speed;//単位ベクトルにスピード倍する
        isRotate = false;
        GameManager.Instance.CanRotate = false;

    }


    //回転関数
    private void Rotate()
    {
        var inputDirection = inputDetector.DetectInputDirection();//inputDirectionにキー入力の方向を追加

        //右回転
        if (GameManager.Instance.CanRotate
            && !isRotate//回転中でない
            && inputDirection.HasValue//何かしらのキー入力がある場合
            && inputDirection == InputDirection.Right//キー入力が右キーの場合
            )
        {
            isRotate = true;//回転中に変更
            StartCoroutine("RotateCharacter", 1);
            //rotateDirection = transform.TransformDirection(Vector3.right);
            //targetRotation = Quaternion.LookRotation(rotateDirection, Vector3.up);//最終的な向きを保存　Vector.upを中心軸にVector3.rightまで回転
        }
        //左回転
        else if (GameManager.Instance.CanRotate
            && !isRotate//回転中でない
            && inputDirection.HasValue//何かしらのキー入力がある場合
            && inputDirection == InputDirection.Left//キー入力が右キーの場合
            )
        {
            isRotate = true;//回転中に変更
            StartCoroutine("RotateCharacter", -1);
            //rotateDirection = transform.TransformDirection(Vector3.left);
            //targetRotation = Quaternion.LookRotation(rotateDirection, Vector3.up);//最終的な向きを保存
        }
         
        
    }


    //横移動関数
    private void Step()
    {
        var inputDirection = inputDetector.DetectInputDirection();//inputDirectionにキー入力の方向を追加

        //右移動
        if (GameManager.Instance.CanStep
            && controller.isGrounded
            && inputDirection.HasValue
            && inputDirection == InputDirection.Right
            //&& sensorRight < 4.0f && sensorLeft > 4.0f
            )
        {
            transform.position += transform.right * 3.0f * Time.deltaTime;
        }

        //左移動
        else if (GameManager.Instance.CanStep
            && controller.isGrounded
            && inputDirection.HasValue
            && inputDirection == InputDirection.Left
            //&& sensorLeft < 4.0f && sensorRight > 4.0f
            )
        {
            transform.position += transform.right * -3.0f * Time.deltaTime;
        }
    }
}
