using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBloc : MonoBehaviour
{
    public ParticleSystem blocDestructionSFX;
    private int currentHealth;
    public Transform self;
    public int ballMultiplierCount;

    private BlocProperties blocProperties;

    [System.NonSerialized] public TowerBehaviour tower;
    [System.NonSerialized] public int blocIndex;
    [System.NonSerialized] public bool invincible;
    [System.NonSerialized] public float blocHeight;
    [System.NonSerialized] public GameObject mesh;


    public void Init(BlocProperties blocProperties,TowerBehaviour tower,int index,float height)
    {
        mesh=Instantiate(blocProperties.meshPrefab, self);
        mesh.transform.localPosition = Vector3.zero;
        mesh.GetComponent<MeshPrefab>().towerBloc = this;
        currentHealth = tower.towerProperties.healthPerBloc;
        this.tower = tower;
        this.blocProperties = blocProperties;
        blocIndex = index;
        blocHeight = height;
    }

    private void TakeDamage(int value)
    {
        //print("damage : " + value);
        currentHealth -= value;
        int damageLeft = Mathf.Abs(currentHealth);
        if (damageLeft > 0)
        {
            if (tower.GetBlocCount()>blocIndex+1)
            {
                tower.GetBlocAtIndex(blocIndex + 1).TakeDamage(damageLeft);
            }
        }
        BallLauncher.instance.IncreaseDamageDone(value);
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }


    private IEnumerator Die()
    {
        blocDestructionSFX.Play();
        Destroy(mesh);
        tower.ToggleBlocsInvincibility(true);
        yield return new WaitForSeconds(0.2f);
        tower.RemoveBloc(this);
        Destroy(gameObject); 
    }


    public void CollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball") && !invincible)
        {
            BallBehaviour ball = collision.collider.GetComponentInParent<BallBehaviour>();

            int damage;
            if (ball.overPowered)
            {
                damage = ball.poweredDamage;
            }
            else
            {
                damage = ball.ballDamage;
            }

            //velocity correction
            ball.rigid.velocity = new Vector3(ball.rigid.velocity.x, 0, ball.rigid.velocity.z);

            TakeDamage(damage);

            switch (blocProperties.blocEffect)
            {
                case BlocEffect.None:
                    //do nothing LUL
                    break;
                case BlocEffect.BallMultiplier:
                    SpawnBallWithRandomDirection(collision.contacts[0].point);
                    break;
                case BlocEffect.PowerBall:
                    ball.overPowered = true;
                    break;
                case BlocEffect.IronBall:
                    ball.resistance = ball.ironResistance;
                    break;
            }
        }
    }

    private void SpawnBallWithRandomDirection(Vector3 pos)
    {
        
        for (int i = 0; i < ballMultiplierCount; i++)
        {
            BallBehaviour ball = Instantiate(BallLauncher.instance.ballPrefab).GetComponent<BallBehaviour>();
            ball.transform.position = pos;
            float angle = Random.Range(0, 2 * Mathf.PI);
            ball.rigid.velocity = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * BallLauncher.instance.ballVelocity;
            BallLauncher.instance.AddBall(ball);
        }
        

    }

}
