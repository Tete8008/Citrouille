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
    private int bonusBalls;
    private int damageDone;

    private List<BallBehaviour> balls;

    public LineRenderer lineRenderer;
    

    [System.NonSerialized] public bool aiming;

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
        bonusBalls = 0;
        initialRotation = transform.eulerAngles;
        balls = new List<BallBehaviour>();
    }

    public void LockAndShoot()
    {
        if (instance.aiming)
        {
            aiming = false;
            ballsLeft = ballsPerSalve + bonusBalls;
            timeLeftBeforeNextShoot = 1 / fireRate;
        }
        
    }


    public void RemoveBall(BallBehaviour ball)
    {
        balls.Remove(ball);
        if (balls.Count <= 0)
        {
            aiming = true;
            bonusBalls = (int)(damageDone * bonusBallPerDamageDealt);
            damageDone = 0;
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
            if (ballsLeft > 0)
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
            }
        }
        else
        {
            if (bonusBalls > 0)
            {
                timeLeftBeforeNextBonusDecrease -= Time.deltaTime;
                if (timeLeftBeforeNextBonusDecrease <= 0)
                {
                    bonusBalls--;
                    timeLeftBeforeNextBonusDecrease += 1 / bonusDecreaseRate;
                }
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
                instance.lineRenderer.SetPosition(0, instance.transform.position + Vector3.up);

                instance.lineRenderer.SetPosition(1, hitInfo.point + Vector3.up);
                instance.lineRenderer.positionCount = 3;
                Vector3 reflectVec = Vector3.Reflect(hitInfo.point + instance.transform.forward * 100, hitInfo.normal);
                instance.lineRenderer.SetPosition(2, reflectVec + Vector3.up);
            }
            else
            {
                instance.lineRenderer.positionCount = 2;
                instance.lineRenderer.SetPosition(1, instance.transform.forward * 100 + Vector3.up);
            }

        }
    }


    public void IncreaseDamageDone(int dmg)
    {
        damageDone += dmg;
    }

    public void AddBall(BallBehaviour ball)
    {
        balls.Add(ball);
    }

}
