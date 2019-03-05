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
    [System.NonSerialized] public TowerBehaviour tower;
    [System.NonSerialized] public int blocIndex;


    public void Init(BlocProperties blocProperties,TowerBehaviour tower,int index)
    {
        meshFilter.mesh = blocProperties.mesh;
        meshRenderer.material = blocProperties.material;
        if (typeof(MeshCollider) == meshCollider.GetType())
        {
            ((MeshCollider)meshCollider).sharedMesh = meshFilter.mesh;
        }
        currentHealth = tower.towerProperties.healthPerBloc;
        this.tower = tower;
        blocIndex = index;
    }

    private void TakeDamage(int value)
    {
        currentHealth -= value;
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
        yield return new WaitForSeconds(0.5f);
        tower.RemoveBloc(this);
        Destroy(gameObject); 
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            TakeDamage(collision.collider.GetComponentInParent<BallBehaviour>().ballDamage);
        }
    }

}
