using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{

    private string SceneName;
    private void GoToTutorialObstacles() { SceneManager.LoadScene("tutorialObstacles"); }
    private void GoToTutorialGemGetContinuous() { SceneManager.LoadScene("tutorialGemGetContinuous"); }
    private void GoToRotatePathsLevel() { SceneManager.LoadScene("rotatePathsLevel"); }

    // Use this for initialization
    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == Constants.PlayerTag && GameManager.Instance.GameState == GameState.MainPlaying){
            UIManager.Instance.SetStatus(Constants.StatusGoal);
            GameManager.Instance.Goal();
            TimeManager.Instance.stopTimer();
        }

        if (hit.gameObject.tag == Constants.PlayerTag && GameManager.Instance.GameState == GameState.Playing)
        {
            Debug.Log("@@@hit");
            //TutorialManager.Instance.tutorialState = TutorialState.Goal;

            SceneName = SceneManager.GetActiveScene().name;
            if (SceneName == "tutorialGemGetSingle") { GoToTutorialObstacles(); }
            else if (SceneName == "tutorialObstacles") { GoToTutorialGemGetContinuous(); }
            else if (SceneName == "tutorialGemGetContinuous") { GoToRotatePathsLevel(); }
        }
    }
}
