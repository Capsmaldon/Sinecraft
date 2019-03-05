using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Control
{
    public int numOfStates;

    private float statePosition;
    private float statePositionIncrement;

    private Vector3 position;

    public override void initialise()
    {
        position = transform.localPosition;
        statePosition = position.x;
        statePositionIncrement = (position.x*-2.0f) / (numOfStates - 1);
    }

    public override void select(Vector3 playerPoint)
    {
        if (statePosition >= position.x*-1.0f)
        {
            statePosition = position.x;
            value = 0;
            sendValue();
        }
        else
        {
            statePosition += statePositionIncrement;
            value++;
            sendValue();
        }
        transform.localPosition = new Vector3(statePosition, transform.localPosition.y, transform.localPosition.z);
    }
}

