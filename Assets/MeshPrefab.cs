using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPrefab : MonoBehaviour
{
    [HideInInspector] public TowerBloc towerBloc;


    private void OnCollisionEnter(Collision collision)
    {
        towerBloc.CollisionEnter(collision);
    }
}
