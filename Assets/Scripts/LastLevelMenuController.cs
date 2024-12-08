/*
 * Author:      Matthew Brown
 * Date:        12/6/24
 * Description: Controls the end level menu that is displayed when the last level is beaten.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastLevelMenuController : MonoBehaviour
{
    private GameObject GameController;     //So we ccan call gameContorller fucntions
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.FindWithTag("GameController"); //Grab the singleton game controller
        gameController = GameController.GetComponent<GameController>();
    }

    //On click for the main menu button. Resets to level one and returns to the main menu.
    public void mainMenuClicked()
    {
        gameController.resetCurrentLevel();
        gameController.enterMainMenu();
    }
}
