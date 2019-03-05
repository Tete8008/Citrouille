using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewMapProperties",menuName ="Citrouille/MapProperties",order =1)]
public class MapProperties : ScriptableObject
{
    public List<TowerData> towers;
    public Vector3 cameraPosition;
    public Quaternion cameraRotation;
    public float levelWidth;
    public float levelDepth;
}
