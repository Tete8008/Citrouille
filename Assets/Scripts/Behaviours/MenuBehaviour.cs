using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;

    public GameObject splashPanel;
    public GameObject tutorialPanel;
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject leaderboardsPanel;
    public GameObject upgradesPanel;
    public GameObject optionsPanel;
    public GameObject gameoverPanel;
    public GameObject missionPanel;
    public GameObject adremovalPanel;
    public GameObject sharePanel;
    public GameObject achievementsPanel;
    public GameObject changeCurrencyPanel;
    public GameObject customizationPanel;

    public void Play()
    {
        //afficher l'écran Game
    }

    public void Quit()
    {
        Application.Quit();
    }
}
