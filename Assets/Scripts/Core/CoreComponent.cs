using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoreComponent{

    public IOInterface io;

    protected CoreComponent(int unityBufferSize)
    {
        io = new IOInterface(unityBufferSize, this);
    }

    public virtual void processControl(int controlNum, float value){}
}
