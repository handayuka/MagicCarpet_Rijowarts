using Assets.Scripts;
using UnityEngine;

public class ArrowKeysDetector : MonoBehaviour, IInputDetector
{
    public InputDirection? DetectInputDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            return InputDirection.Top;
        else if (Input.GetKey(KeyCode.DownArrow))
            return InputDirection.Bottom;
        else if (Input.GetKey(KeyCode.RightArrow))
            return InputDirection.Right;
        else if (Input.GetKey(KeyCode.LeftArrow))
            return InputDirection.Left;
        else
            return null;
    }
}