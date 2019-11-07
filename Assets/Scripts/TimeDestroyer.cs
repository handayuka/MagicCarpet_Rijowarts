using UnityEngine;
using System.Collections;

//生成されて10秒後にはみんな死ぬ世界線
public class TimeDestroyer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Invoke("DestroyObject", LifeTime);//LifeTimeおきにDestroyObject関数呼び出し
    }


    void DestroyObject()
    {
        if (GameManager.Instance.GameState != GameState.Dead)
            Destroy(gameObject);
    }


    public float LifeTime = 10f;
}
