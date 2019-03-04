using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewMapProperties",menuName ="Citrouille/MapProperties",order =1)]
public class MapProperties : ScriptableObject
{
    public List<TowerBehaviour> towers;
    public Transform cameraTransform;
    public float levelWidth;
    public float levelDepth;
}
