using UnityEngine;
using System.Collections;

//多分曲がり角となる赤い道のとこにこのスワイプコライダーが毎度立ってて
//ここに入ればCanRotate=true
//出ればCanRotate=false
//ってしたかったんだと思う
public class SwipeColliderThreeArrows : MonoBehaviour
{

    // Use this for initialization
    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == Constants.PlayerTag)
        {
            GameManager.Instance.CanRotate = true;
            GameManager.Instance.CanStep = false;
            UIManager.Instance.SetCanvasGroupEnable(UIManager.Instance.canvasgroup[3], true);
            
        }

    }

    void OnTriggerExit(Collider hit)
    {
        if (hit.gameObject.tag == Constants.PlayerTag)
        {
            GameManager.Instance.CanRotate = false;
            GameManager.Instance.CanStep = true;
            UIManager.Instance.SetCanvasGroupEnable(UIManager.Instance.canvasgroup[3], false);
            
        }

    }
}
