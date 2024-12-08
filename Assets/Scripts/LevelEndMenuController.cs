/*
 * Author:      Matthew Brown
 * Date:        11/30/24
 * Description: Controls the level end menu. Will display a victory window if won or a defeat window if lost.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndMenuController : MonoBehaviour
{
    private GameObject GameController;      //Grab the singleton gamecontroller
    private GameController gameController;  //Grab its script
    private bool menuState;                 //Are we a victor or loser
    public Text menuTitle;                  //Victory or defeat
    public Text topButtonTitle;             //Next level or restart

    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.FindWithTag("GameController");
        gameController = GameController.GetComponent<GameController>();
    }

    //Game controller tells the end menu to display either the victory or defeat layout
    public void displayLevelEndMenu(bool alive)
    {
        menuState = alive;
        if(alive) //Victory
        {
            menuTitle.text = "Victory";
            topButtonTitle.text = "Next Level";
        }
        else //Defeat
        {
            menuTitle.text = "Defeat";
            topButtonTitle.text = "Restart";
        }
    }

    //On click for the top button. If victory next level otherwise reset current level.
    public void topButtonClicked()
    {
        if(menuState) //Victory
        {
            gameController.nextLevelClicked();
        }
        else //Loss
        {
            gameController.resetButtonClicked();
        }
    }

    //On click for bottom main menu button
    public void bottomButtonClicked()
    {
        gameController.enterMainMenu();
    }

}
