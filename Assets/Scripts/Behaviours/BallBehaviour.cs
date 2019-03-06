﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public Rigidbody rigid;
    public int ballDamage=1;
    public int resistance = 3;
    public float timeToLive = 10f;
    public int poweredDamage;
    public int ironResistance;

    private float timeLeftToLive;

    public bool overPowered=false;

    private void Start()
    {
        timeLeftToLive = timeToLive;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Border") || collision.collider.CompareTag("TowerBloc") || collision.collider.CompareTag("Obstacle"))
        {
            resistance--;
            if (resistance <= 0)
            {
                Die();
            }
        }
    }

    private void Update()
    {
        timeLeftToLive -= Time.deltaTime;
        if (timeLeftToLive <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        BallLauncher.instance.RemoveBall(this);
        Destroy(gameObject);
    }

}
