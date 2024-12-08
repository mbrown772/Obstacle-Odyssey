/*
 * Author:      Matthew Brown
 * Date:        11/30/24
 * Description: Controls player movement and collision detection on player. Tells the game controller if the player 
 *              either died or won. Handles the animations and sounds for the player as well.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f; //how high player jumps set in unity
    public float moveSpeed = 5f; //How fast the player moves set in unity

    private GameObject gameControllerObj; //So we can grab the singleton gameController
    private GameController gameController;//So we can grab the singleton gameController scipt

    public Animator playerAnimator; //Unity link for animator attached to player
    private bool airborne = false;  //Tells us if the player is airborne
    private bool endState = false;  //Tells if we have either died or won 
    private bool movePlayer = false;//If we should move the player or not
    private Rigidbody2D rb; //Rigidbody attached to player
    private AudioSource[] audioSources; //All the audio sources on the player object

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gameControllerObj = GameObject.FindWithTag("GameController");
        gameController = gameControllerObj.GetComponent<GameController>();

        gameController.newSceneLoaded();

        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movePlayer)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y); //rb.velocity.y respects the force added to it
            if (Input.GetKey("space")) //JUMP
            {
                if (!airborne)
                {
                    audioSources[0].Play(); //Play jump sound

                    playerAnimator.SetBool("IsJumping", true); //Enable jump animation
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    airborne = true;
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero; //Stop movement
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) //If we touch the ground after jumping
        {
            playerAnimator.SetBool("IsJumping", false); //Disable jump animation
            airborne = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!endState) //If we have not already died or won
        {
            if (collision.gameObject.CompareTag("Death")) //If we touched a trigger for death
            {
                audioSources[2].Play(); //Play pop sound
                endState = true;
                disableMovePlayer();
                playerAnimator.SetBool("Dead", true);
                gameController.playerDeath();
            }
            else if (collision.gameObject.CompareTag("LevelEnd")) //If we touched a trigger for victory
            {
                endState = true;
                disableMovePlayer();
                playerAnimator.SetBool("Victory", true);
                gameController.playerLevelEnd();
            }
            else if (collision.gameObject.CompareTag("StopMove")) //If we touched a trigger to stop moving usually on the side of platforms
            {
                disableMovePlayer();
                Physics2D.gravity = new Vector2(0, -50f); //So we don't hang for a long time on the wall
            }
        }
    }

    //Short helper function to disable the players forward movement and set the idle animation
    public void disableMovePlayer() 
    {
        playerAnimator.SetFloat("Speed", 0f);
        movePlayer = false;
        audioSources[1].Stop(); //Sto running sound
    }
    //Makes the player move forward and enable running animation
    public void enableMovePlayer() 
    {
        playerAnimator.SetFloat("Speed", 50f);
        movePlayer = true;
        audioSources[1].Play(); //Playing running sound
    }
}
