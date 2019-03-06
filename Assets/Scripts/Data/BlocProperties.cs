using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlocEffect { None,BallMultiplier,PowerBall,IronBall}

[CreateAssetMenu(fileName = "NewBlocProperties", menuName = "Citrouille/BlocProperties", order = 1)]
public class BlocProperties : ScriptableObject
{
    public BlocEffect blocEffect;
    public GameObject meshPrefab;

}
