using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [System.NonSerialized] public bool isBumper;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public Collider meshCollider;
    public Transform self;
    public ObstacleProperties obstacleProperties;

    public void Init(ObstacleProperties obstacleProperties)
    {
        this.obstacleProperties = obstacleProperties;
        meshFilter.mesh = obstacleProperties.mesh;

        if (typeof(MeshCollider) == meshCollider.GetType())
        {
            ((MeshCollider)meshCollider).sharedMesh = meshFilter.sharedMesh;
        }
        meshRenderer.material = obstacleProperties.material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            if (obstacleProperties.isBumper)
            {
                BallBehaviour ball = collision.collider.GetComponentInParent<BallBehaviour>();
                //velocity correction
                ball.rigid.velocity = new Vector3(ball.rigid.velocity.x,0,ball.rigid.velocity.z)* obstacleProperties.bumperAccelerationMultiplier;
            }
        }
        
    }


}
