using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapGenerator : EditorWindow
{
    private MapProperties mapProperties;

    private GameObject map;

    private TowerProperties selectedTowerProperties;

    private bool hideMap=false;

    private GameObject towerPrefab;

    [MenuItem("Citrouille/MapGenerator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MapGenerator window = (MapGenerator)EditorWindow.GetWindow(typeof(MapGenerator));
        window.Show();
    }



    private void OnGUI()
    {
        if (map == null)
        {
            map = new GameObject();
            map.name = "Map";
        }
        

        if (GUILayout.Button("New Map"))
        {
            MapProperties newMapProperties = ScriptableObject.CreateInstance<MapProperties>();
            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/ScriptableObjects/Maps/NewMapProperties.asset");
            AssetDatabase.CreateAsset(newMapProperties, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            newMapProperties.towers = new List<TowerBehaviour>();
            
            Selection.activeObject = newMapProperties;
            mapProperties = newMapProperties;
        }

        EditorGUI.BeginChangeCheck();
        mapProperties = EditorGUILayout.ObjectField("Map file", mapProperties, typeof(MapProperties), false) as MapProperties;
        if (EditorGUI.EndChangeCheck())
        {
            
            RefreshMap();
        }

        towerPrefab=EditorGUILayout.ObjectField("TowerPrefab", towerPrefab, typeof(GameObject), false) as GameObject;

        EditorGUILayout.BeginHorizontal();

        selectedTowerProperties = EditorGUILayout.ObjectField(selectedTowerProperties,typeof(TowerProperties),false) as TowerProperties;

        if (GUILayout.Button("Add tower (CTRL+W)"))
        {
            GameObject towerObject = Instantiate(towerPrefab);
            towerObject.transform.SetParent(map.transform);
            towerObject.name = "Tower" + mapProperties.towers.Count;
            TowerBehaviour towerBehaviour = towerObject.GetComponent<TowerBehaviour>();
            towerBehaviour.Init(selectedTowerProperties);
            if (mapProperties.towers.Count > 0)
            {
                towerBehaviour.self.localPosition = mapProperties.towers[mapProperties.towers.Count - 1].self.localPosition;
            }
            
            mapProperties.towers.Add(towerBehaviour);
        }

        EditorGUILayout.EndHorizontal();




    }

    private void RefreshMap()
    {
        DestroyImmediate(map);
        map = new GameObject();
        map.name = "Map";
        for (int i = 0; i < mapProperties.towers.Count; i++)
        {
            GameObject towerObject = Instantiate(towerPrefab);
            towerObject.transform.SetParent(map.transform);
            towerObject.name = "Tower" + i;
            TowerBehaviour towerBehaviour = towerObject.GetComponent<TowerBehaviour>();
            towerBehaviour.Init(mapProperties.towers[i].towerProperties);
            towerBehaviour.self.localPosition = mapProperties.towers[i].self.localPosition;
        }
    }


}
