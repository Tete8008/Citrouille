using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TowerBehaviour))]
public class TowerInspector : Editor 
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Init"))
        {
            TowerBehaviour tower = target as TowerBehaviour;
            if (tower.towerProperties != null)
            {
                tower.Init(tower.towerProperties);
            }
            else
            {
                Debug.Log("tower properties not referenced");
            }
        }
    }
}
