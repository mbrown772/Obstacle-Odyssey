/*
 * Author:      Matthew Brown
 * Date:        11/30/24
 * Description: This has the pause menu on click functions for all buttons in main menu scene.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private GameObject gameControllerObj;
    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameControllerObj = GameObject.FindWithTag("GameController");
        gameController = gameControllerObj.GetComponent<GameController>();
    }

    //Go to the main menu. Reset the Timescale to 1 because it is set to 0 when the menu is opened
    public void enterMainMenu()
    {
        Time.timeScale = 1;  //rest the timescale so the game works will be left hanging 
        gameController.enterMainMenu();
    }

    //Closes the menu top right x button.
    public void closeMenu()
    {
        gameController.pauseMenuHelper();
    }

    //Exit game button. Exits game
    public void exitGame()
    {
        Application.Quit();
    }

}
