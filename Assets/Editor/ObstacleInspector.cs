using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Obstacle))]
public class ObstacleInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Init"))
        {
            Obstacle obstacle = target as Obstacle;
            if (obstacle.obstacleProperties != null)
            {
                obstacle.Init(obstacle.obstacleProperties);
            }
            else
            {
                Debug.Log("obstacle properties not referenced");
            }
        }
    }

}
