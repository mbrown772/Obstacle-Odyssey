/*
 * Author:      Matthew Brown
 * Date:        11/30/24
 * Description: This is the camera controller. Makes the camera follow the player.
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; //Link to the player object to track
    public Vector3 offset;    //If we want to mess with the offset in unity.

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 holder = player.transform.position;
        holder.y = -3.5f; //So we don't move the camera up and down with every jump
        transform.position = holder + offset;
    }
}
