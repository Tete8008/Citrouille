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
    public float bonusDecreaseRate;

    public int ballsPerSalve;
    public float bonusBallPerDamageDealt;

    public Transform spawnPoint;

    private float timeLeftBeforeNextShoot;
    private float timeLeftBeforeNextBonusDecrease;

    private Vector3 initialRotation;
    private int ballsLeft;
    private float bonusBalls;
    private int damageDone;

    private List<BallBehaviour> balls;

    public LineRenderer lineRenderer;
    

    [System.NonSerialized] public bool aiming;
    [System.NonSerialized] public int currentSalveIndex;
    [System.NonSerialized] public bool outOfSalves;


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
        aiming = true;
        currentSalveIndex = 1;
        bonusBalls = 0;
        initialRotation = transform.eulerAngles;
        balls = new List<BallBehaviour>();
    }

    public void LockAndShoot()
    {
        if (aiming && outOfSalves==false)
        {
            aiming = false;
            ballsLeft = ballsPerSalve + (int)bonusBalls;
            bonusBalls = 0;
            timeLeftBeforeNextShoot = 1 / fireRate;
        }
    }


    public void RemoveBall(BallBehaviour ball)
    {
        balls.Remove(ball);
        if (balls.Count <= 0 && outOfSalves)
        {
            Debug.Log("YOU LOSE");
        }
    }

    private void ShootBall()
    {
        GameObject ballObject=Instantiate(instance.ballPrefab);
        ballObject.transform.position = instance.spawnPoint.position;
        BallBehaviour ball = ballObject.GetComponent<BallBehaviour>();
        ball.rigid.velocity = instance.transform.forward*instance.ballVelocity;
        balls.Add(ball);
        ballsLeft--;
    }


    private void Update()
    {
        if (!instance.aiming)
        {
            if (ballsLeft  > 0)
            {
                timeLeftBeforeNextShoot -= Time.deltaTime;
                if (timeLeftBeforeNextShoot <= 0)
                {
                    instance.ShootBall();
                    timeLeftBeforeNextShoot += (1 / fireRate);
                }
            }
            else
            {
                /*aiming = true;
                bonusBalls = (int)(damageDone * bonusBallPerDamageDealt);*/
                aiming = true;

                if (currentSalveIndex < 3)
                {
                    currentSalveIndex++;
                }
                else
                {
                    outOfSalves = true;
                }
            }
        }
        else
        {
            if (bonusBalls > 0)
            {
                bonusBalls -= Time.deltaTime * bonusDecreaseRate;
            }
            else
            {
                bonusBalls = 0;
            }
            
        }
    }

    public static void Rotate(Vector2 mousePosition)
    {
        if (instance.aiming)
        {
            Ray ray = MapManager.instance.cam.ScreenPointToRay(mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Ground")))
            {
                Vector3 direction = (hit.point - instance.transform.position).normalized;
                Vector3 rotation = Vector3.RotateTowards(instance.transform.forward, direction, 2 * Mathf.PI, 0f);
                instance.transform.rotation = Quaternion.LookRotation(new Vector3(rotation.x,0,rotation.z));   
            }

            //instance.transform.eulerAngles= new Vector3(instance.initialRotation.x, instance.initialRotation.y, instance.initialRotation.z);

            if (instance.transform.eulerAngles.y > instance.initialRotation.y + instance.maxAngle / 2 && instance.transform.eulerAngles.y < 180)
            {
                instance.transform.eulerAngles = new Vector3(instance.initialRotation.x, instance.initialRotation.y + instance.maxAngle / 2, instance.initialRotation.z);
            }
            if (instance.transform.eulerAngles.y > 180 && instance.transform.eulerAngles.y < 360 + instance.initialRotation.y - instance.maxAngle / 2)
            {
                instance.transform.eulerAngles = new Vector3(instance.initialRotation.x, instance.initialRotation.y - instance.maxAngle / 2, instance.initialRotation.z);
            }

            if (Physics.Raycast(instance.transform.position, instance.transform.forward, out RaycastHit hitInfo, 100, LayerMask.GetMask("Border")))
            {
                instance.lineRenderer.SetPosition(0, instance.transform.position );

                instance.lineRenderer.SetPosition(1, hitInfo.point );
                instance.lineRenderer.positionCount = 3;
                Vector3 reflectVec = Vector3.Reflect(hitInfo.point + instance.transform.forward * 100, hitInfo.normal);
                instance.lineRenderer.SetPosition(2, reflectVec);
            }
            else
            {
                instance.lineRenderer.positionCount = 2;
                instance.lineRenderer.SetPosition(1, instance.transform.position+instance.transform.forward * 100 );
            }
        }
    }


    public void IncreaseDamageDone(int dmg)
    {
        bonusBalls += dmg * bonusBallPerDamageDealt;
    }

    public void AddBall(BallBehaviour ball)
    {
        balls.Add(ball);
    }

}
