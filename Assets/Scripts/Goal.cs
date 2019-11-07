using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{

    // Use this for initialization
    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == Constants.PlayerTag && GameManager.Instance.GameState == GameState.MainPlaying){
            UIManager.Instance.SetStatus(Constants.StatusGoal);
            GameManager.Instance.Goal();
            TimeManager.Instance.stopTimer();
        }

        if (hit.gameObject.tag == Constants.PlayerTag && GameManager.Instance.GameState == GameState.Playing){
            Debug.Log("@@@hit");
            TutorialManager.Instance.tutorialState = TutorialState.Goal;
        }
           

    }

}
