using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    [System.NonSerialized] public TowerProperties towerProperties;
    public Transform self;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public void Init(TowerProperties towerProperties)
    {
        print("init");
        this.towerProperties = towerProperties;
        self = transform;
        meshFilter.mesh = towerProperties.mesh;
        meshRenderer.material = towerProperties.material;
    }

<<<<<<< HEAD
=======

>>>>>>> master
}
