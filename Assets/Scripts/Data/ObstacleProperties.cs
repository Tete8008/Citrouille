using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObstacleProperties", menuName = "Citrouille/ObstacleProperties", order = 1)]
public class ObstacleProperties : ScriptableObject
{
    public bool isBumper;
    public Mesh mesh;
    public Material material;
    [Header("if bumper only")]
    [Range(1,3)]
    public float bumperAccelerationMultiplier=1;
}
