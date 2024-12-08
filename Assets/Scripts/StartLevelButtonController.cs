/*
 * Author:      Matthew Brown
 * Date:        11/30/24
 * Description: The script controls the start level button at the beginning of each level. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelButtonController : MonoBehaviour
{
    private GameObject GameController;     //get game controller object
    private GameController gameController; //So we can use gameController startLeelCLicked()
    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.FindWithTag("GameController"); //Grab the singleton game controller
        gameController = GameController.GetComponent<GameController>();
    }

    //Used by start level button to tell game controller to start the level.
    public void startLevelButtonClicked()
    {
        gameController.startLevelClicked();
    }
}
