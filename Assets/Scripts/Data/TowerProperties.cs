using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerStructure { Straight,Spiral,Random}

[CreateAssetMenu(fileName ="NewTowerProperties",menuName ="Citrouille/TowerProperties",order =1)]
public class TowerProperties : ScriptableObject
{
    public int blocCount;
    public int healthPerBloc;
    public TowerStructure towerStructure;
    public List<BlocProperties> blocPattern;
}
