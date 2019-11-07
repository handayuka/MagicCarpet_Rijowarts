using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == Constants.PlayerTag)

            GameManager.Instance.Die();
    }
}
