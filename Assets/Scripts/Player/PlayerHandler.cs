using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

    //Turn speeds
    public float f_verticalSpeed;
    public float f_horizontalSpeed;

    //Player movement speed
    public float f_moveSpeed;

    //Player interaction distance
    public float f_interactionDistance;

    //Camera rotation
    private float yaw;
    private float pitch;

    //The camera attached to the player
    public Camera playerCamera;

	// Use this for initialization
	void Start () {
        playerCamera = GetComponentInChildren<Camera>();
	}

    //Rotate the player according to the mouse position
    public void rotate(float mouseX, float mouseY)
    {
        //Calculate rotation corresponding to mouse position
        yaw += f_verticalSpeed * mouseX;
        pitch += f_horizontalSpeed * mouseY;

        //Rotate the camera accordingly
        playerCamera.transform.eulerAngles = new Vector3(-pitch, yaw, 0.0f);

        //Rotate the player model around the y axis
        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
    }

    //Move the player according to which 'wasd' keys are pressed
    public void move(int direction)
    {
        if(direction == 0)
        {
            transform.Translate(new Vector3(0, 0, f_moveSpeed) * Time.deltaTime);
        }
        if (direction == 1)
        {
            transform.Translate(new Vector3(-f_moveSpeed, 0, 0) * Time.deltaTime);
        }
        if (direction == 2)
        {
            transform.Translate(new Vector3(0, 0, -f_moveSpeed) * Time.deltaTime);
        }
        if (direction == 3)
        {
            transform.Translate(new Vector3(f_moveSpeed, 0, 0) * Time.deltaTime);
        }
    }

}
