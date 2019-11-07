using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{

    // Use this for initialization
    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == Constants.PlayerTag)
        UIManager.Instance.SetStatus(Constants.StatusGoal);
        GameManager.Instance.Goal();
    }

}
