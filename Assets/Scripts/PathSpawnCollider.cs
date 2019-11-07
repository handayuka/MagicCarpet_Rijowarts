using UnityEngine;
using System.Collections;

public class PathSpawnCollider : MonoBehaviour {

    public float positionY = 0.81f;
    public Transform[] PathSpawnPoints;//座標、回転、スケールが入ってる
    public GameObject Path;
    public GameObject DangerousBorder;

    void OnTriggerEnter(Collider hit)
    {
        //player has hit the collider
        if (hit.gameObject.tag == Constants.PlayerTag)
        {
            //find whether the next path will be straight, left or right
            int randomSpawnPoint = Random.Range(0, PathSpawnPoints.Length);//.Lengthは配列の要素数　0~PathSpawnPointsの要素数のうちランダムで数を１つ取ってくる
            for (int i = 0; i < PathSpawnPoints.Length; i++)//PathSpawnPointsの要素数分for文を回す
            {
                //instantiate the path, on the set rotation
                if (i == randomSpawnPoint)//ランダムで選んだ１つに該当した場合
                    Instantiate(Path, PathSpawnPoints[i].position, PathSpawnPoints[i].rotation);//Instantiateは新しい位置・回転でクローンを生成する
                else
                {
                    //instantiate the border, but rotate it 90 degrees first
                    //右に90度回転さしたデンジャラスボーダーを生成
                    Vector3 rotation = PathSpawnPoints[i].rotation.eulerAngles;
                    rotation.y += 90;//右に90度回転
                    Vector3 position = PathSpawnPoints[i].position;
                    position.y += positionY;//y座標で0.81f上方向に設定（なんでy座標？？？）
                    Instantiate(DangerousBorder, position, Quaternion.Euler(rotation));//デンジャラスボーダー生成
                }
            }
            
        }
    }

}
