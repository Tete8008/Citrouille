using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public static BallLauncher instance = null;

    public float fireRate;
    public float ballVelocity;
    public GameObject ballPrefab;
    public float maxAngle;


    public Transform spawnPoint;

    private float timeLeftBeforeNextShoot;

    private Vector3 initialRotation;

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

    private void Start()
    {
        timeLeftBeforeNextShoot = 1 / fireRate;
        initialRotation = transform.eulerAngles;
        print("initial rotation : " + initialRotation);
    }




    private void ShootBall()
    {
        GameObject ballObject=Instantiate(instance.ballPrefab);
        ballObject.transform.position = instance.spawnPoint.position;
        ballObject.GetComponent<BallBehaviour>().rigid.velocity = instance.transform.forward*instance.ballVelocity;

    }



    private void Update()
    {
        timeLeftBeforeNextShoot -= Time.deltaTime;
        if (timeLeftBeforeNextShoot <= 0)
        {
            instance.ShootBall();
            timeLeftBeforeNextShoot+= (1 / fireRate);
        }
    }

    public static void Rotate(Vector2 mousePosition)
    {
        Ray ray = MapManager.instance.cam.ScreenPointToRay(mousePosition);
        
        
        if (Physics.Raycast(ray, out RaycastHit hit,99999,LayerMask.GetMask("Ground")))
        {
            Vector3 direction = (hit.point- instance.transform.position).normalized ;
            Vector3 rotation = Vector3.RotateTowards(instance.transform.forward, direction, 2 * Mathf.PI, 0f);
            instance.transform.rotation = Quaternion.LookRotation(rotation);
        }

        //instance.transform.eulerAngles= new Vector3(instance.initialRotation.x, instance.initialRotation.y, instance.initialRotation.z);
        
        if (instance.transform.eulerAngles.y>instance.initialRotation.y + instance.maxAngle / 2 && instance.transform.eulerAngles.y<180)
        {
            instance.transform.eulerAngles = new Vector3(instance.initialRotation.x, instance.initialRotation.y + instance.maxAngle / 2, instance.initialRotation.z);
        }
        if (instance.transform.eulerAngles.y >180 && instance.transform.eulerAngles.y<360+instance.initialRotation.y-instance.maxAngle/2 )
        {
            instance.transform.eulerAngles = new Vector3(instance.initialRotation.x, instance.initialRotation.y - instance.maxAngle / 2, instance.initialRotation.z);
        }
        print(instance.transform.eulerAngles.y);
    }


}
