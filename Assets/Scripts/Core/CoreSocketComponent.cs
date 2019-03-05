using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSocketComponent{

    public int socketNum;
    public int controlNum;
    public CoreComponent adjoinedComponent;
    public CoreSocketComponent connectedSocket;

    //Initialisation of lists
    public CoreSocketComponent(CoreComponent adjoinedComponent)
    {
        this.adjoinedComponent = adjoinedComponent;
        connectedSocket = null;
    }

    public void setSocketNum(int socketNum)
    {
        this.socketNum = socketNum;
    }

    public void setControlNum(int controlNum)
    {
        this.controlNum = controlNum;
    }

    //Connect a socket to another object
    public void connect(CoreSocketComponent socket)
    {
        if (connectedSocket == null)
        {
            //If nothing's plugged in connect
            connectedSocket = socket;
        }
        else
        {
            //If something else is plugged in, disconnect the previous socket and connect the new one
            connectedSocket.connectedSocket = null;
            connectedSocket = socket;
        }
    }

    //Disconnect socket from whatever it's plugged in to
    public void disconnect()
    {
        if (connectedSocket != null) connectedSocket.connectedSocket = null;
        connectedSocket = null;
    }

    public bool isConnected()
    {
        if (connectedSocket != null) return true;
        else return false;
    }
}
