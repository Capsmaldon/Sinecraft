using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSocketComponent : CoreSocketComponent
{
    private float value;

    public DataSocketComponent(CoreComponent adjoinedComponent) : base(adjoinedComponent)
    {

    }

    public void set(float value)
    {
        this.value = value;
    }

    public float getValue()
    {
        return value;
    }

    //Push the data to the next block
    public void push()
    {
        DataSocketComponent connectedDataSocket = connectedSocket as DataSocketComponent;

        if (connectedDataSocket != null) 
        {
            connectedDataSocket.value = value;
            connectedDataSocket.adjoinedComponent.processControl(connectedDataSocket.controlNum, value);
        }
    }

    public void zero()
    {
        value = 0;
    }
}

