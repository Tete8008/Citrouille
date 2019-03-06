using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBloc : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Collider meshCollider;
    public ParticleSystem blocDestructionSFX;
    private int currentHealth;
    public Transform self;

    private BlocProperties blocProperties;

    [System.NonSerialized] public TowerBehaviour tower;
    [System.NonSerialized] public int blocIndex;
    [System.NonSerialized] public bool invincible;


    public void Init(BlocProperties blocProperties,TowerBehaviour tower,int index)
    {
        meshFilter.mesh = blocProperties.mesh;
        meshRenderer.material = blocProperties.material;
        if (typeof(MeshCollider) == meshCollider.GetType())
        {
            ((MeshCollider)meshCollider).sharedMesh = meshFilter.sharedMesh;
        }
        currentHealth = tower.towerProperties.healthPerBloc;
        this.tower = tower;
        this.blocProperties = blocProperties;
        blocIndex = index;
    }

    private void TakeDamage(int value)
    {
        currentHealth -= value;
        int damageLeft = Mathf.Abs(currentHealth);
        if (damageLeft > 0)
        {
            tower.GetBlocAtIndex(blocIndex - 1).TakeDamage(damageLeft);
        }
        BallLauncher.instance.IncreaseDamageDone(value);
        if (currentHealth <= 0)
        {
            Debug.Log("Bloc destroyed");
            StartCoroutine(Die());
        }
    }


    private IEnumerator Die()
    {
        blocDestructionSFX.Play();
        meshRenderer.enabled = false;
        meshCollider.enabled = false;
        tower.ToggleBlocsInvincibility(true);
        yield return new WaitForSeconds(0.5f);
        tower.RemoveBloc(this);
        Destroy(gameObject); 
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball") && !invincible)
        {
            BallBehaviour ball = collision.collider.GetComponentInParent<BallBehaviour>();
            switch (blocProperties.blocEffect)
            {
                case BlocEffect.None:
                    //do nothing LUL
                    break;
                case BlocEffect.BallMultiplier:
                    SpawnBallWithRandomDirection();
                    break;
                case BlocEffect.PowerBall:
                    ball.overPowered = true;
                    break;
                case BlocEffect.IronBall:
                    ball.resistance = ball.ironResistance;
                    break;
            }
            
            int damage;
            if (ball.overPowered) {
                damage = ball.poweredDamage;
            }
            else
            {
                damage = ball.ballDamage;
            }

            //velocity correction
            ball.rigid.velocity = new Vector3(ball.rigid.velocity.x, 0, ball.rigid.velocity.z);

            TakeDamage(damage);
        }
    }

    private void SpawnBallWithRandomDirection()
    {
        BallBehaviour ball = Instantiate(BallLauncher.instance.ballPrefab).GetComponent<BallBehaviour>();
        float angle = Random.Range(0, 2 * Mathf.PI);
        ball.rigid.velocity = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))*BallLauncher.instance.ballVelocity;
        BallLauncher.instance.AddBall(ball);

    }

}
