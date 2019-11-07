using UnityEngine;
using System.Collections;

public class GemCollider : MonoBehaviour
{
    
    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == Constants.PlayerTag)

        UIManager.Instance.IncreaseScore(ScorePoints);
        AudioManager.Instance.PlaySE("GemGetSE");
        Destroy(this.gameObject);
    }
    public int ScorePoints = 100;
}
