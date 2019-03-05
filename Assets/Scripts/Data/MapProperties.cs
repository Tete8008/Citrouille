using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewMapProperties",menuName ="Citrouille/MapProperties",order =1)]
public class MapProperties : ScriptableObject
{

    public List<TowerData> towers;
    public Vector3 cameraPosition;
    public Quaternion cameraRotation;
    public Vector3 launcherPosition;
    public Quaternion launcherRotation;
    public Vector3 groundPosition;
    public Quaternion groundRotation;
    public List<Vector3> borderPositions;
    public List<Quaternion> borderRotations;
    public float levelWidth;
    public float levelDepth;
}
