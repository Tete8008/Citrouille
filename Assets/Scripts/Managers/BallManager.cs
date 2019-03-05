using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager instance = null;

    public float fireRate = 1;
    public float ballVelocity = 1;
    public float maxAngle;

    public GameObject ballLauncherPrefab;


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


    public static void SpawnLauncher()
    {
        
    }


    

    

}
