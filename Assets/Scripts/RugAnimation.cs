using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RugAnimation : MonoBehaviour
{

    enum RugTilt {
        CENTER,
        RIGHT,
        LEFT
    }

    RugTilt rugTilt = RugTilt.CENTER;

    private int rag_z = 0;

    public IEnumerator leftTiltAnimation() 
    {
        rugTilt = RugTilt.LEFT;

        conversionZValeu();

        for (int count = rag_z; count <= 15; count++){
            this.transform.Rotate(0, -1, 1);
            yield return new WaitForSeconds(0.05f);

            if(rugTilt != RugTilt.LEFT)
            {
                yield break;
            }
        }
    }

    public IEnumerator rightTiltAnimation()
    {
        rugTilt = RugTilt.RIGHT;

        conversionZValeu();

        for (int count = rag_z; count >= -15; count--)
        {
            this.transform.Rotate(0, 1, -1);
            yield return new WaitForSeconds(0.05f);

            if (rugTilt != RugTilt.RIGHT)
            {
                yield break;
            }
        }
    }

    public IEnumerator setCenterAnimation()
    {
        checkTilt();
        conversionZValeu();

        if (rugTilt == RugTilt.RIGHT){ //右に傾いている

            rugTilt = RugTilt.CENTER;

            for (int count = rag_z; count <= 0; count++)
            {
                this.transform.Rotate(0, -1, 1);
                yield return new WaitForSeconds(0.05f);

                if (rugTilt != RugTilt.CENTER)
                {
                    yield break;
                }
            }

        }
        else if(rugTilt == RugTilt.LEFT){ //左に傾いている

            rugTilt = RugTilt.CENTER;

            for (int count = rag_z; count >= 0; count--)
            {
                this.transform.Rotate(0, 1, -1);
                yield return new WaitForSeconds(0.05f);

                if (rugTilt != RugTilt.CENTER)
                {
                    yield break;
                }
            }
        
        } 
    }

    private void conversionZValeu(){
        if (transform.eulerAngles.z > 180 && transform.eulerAngles.z < 360)
        {
            rag_z = (int)transform.eulerAngles.z - 360;
        }
        else if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            rag_z = (int)transform.eulerAngles.z;
        }
    }


    private void checkTilt(){
        if (transform.eulerAngles.z > 180 && transform.eulerAngles.z < 360)
        {
            rugTilt = RugTilt.RIGHT;
        }
        else if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            rugTilt = RugTilt.LEFT;
        }
    }
}
