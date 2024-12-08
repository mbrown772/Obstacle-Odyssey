/*
 * Author:      Matthew Brown
 * Date:        11/30/24
 * Description: Has the on click button functions for the main menu.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameController gameController; //So we can call gameController functions
    public GameObject instructionsMenu;   //So we can open and close the instructions menu
    public GameObject mainMenu;           //So we can open and close the main menu

    void Start()
    {
        instructionsMenu.SetActive(false);
    }

    //The start level button was clicked
    public void startButtonClicked()
    {
        gameController.exitMainMenu();
    }
    
    //The exit game button was clicked
    public void exitButtonClicked()
    {
        Application.Quit();
    }
    
    //Return button was clicked
    public void returnButtonClicked()
    {
        mainMenu.SetActive(true);
        instructionsMenu.SetActive(false);
    }

    //Instruction button was clicked
    public void instructionsButtonClicked()
    {
        mainMenu.SetActive(false);
        instructionsMenu.SetActive(true);
    }

}
