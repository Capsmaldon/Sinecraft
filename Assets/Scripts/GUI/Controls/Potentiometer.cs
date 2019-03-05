using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potentiometer : Control {

    private float offset;
    private float prevRotation;
    private Vector3 rotation;
    private Vector3 newPosition;
    private float yRotation;
    private float minRotation;
    private float maxRotation;
    private float valueMultiplier;

    //Get the current rotational position
    public override void initialise()
    {
        rotation = transform.localEulerAngles;
        minRotation = 0;
        maxRotation = 300;
        valueMultiplier = 1 / maxRotation;
    }

    //Imagine there is a 2D plane is infront of the player, project onto that and get the current y position of it
    public override void select(Vector3 playerPoint)
    {
        newPosition = Vector3.ProjectOnPlane(playerPoint, transform.position);
        offset = newPosition.y;
    }

    public override void interact(Vector3 playerPoint)
    {
        //Recalculate the position on the plane
        newPosition = Vector3.ProjectOnPlane(playerPoint, transform.position);

        //The new rotation will be the old rotation, + the change in the projected position, scaled up to be in degrees
        yRotation = prevRotation + ((newPosition.y - offset) * 2.0f * 300.0f);

        //It cannot rotate more or less than it's range (0 to 300)
        if (yRotation < minRotation) yRotation = minRotation;
        else if (yRotation > maxRotation) yRotation = maxRotation;

        //Change it's rotation and calculate the value
        transform.localEulerAngles = new Vector3(rotation.x, yRotation, rotation.z);
        setValue((yRotation * valueMultiplier * 2) - 1);
    }

    public override void release()
    {
        prevRotation = yRotation;
    }
}
