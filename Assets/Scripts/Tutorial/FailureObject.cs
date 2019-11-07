using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailureObject : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        //if the player hits one obstacle, it's game over
        if (col.gameObject.tag == Constants.PlayerTag)
        {
            Debug.Log("@@@hit" + TurnCorner.Instance.hitFailureObject);
            TurnCorner.Instance.hitFailureObject = true;
        }
    }
}
