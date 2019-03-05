using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Screen { Splash,Menu,Game,Leaderboards,Upgrades,Options,Missions,Adremoval,Tutorial,Share,Achievements,ChangeCurrency,Customization}

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    //prefabs
    [System.NonSerialized] public GameObject canvasPrefab;

    //variables
    public MenuBehaviour menu;
    private GameObject activePanel;

<<<<<<< HEAD
    private bool gameoverPanelActive;

=======
>>>>>>> master

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


    public static void SpawnCanvas()
    {
        instance.menu = Instantiate(instance.canvasPrefab).GetComponent<MenuBehaviour>();
        ChangeScreen(Screen.Splash);
    }

    public static void ChangeScreen(Screen screen)
    {
        //afficher différents panels en fonction de 'screen'
        instance.activePanel.SetActive(false);

        switch (screen)
        {
            case Screen.Menu:
                instance.activePanel = instance.menu.menuPanel;
                break;
            case Screen.Game:
                instance.activePanel = instance.menu.gamePanel;
                break;
            case Screen.Leaderboards:
                instance.activePanel = instance.menu.leaderboardsPanel;
                break;
            case Screen.Upgrades:
                instance.activePanel = instance.menu.upgradesPanel;
                break;
            case Screen.Options:
                instance.activePanel = instance.menu.optionsPanel;
                break;
            case Screen.Missions:
                instance.activePanel = instance.menu.missionPanel;
                break;
            case Screen.Adremoval:
                instance.activePanel = instance.menu.adremovalPanel;
                break;
            case Screen.Splash:
                instance.activePanel = instance.menu.splashPanel;
                break;
            case Screen.Tutorial:
                instance.activePanel = instance.menu.tutorialPanel;
                break;
            case Screen.Share:
                instance.activePanel = instance.menu.sharePanel;
                break;
            case Screen.Achievements:
                instance.activePanel = instance.menu.achievementsPanel;
                break;
            case Screen.ChangeCurrency:
                instance.activePanel = instance.menu.changeCurrencyPanel;
                break;
            case Screen.Customization:
                instance.activePanel = instance.menu.customizationPanel;
                break;
        }
        instance.activePanel.SetActive(true);
    }

    public static void DisplayGameoverScreen()
    {
<<<<<<< HEAD
        instance.gameoverPanelActive = true;
=======
>>>>>>> master
        instance.menu.gameoverPanel.SetActive(true);
    }
}
