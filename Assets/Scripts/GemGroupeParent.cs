using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GemGroupeParent : MonoBehaviour
{

    [SerializeField] NewRotate player = null;
    [SerializeField] float speedDiff = 3;

    void Start() {
        this.ObserveEveryValueChanged(x => this.transform.childCount)
            .Skip(1) //最初の一回の値の変動をスキップする
            .Subscribe( _ => checkAllGemGet() );
    }

    void checkAllGemGet() {
        //Groupe内のダイヤを全部取った時＝連続ゲット？
        if (this.transform.childCount == 0){
            player.SpeedChange(player.Speed + this.speedDiff);
            player.RotateSpeedChange();

        }
    }

}
