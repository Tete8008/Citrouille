using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject canvasPrefab;
    public GameObject mapManagerPrefab;


    [NonSerialized] public long score;
    [NonSerialized] public int level;
        




    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    public void AddScore(int value)
    {
        score += value;
    }


    public static void Init()
    {
        Transform self = instance.transform;


        //spawn all managers and load Game scene

        GameObject uimanager = new GameObject();
        uimanager.transform.SetParent(self);
        uimanager.name = "UIManager";
        uimanager.AddComponent<UIManager>();
        UIManager.instance.canvasPrefab = instance.canvasPrefab;

        GameObject mapManager = Instantiate(instance.mapManagerPrefab);
        mapManager.transform.SetParent(self);


        GameObject inputManager = new GameObject();
        inputManager.name = "InputManager";
        inputManager.transform.SetParent(self);
        inputManager.AddComponent<InputManager>();


        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        SceneManager.LoadScene("Game");


    }

    private static void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //si on charge le game <=> si on charge le jeu pour la première fois
        if (arg0.name == "Game")
        {
            MapManager.GenerateMap();
        }
    }


    public void WinLevel()
    {
        //uimanager.popWinPanel

    }

    public void LoseLevel()
    {
        //uiManager.popLosePanel
    }


}
