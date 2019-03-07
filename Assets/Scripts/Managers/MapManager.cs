using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance = null;

    //Prefabs
    public GameObject towerPrefab;
    public GameObject ballLauncherPrefab;
    public GameObject groundPrefab;
    public GameObject borderPrefab;
    public GameObject obstaclePrefab;

    //Variables
    //private GameObject map;
    [HideInInspector] public Camera cam;
    private List<TowerBehaviour> towers;
    private List<Obstacle> obstacles;
    /*
    private BallLauncher ballLauncher;
    private GameObject ground;
    private GameObject rightBorder;
    private GameObject leftBorder;
    private GameObject topBorder;
    private GameObject bottomBorder;*/

    public List<GameObject> chunkPrefabs;

    public float gridScale;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void GenerateMap()
    {
        if (instance.chunkPrefabs.Count == 0) { Debug.LogWarning("no chunk prefab referenced");return; }

        MapGrid.instance.ground.transform.localScale *= instance.gridScale;
        instance.cam = MapGrid.instance.cam;
        instance.towers = new List<TowerBehaviour>();
        instance.obstacles = new List<Obstacle>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 2; j < 7; j++)
            {
                Chunk chunk = Instantiate(instance.chunkPrefabs[Random.Range(0, instance.chunkPrefabs.Count)]).GetComponent<Chunk>() ;
                chunk.self.position = new Vector3(instance.gridScale * i*10+5, 0, instance.gridScale * j*10);
                chunk.groundPreview.SetActive(false);
                for (int k=0;k< chunk.towersParent.childCount; k++)
                {
                    TowerBehaviour tower = chunk.towersParent.GetChild(k).gameObject.GetComponent<TowerBehaviour>();
                    tower.Init(tower.towerProperties);
                    instance.towers.Add(tower);
                }

                for (int k = 0; k < chunk.obstaclesParent.childCount; k++)
                {
                    Obstacle obstacle = chunk.obstaclesParent.GetChild(k).GetComponent<Obstacle>();
                    obstacle.Init(obstacle.obstacleProperties);
                    instance.obstacles.Add(obstacle);
                }
            }
        }


        /*
        instance.map = new GameObject();
        instance.map.name = "Map";
        GameObject cameraObject = new GameObject();
        cameraObject.AddComponent<Camera>();
        instance.cam = cameraObject.GetComponent<Camera>();
        instance.cam.tag = "MainCamera";
        instance.cam.name = "CAMERA";
        instance.cam.transform.position = mapProperties.cameraPosition;
        instance.cam.transform.rotation = mapProperties.cameraRotation;
        if (mapProperties != null)
        {
            instance.towers = new List<TowerBehaviour>();
            for (int i = 0; i < mapProperties.towers.Count; i++)
            {
                GameObject towerObject = Instantiate(instance.towerPrefab);
                towerObject.transform.SetParent(instance.map.transform);
                towerObject.name = "Tower" + i;
                TowerBehaviour towerBehaviour = towerObject.GetComponent<TowerBehaviour>();
                towerBehaviour.Init(mapProperties.towers[i].towerProperties);
                towerBehaviour.self.position = mapProperties.towers[i].position;
                towerBehaviour.self.rotation = mapProperties.towers[i].rotation;
                instance.towers.Add(towerBehaviour);
            }

            instance.obstacles = new List<Obstacle>();
            for (int i = 0; i < mapProperties.obstacles.Count; i++)
            {
                GameObject obstacleObject = Instantiate(instance.obstaclePrefab);
                obstacleObject.name = "Obstacle" + i;
                Obstacle obstacle = obstacleObject.GetComponent<Obstacle>();
                obstacle.Init(mapProperties.obstacles[i].obstacleProperties);
                obstacle.self.position = mapProperties.obstacles[i].position;
                obstacle.self.rotation = mapProperties.obstacles[i].rotation;
                instance.obstacles.Add(obstacle);
            }
        }
        instance.ballLauncher=Instantiate(instance.ballLauncherPrefab).GetComponent<BallLauncher>();
        instance.ballLauncher.transform.position = mapProperties.launcherPosition;
        instance.ballLauncher.transform.rotation = mapProperties.launcherRotation;
        instance.ground = Instantiate(instance.groundPrefab);
        instance.ground.transform.position = mapProperties.groundPosition;
        instance.ground.transform.rotation = mapProperties.groundRotation;

        instance.rightBorder = Instantiate(instance.borderPrefab);
        instance.leftBorder = Instantiate(instance.borderPrefab);
        instance.topBorder = Instantiate(instance.borderPrefab);
        instance.bottomBorder = Instantiate(instance.borderPrefab);

        instance.rightBorder.transform.position = mapProperties.borderPositions[0];
        instance.leftBorder.transform.position = mapProperties.borderPositions[1];
        instance.topBorder.transform.position = mapProperties.borderPositions[2];
        instance.bottomBorder.transform.position = mapProperties.borderPositions[3];
        instance.rightBorder.transform.rotation = mapProperties.borderRotations[0];
        instance.leftBorder.transform.rotation = mapProperties.borderRotations[1];
        instance.topBorder.transform.rotation = mapProperties.borderRotations[2];
        instance.bottomBorder.transform.rotation = mapProperties.borderRotations[3];*/
    }

    public void RemoveTower(TowerBehaviour tower)
    {
        towers.Remove(tower);
        if (towers.Count == 0)
        {
            GameManager.instance.AddScore(BallLauncher.instance.GetBonusPoints());
            GameManager.instance.WinLevel();
            Debug.Log("YOU WIN");
        }
    }

}
