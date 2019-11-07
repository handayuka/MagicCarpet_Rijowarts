using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RugManager : MonoBehaviour
{

    [SerializeField] RugAnimation rug = null;
   
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(rug.rightTiltAnimation());
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(rug.leftTiltAnimation());
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            StartCoroutine(rug.setCenterAnimation());
        }
    }
}
