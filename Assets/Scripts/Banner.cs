using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banner : MonoBehaviour
{

    public float interval = 0.3f;

    void Start()
    {
        StartCoroutine("Blink");
    }

    IEnumerator Blink()
    {
        while (true)
        {
            var renderComponent = GetComponent<Renderer>();
            renderComponent.enabled = !renderComponent.enabled;
            yield return new WaitForSeconds(interval);
        }
    }

}