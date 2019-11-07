using UnityEngine;
using System.Collections;

public class ObstacleCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == Constants.PlayerTag)

        UIManager.Instance.DecreaseLife(LifePoints);
        Destroy(this.gameObject);
    }
    public int LifePoints = 1;
}
