using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioComponent : CoreComponent {

    protected int sampleRate;
    protected float sampleTime;
    protected int bufferLength;

    public AudioComponent(int unitySampleRate, int unityBufferSize) : base(unityBufferSize)
    {
        //Core constants for all audio components
        sampleRate = unitySampleRate;
        sampleTime = 1.0f / sampleRate;
        bufferLength = unityBufferSize;
    }

    public void tickBuffer()
    {
        processBuffer();
    }

    //Every audio component must do something when the buffer is ticked
    protected abstract void processBuffer();
}
