using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance = null;

    public GameObject towerPrefab;
    public GameObject ballLauncherPrefab;
    public GameObject groundPrefab;
    private GameObject map;
    [System.NonSerialized]public Camera cam;

    private List<TowerBehaviour> towers;
    private BallLauncher ballLauncher;
    private GameObject ground;

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

    public static void GenerateMap(MapProperties mapProperties)
    {
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
                instance.towers.Add(towerBehaviour);
            }
        }
        instance.ballLauncher=Instantiate(instance.ballLauncherPrefab).GetComponent<BallLauncher>();
        instance.ballLauncher.transform.position = mapProperties.launcherPosition;
        instance.ballLauncher.transform.rotation = mapProperties.launcherRotation;
        instance.ground = Instantiate(instance.groundPrefab);
        instance.ground.transform.position = mapProperties.groundPosition;
        instance.ground.transform.rotation = mapProperties.groundRotation;
    }
}
