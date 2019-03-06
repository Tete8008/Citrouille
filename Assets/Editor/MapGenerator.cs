using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapGenerator : EditorWindow
{
    private MapProperties mapProperties;

    private GameObject towersParent;
    private GameObject obstaclesParent;

    private TowerProperties selectedTowerProperties;
    private ObstacleProperties selectedObstacleProperties;

    private GameObject towerPrefab;
    private GameObject launcherPrefab;

    private List<TowerBehaviour> towerPreviews;
    private List<Obstacle> obstaclePreviews;
    private int towerPreviewsCount;
    private int obstaclePreviewsCount;

    private RenderTexture renderTexture;
    private Camera camera;
    private Vector2 scrollPos;

    private GameObject ballLauncher;
    private GameObject groundPrefab;
    private GameObject ground;

    private GameObject rightBorder;
    private GameObject leftBorder;
    private GameObject topBorder;
    private GameObject bottomBorder;

    private GameObject borderPrefab;

    private GameObject obstaclePrefab;

    [MenuItem("Citrouille/MapGenerator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MapGenerator window = (MapGenerator)EditorWindow.GetWindow(typeof(MapGenerator));
        window.Show();
    }


    private void OnEnable()
    {
        launcherPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/BallLauncher.prefab",typeof(GameObject))as GameObject;
        towerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/TowerPrefab.prefab", typeof(GameObject)) as GameObject;
        groundPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GroundPrefab.prefab", typeof(GameObject)) as GameObject;
        borderPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Border.prefab", typeof(GameObject)) as GameObject;
        obstaclePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Obstacle.prefab", typeof(GameObject)) as GameObject;

        if (launcherPrefab == null) { Debug.LogWarning("Launcher prefab not found"); }
        if (towerPrefab == null) { Debug.LogWarning("Tower prefab not found"); }
        if (groundPrefab == null) { Debug.LogWarning("Ground prefab not found"); }
        if (borderPrefab == null) { Debug.LogWarning("Border prefab not found"); }
        if (obstaclePrefab == null) { Debug.LogWarning("Obstacle prefab not found"); }
    }

    

    private void OnDisable()
    {
        DestroyImmediate(towersParent);
        DestroyImmediate(camera.gameObject);
        DestroyImmediate(ballLauncher.gameObject);
        DestroyImmediate(ground);
        DestroyImmediate(rightBorder);
        DestroyImmediate(leftBorder);
        DestroyImmediate(topBorder);
        DestroyImmediate(bottomBorder);
        DestroyImmediate(obstaclesParent);
        
    }


    private void OnGUI()
    {
        scrollPos=EditorGUILayout.BeginScrollView(scrollPos);

        if (rightBorder == null)
        {
            rightBorder = Instantiate(borderPrefab);
            rightBorder.name = "[Tool]RightBorder";
            rightBorder.transform.position = Vector3.zero;
        }

        if (leftBorder == null)
        {
            leftBorder = Instantiate(borderPrefab);
            leftBorder.name = "[Tool]LeftBorder";
            leftBorder.transform.position = Vector3.zero;
        }

        if (topBorder == null)
        {
            topBorder = Instantiate(borderPrefab);
            topBorder.name = "[Tool]TopBorder";
            topBorder.transform.position = Vector3.zero;
        }

        if (bottomBorder == null)
        {
            bottomBorder = Instantiate(borderPrefab);
            bottomBorder.name = "[Tool]BottomBorder";
            bottomBorder.transform.position = Vector3.zero;
        }

        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(500, 500, 500);
        }

        if (ground == null)
        {
            ground = Instantiate(groundPrefab);
            ground.transform.position = Vector3.zero;
            ground.name = "[Tool]Ground";
        }

        if (ballLauncher == null)
        {
            ballLauncher = Instantiate(launcherPrefab);
            ballLauncher.transform.position = Vector3.zero;
            ballLauncher.name = "[Tool]BallLauncher";
        }

        if (towersParent == null)
        {
            towersParent = new GameObject();
            towersParent.name = "[Tool]Towers";
            towersParent.transform.position = Vector3.zero;
        }

        if (obstaclesParent == null)
        {
            obstaclesParent = new GameObject();
            obstaclesParent.name = "[Tool]Obstacles";
            obstaclesParent.transform.position = Vector3.zero;
        }

        if (camera == null)
        {
            GameObject cameraObject = new GameObject();
            cameraObject.AddComponent<Camera>();
            camera = cameraObject.GetComponent<Camera>();
            camera.targetTexture = renderTexture;
            camera.name = "[Tool]Camera";
        }

        

        if (towerPreviews==null || towerPreviews.Count != towersParent.transform.childCount)
        {
            towerPreviews = new List<TowerBehaviour>();
            for (int i = 0; i < towersParent.transform.childCount; i++)
            {
                towerPreviews.Add(towersParent.transform.GetChild(i).GetComponent<TowerBehaviour>());
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

        if (obstaclePreviews==null || obstaclePreviews.Count != obstaclesParent.transform.childCount)
        {
            obstaclePreviews = new List<Obstacle>();
            for (int i = 0; i < obstaclesParent.transform.childCount; i++)
            {
                obstaclePreviews.Add(obstaclesParent.transform.GetChild(i).GetComponent<Obstacle>());
            }
        }


        for (int i = 0; i < obstaclePreviews.Count; i++)
        {
            if (obstaclePreviews[i] == null)
            {
                obstaclePreviews.RemoveAt(i);
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

        /*towerPrefab=EditorGUILayout.ObjectField("TowerPrefab", towerPrefab, typeof(GameObject), false) as GameObject;

        launcherPrefab = EditorGUILayout.ObjectField("Launcher prefab", launcherPrefab, typeof(GameObject), false) as GameObject;*/

        EditorGUILayout.BeginHorizontal();

        selectedTowerProperties = EditorGUILayout.ObjectField(selectedTowerProperties,typeof(TowerProperties),false) as TowerProperties;

        if (GUILayout.Button("Add tower of this ← type ") && selectedTowerProperties!=null)
        {
            GameObject towerObject = Instantiate(towerPrefab, towersParent.transform);
            towerObject.name = "Tower" + towerPreviews.Count;
            TowerBehaviour towerBehaviour = towerObject.GetComponent<TowerBehaviour>();
            towerBehaviour.Init(selectedTowerProperties);
            
            TowerData towerData = new TowerData();
            towerData.towerProperties = towerBehaviour.towerProperties;
            if (towerPreviews.Count > 0)
            {
                towerBehaviour.self.position = towerPreviews[towerPreviews.Count-1].self.position;
                towerBehaviour.self.rotation = towerPreviews[towerPreviews.Count - 1].self.rotation;
            }
            else
            {
                towerBehaviour.self.localPosition = Vector3.zero;
            }
            towerData.position = towerBehaviour.self.position;
            towerData.rotation = towerBehaviour.self.rotation;
            mapProperties.towers.Add(towerData);
            towerPreviews.Add(towerBehaviour);
            towerPreviewsCount++;
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

        EditorGUILayout.BeginHorizontal();

        selectedObstacleProperties = EditorGUILayout.ObjectField(selectedObstacleProperties, typeof(ObstacleProperties), false) as ObstacleProperties;


        

        if (GUILayout.Button("Add obstacle of this ← type") && selectedObstacleProperties!=null)
        {
            GameObject obstacleObject = Instantiate(obstaclePrefab, obstaclesParent.transform);
            
            obstacleObject.name = "Obstacle" + obstaclePreviews.Count;
            Obstacle obstacle = obstacleObject.GetComponent<Obstacle>();
            obstacle.Init(selectedObstacleProperties);

            ObstacleData obstacleData = new ObstacleData();
            obstacleData.obstacleProperties = selectedObstacleProperties;
            if (obstaclePreviews.Count > 0)
            {
                obstacle.self.position = obstaclePreviews[obstaclePreviews.Count - 1].self.position;
                obstacle.self.rotation = obstaclePreviews[obstaclePreviews.Count - 1].self.rotation;
            }
            else
            {
                obstacle.self.localPosition = Vector3.zero;
            }
            

            obstacleData.position = obstacle.self.position;
            obstacleData.rotation = obstacle.self.rotation;
            mapProperties.obstacles.Add(obstacleData);
            obstaclePreviews.Add(obstacle);
            obstaclePreviewsCount++;
            Selection.activeObject = obstacle;
        }

        if (Selection.activeGameObject != null)
        {
            Obstacle obstacle = (Selection.activeGameObject).GetComponent<Obstacle>();
            if (obstacle != null && GUILayout.Button("Remove selected obstacle", EditorStyles.miniButton))
            {
                obstaclePreviews.Remove(obstacle);
                DestroyImmediate(Selection.activeObject);
            }
        }
        EditorGUILayout.EndHorizontal();
        //ça c'est pog ↓
        GUILayout.Box(new GUIContent(renderTexture),new GUIStyle { alignment=TextAnchor.MiddleCenter});



        if (GUILayout.Button("Save current map"))
        {
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

            mapProperties.obstacles = new List<ObstacleData>();
            for (int i = 0; i < obstaclePreviews.Count; i++)
            {
                mapProperties.obstacles.Add(new ObstacleData
                {
                    obstacleProperties = obstaclePreviews[i].obstacleProperties,
                    position = obstaclePreviews[i].self.position,
                    rotation = obstaclePreviews[i].self.rotation
                });
            }
            mapProperties.cameraPosition = camera.transform.position;
            mapProperties.cameraRotation = camera.transform.rotation;
            mapProperties.launcherPosition = ballLauncher.transform.position;
            mapProperties.launcherRotation = ballLauncher.transform.rotation;

            mapProperties.groundPosition = ground.transform.position;
            mapProperties.groundRotation = ground.transform.rotation;

            mapProperties.borderPositions = new List<Vector3>()
            {
                rightBorder.transform.position,
                leftBorder.transform.position,
                topBorder.transform.position,
                bottomBorder.transform.position
            };

            mapProperties.borderRotations = new List<Quaternion>()
            {
                rightBorder.transform.rotation,
                leftBorder.transform.rotation,
                topBorder.transform.rotation,
                bottomBorder.transform.rotation
            };

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
        DestroyImmediate(towersParent);
        towersParent = new GameObject();
        towersParent.name = "[Tool]Towers";
        DestroyImmediate(obstaclesParent);
        obstaclesParent = new GameObject();
        obstaclesParent.name = "[Tool]Obstacles";
        DestroyImmediate(camera.gameObject);
        GameObject cameraObject = new GameObject();
        cameraObject.AddComponent<Camera>();
        cameraObject.transform.position = mapProperties.cameraPosition;
        cameraObject.transform.rotation = mapProperties.cameraRotation;
        camera = cameraObject.GetComponent<Camera>();
        camera.targetTexture = renderTexture;
        camera.name = "[Tool]Camera";
        DestroyImmediate(ballLauncher.gameObject);
        ballLauncher = Instantiate(launcherPrefab);
        ballLauncher.name = "[Tool]BallLauncher";
        ballLauncher.transform.position = mapProperties.launcherPosition;
        ballLauncher.transform.rotation = mapProperties.launcherRotation;
        ground.transform.position = mapProperties.groundPosition;
        ground.transform.rotation = mapProperties.groundRotation;
        rightBorder.transform.position = mapProperties.borderPositions[0];
        leftBorder.transform.position = mapProperties.borderPositions[1];
        topBorder.transform.position = mapProperties.borderPositions[2];
        bottomBorder.transform.position = mapProperties.borderPositions[3];
        rightBorder.transform.rotation = mapProperties.borderRotations[0];
        leftBorder.transform.rotation = mapProperties.borderRotations[1];
        topBorder.transform.rotation = mapProperties.borderRotations[2];
        bottomBorder.transform.rotation = mapProperties.borderRotations[3];


        towerPreviews = new List<TowerBehaviour>();
        obstaclePreviews = new List<Obstacle>();
        if (mapProperties != null)
        {
            for (int i = 0; i < mapProperties.towers.Count; i++)
            {
                GameObject towerObject = Instantiate(towerPrefab, towersParent.transform);
                towerObject.name = "Tower" + i;
                TowerBehaviour towerBehaviour = towerObject.GetComponent<TowerBehaviour>();
                towerBehaviour.Init(mapProperties.towers[i].towerProperties);
                towerBehaviour.self.position = mapProperties.towers[i].position;
                towerPreviews.Add(towerBehaviour);
            }

            for (int i = 0; i < mapProperties.obstacles.Count; i++)
            {
                GameObject obstacleObject = Instantiate(obstaclePrefab,obstaclesParent.transform);
                obstacleObject.name = "Obstacle" + i;
                Obstacle obstacle = obstacleObject.GetComponent<Obstacle>();
                obstacle.Init(mapProperties.obstacles[i].obstacleProperties);
                obstacle.self.position = mapProperties.obstacles[i].position;
                obstacle.self.rotation = mapProperties.obstacles[i].rotation;
            }

        }

        
        
    }


}
