using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;
    private Vector2 moyPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                moyPosition += Input.GetTouch(i).position;
            }
            moyPosition /= Input.touchCount;
            print(moyPosition);
        }*/

        if (Input.GetMouseButton(0))
        {
            BallLauncher.Rotate(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            BallLauncher.instance.LockAndShoot();
        }
        
    }
}
