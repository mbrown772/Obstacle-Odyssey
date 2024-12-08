/*
 * Author:      Matthew Brown      
 * Date:        11/30/24
 * Description: This is the main game controller logic script. Player and the UI communicate to this script to control game flow.
 *              This object is a singleton created in the main menu and is not rebuilt on scene load. Controls the pause menu, level end menu, last level menu,
 *              and the start level button. This script also handles updating what level the player is currently on in player prefs. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int currentLevel;               //Current Level read in from player prefs
    private const int LASTLEVEL = 2;        //CONST for how many levels there are
    private static GameController instance; //So this script is a singleton

    private GameObject player;                  //Player grabbed when a player spawns in
    private PlayerController playerController;  //Grabs the script as well

    private GameObject pauseMenu;               //Grabs the pausemenu in the scene when player loads
    private GameObject startLevelButton;        //Grabs the start level button when player loads

    private GameObject lastLevelMenu;          //Grabs the last level menu if we are in the last level

    private GameObject levelEndMenu;                       //Grab the level end menu when player loads
    private LevelEndMenuController levelEndMenuController; //Grab the controller as weell

    private bool inLevelEndMenu = false; //Are we in the level end menu to prevent update from constantly reloading the menu
    private bool inPauseMenu = false;    //Are we in the pause menu so we can swap between turning on and off
    private bool levelComplete = false;  //Did we beat the level. so we can load the correct level end menu
    private bool dead = false;           //Did the player die. so we can load the correct level end menu
    private bool inMainMenu = true;      //are we in the main menu. Prevents opening pause menu in main menu
    private bool startButtonClicked = false;

    //Make this script a singleton
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        { 
            Destroy(gameObject);
        }

       currentLevel = getCurrentLevel();
        if (currentLevel < 1 || currentLevel > LASTLEVEL) //Make sure nothing funky has happened with the player prefs
        {
            resetCurrentLevel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!inLevelEndMenu)
        {
            if (dead) //Lost current level
            {
                inLevelEndMenu = true;
                levelEndMenu.SetActive(true);
                levelEndMenuController.displayLevelEndMenu(false);
            }
            else if (levelComplete) //Won current level
            {
                inLevelEndMenu = true;
                if(currentLevel < LASTLEVEL) 
                {
                    updateCurrentLevel();
                    levelEndMenu.SetActive(true);
                    levelEndMenuController.displayLevelEndMenu(true);
                }
                else //Last level
                {
                    lastLevelMenu.SetActive(true);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!inMainMenu && !inLevelEndMenu) //So we can't open the pause menu in the main menu or levelEndMenu
            {
                Debug.Log("enter pause menu");
                pauseMenuHelper();
            }
        }
    }

    //Called when esc is clicked. Pauses game time and flip flops on and off depending on if we are in it or not
    //Also called by the exit button in pause menu
    public void pauseMenuHelper()
    {
        if (inPauseMenu) //Leave the menu if we are in it
        {
            if(!startButtonClicked) startLevelButton.SetActive(true); //Display the start level button if it hasn't already been clicked
            Time.timeScale = 1; 
            pauseMenu.SetActive(false);
            inPauseMenu = false;
        }
        else //Enter the menu
        {
            startLevelButton.SetActive(false); //Hide it so its not over the menu
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            inPauseMenu = true;
        }
    }

    //Next level button was clicked in the end menu
    public void nextLevelClicked()
    {
        inLevelEndMenu = false;
        resetLevelStates();
        launchCurrentLevel();
    }

    //Reset button was clicked in defeat end menu
    public void resetButtonClicked()
    {
        inLevelEndMenu = false;
        resetLevelStates();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Main menu button was clicked. Use in pause, end menu, level end menu, and last level menu
    public void enterMainMenu()
    {
        inMainMenu = true;
        SceneManager.LoadScene("MainMenu");
    }

    //Start clicked in main menu. Launches the level the player is currently on.
    public void exitMainMenu()
    {
        inMainMenu = false;
        resetLevelStates();
        launchCurrentLevel();
    }

    //Start level button clicked after level load in. Starts player movement.
    public void startLevelClicked()
    {
        startButtonClicked = true;
        startLevelButton.SetActive(false);
        playerController.enableMovePlayer();
    }

    //Used by player controller to tell the game controller the player died.
    public void playerDeath()
    {
        dead = true;
    }

    //Used by player controller to tell game controller the player won.
    public void playerLevelEnd()
    {
        levelComplete = true;
    }

    //Called by the player script when it is loaded in. This function resets game logic to what it should be at the beginning
    //of the level. It also grabs the ui components and player components.
    public void newSceneLoaded()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        pauseMenu = GameObject.FindWithTag("PauseMenu");
        pauseMenu.SetActive(false);

        startLevelButton = GameObject.FindWithTag("StartLevelButton");

        levelEndMenu = GameObject.FindWithTag("LevelEndMenu");
        levelEndMenuController = levelEndMenu.GetComponent<LevelEndMenuController>();
        levelEndMenu.SetActive(false);

        if(currentLevel == LASTLEVEL) //We are on the last elvel
        {
            try
            {
                lastLevelMenu = GameObject.FindWithTag("LastLevelMenu");
                lastLevelMenu.SetActive(false);
            }
            catch (System.Exception ex) //In case something weird hapened
            {
                resetLevelStates();
                enterMainMenu();
            }
        }

        resetLevelStates();
    }

    //Resets game logic variables to default state for a new level.
    private void resetLevelStates()
    {
        Physics2D.gravity = new Vector2(0, -9.81f); //In case we hit a stop move return to default
        inPauseMenu = false;
        levelComplete = false;
        dead = false;
        inLevelEndMenu = false;
        inMainMenu = false;
        startButtonClicked = false;
    }

    //Function to reset the current level to level 1.
    public void resetCurrentLevel()
    {
        currentLevel = 1;
        PlayerPrefs.SetInt("currentLevel", 1);
        PlayerPrefs.Save();
    }

    //Called when the player wins a level so the player can progress to the next level
    private void updateCurrentLevel() 
    {
        currentLevel++;
        if(currentLevel > LASTLEVEL || currentLevel < 1) //This should never happen without editing files directly
        {
            currentLevel = 1;
        }
        PlayerPrefs.SetInt("currentLevel", currentLevel);
        PlayerPrefs.Save();
    }

    //Called on initial launch of the game. Returns the current level or 0 if the pref hasn't been made yet
    private int getCurrentLevel()
    {
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            return PlayerPrefs.GetInt("currentLevel");
        }
        else return 0;
    }

    //Launches the level the player is currently on. Used by end menu continue button and main menu start button.
    public void launchCurrentLevel()
    {
        string levelLoad = "Level" + currentLevel.ToString();
        Debug.Log("Launching: " + levelLoad);
        SceneManager.LoadScene(levelLoad);
    }
}
