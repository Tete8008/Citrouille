using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewTowerProperties",menuName ="Citrouille/TowerProperties",order =1)]
public class TowerProperties : ScriptableObject
{
    public Mesh mesh;
    public Material material;
    public int health;
}
