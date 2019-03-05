using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSocketComponent : CoreSocketComponent
{
    private float[] buffer; // Where to store the buffers before and after processing - index corresponds to socket

    //Initialisation of lists
    public AudioSocketComponent(int bufferLength, CoreComponent adjoinedComponent) : base(adjoinedComponent)
    {
        buffer = new float[bufferLength];
    }

    public void setBuffer(float[] buffer)
    {
        this.buffer = buffer;
    }

    public float[] getBuffer()
    {
        AudioSocketComponent connectedAudioSocket = connectedSocket as AudioSocketComponent;
        if (connectedAudioSocket != null) return buffer;
        else return null;
    }

    //Push the data from to the next block
    public void push()
    {
        AudioSocketComponent connectedAudioSocket = connectedSocket as AudioSocketComponent;
        if (connectedAudioSocket != null) connectedAudioSocket.buffer = buffer;
    }

    public void zero()
    {
        if(buffer != null)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }
        }
    }
}
