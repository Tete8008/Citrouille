using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapGenerator : EditorWindow
{
    private MapProperties mapProperties;

    private GameObject map;

    private TowerProperties selectedTowerProperties;

    private GameObject towerPrefab;

    private List<TowerBehaviour> towerPreviews;
    private int previewsCount;

    private RenderTexture renderTexture;
    private Camera camera;
    private Vector2 scrollPos;

    [MenuItem("Citrouille/MapGenerator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MapGenerator window = (MapGenerator)EditorWindow.GetWindow(typeof(MapGenerator));
        window.Show();
    }


    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        DestroyImmediate(map);
        DestroyImmediate(camera.gameObject);
    }


    private void OnGUI()
    {
        scrollPos=EditorGUILayout.BeginScrollView(scrollPos);
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(500, 500, 500);
        }

        if (map == null)
        {
            map = new GameObject();
            map.name = "Map";
            map.transform.position = Vector3.zero;
        }

        if (camera == null)
        {
            GameObject cameraObject = new GameObject();
            cameraObject.AddComponent<Camera>();
            camera = cameraObject.GetComponent<Camera>();
            camera.targetTexture = renderTexture;
            camera.name = "CAMERA";
        }

        

        if (towerPreviews==null || towerPreviews.Count != map.transform.childCount)
        {
            towerPreviews = new List<TowerBehaviour>();
            for (int i = 0; i < map.transform.childCount; i++)
            {
                towerPreviews.Add(map.transform.GetChild(i).GetComponent<TowerBehaviour>());
            }
        }

        for (int i = 0; i < towerPreviews.Count; i++)
        {
            if (towerPreviews[i] == null)
            {
                towerPreviews.RemoveAt(i);
                i--;
            }
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("New Map"))
        {
            MapProperties newMapProperties = ScriptableObject.CreateInstance<MapProperties>();
            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/ScriptableObjects/Maps/NewMapProperties.asset");
            AssetDatabase.CreateAsset(newMapProperties, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            newMapProperties.towers = new List<TowerData>();
            
            Selection.activeObject = newMapProperties;
            mapProperties = newMapProperties;
        }


        if (mapProperties!=null && GUILayout.Button("Delete this map ↓",EditorStyles.miniButton))
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(mapProperties));
            AssetDatabase.Refresh();
            RefreshMap();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        mapProperties = EditorGUILayout.ObjectField("Map file", mapProperties, typeof(MapProperties), false) as MapProperties;
        if (EditorGUI.EndChangeCheck())
        {   
            RefreshMap();
        }

        towerPrefab=EditorGUILayout.ObjectField("TowerPrefab", towerPrefab, typeof(GameObject), false) as GameObject;

        EditorGUILayout.BeginHorizontal();

        selectedTowerProperties = EditorGUILayout.ObjectField(selectedTowerProperties,typeof(TowerProperties),false) as TowerProperties;

        if (GUILayout.Button("Add tower of this ← type ") && selectedTowerProperties!=null)
        {
            GameObject towerObject = Instantiate(towerPrefab);
            towerObject.transform.SetParent(map.transform);
            towerObject.name = "Tower" + towerPreviews.Count;
            TowerBehaviour towerBehaviour = towerObject.GetComponent<TowerBehaviour>();
            towerBehaviour.Init(selectedTowerProperties);
            
            TowerData towerData = new TowerData();
            towerData.towerProperties = towerBehaviour.towerProperties;
            if (towerPreviews.Count > 0)
            {
                Debug.Log(towerPreviews[towerPreviews.Count - 1]);
                towerBehaviour.self.position = towerPreviews[towerPreviews.Count-1].self.position;
                towerBehaviour.self.rotation = towerPreviews[towerPreviews.Count - 1].self.rotation;
            }
            towerData.position = towerBehaviour.self.position;
            towerData.rotation = towerBehaviour.self.rotation;
            mapProperties.towers.Add(towerData);
            towerPreviews.Add(towerBehaviour);
            previewsCount++;
            Selection.activeObject = towerBehaviour;
        }


        if (Selection.activeGameObject!=null)
        {
            TowerBehaviour tower = (Selection.activeGameObject).GetComponent<TowerBehaviour>();
            if (tower!=null && GUILayout.Button("Remove selected tower", EditorStyles.miniButton))
            {
                towerPreviews.Remove(tower);
                DestroyImmediate(Selection.activeObject);
            }
        }

        EditorGUILayout.EndHorizontal();


        //ça c'est pog ↓
        GUILayout.Box(new GUIContent(renderTexture),new GUIStyle { alignment=TextAnchor.MiddleCenter});



        if (GUILayout.Button("Save current map"))
        {
            if (map == null)
            {
                Debug.LogWarning("Map not instantiated");
                return;
            }

            if (mapProperties == null)
            {
                Debug.LogWarning("Map properties not referenced");
                return;
            }

            mapProperties.towers = new List<TowerData>();
            for (int i = 0; i < towerPreviews.Count; i++)
            {
                mapProperties.towers.Add(new TowerData
                {
                    position=towerPreviews[i].self.position,
                    rotation= towerPreviews[i].self.rotation,
                    towerProperties= towerPreviews[i].towerProperties
                });
            }
            mapProperties.cameraPosition = camera.transform.position;
            mapProperties.cameraRotation = camera.transform.rotation;
            //todo mapProperties.levelWidth et mapProperties.levelDepth
            
        }
        if (mapProperties != null)
        {
            EditorUtility.SetDirty(mapProperties);
        }

        EditorGUILayout.EndScrollView();
        


    }

    private void RefreshMap()
    {
        DestroyImmediate(map);
        map = new GameObject();
        map.name = "Map";
        DestroyImmediate(camera);
        GameObject cameraObject = new GameObject();
        cameraObject.AddComponent<Camera>();
        camera = cameraObject.GetComponent<Camera>();
        camera.targetTexture = renderTexture;
        camera.name = "CAMERA";
        towerPreviews = new List<TowerBehaviour>();
        if (mapProperties != null)
        {
            for (int i = 0; i < mapProperties.towers.Count; i++)
            {
                GameObject towerObject = Instantiate(towerPrefab);
                towerObject.transform.SetParent(map.transform);
                towerObject.name = "Tower" + i;
                TowerBehaviour towerBehaviour = towerObject.GetComponent<TowerBehaviour>();
                towerBehaviour.Init(mapProperties.towers[i].towerProperties);
                towerBehaviour.self.position = mapProperties.towers[i].position;
                towerPreviews.Add(towerBehaviour);
            }
        }
        
    }


}
