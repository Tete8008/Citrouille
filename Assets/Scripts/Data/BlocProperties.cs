using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBlocProperties", menuName = "Citrouille/BlocProperties", order = 1)]
public class BlocProperties : ScriptableObject
{
    public Mesh mesh;
    public Material material;
}
