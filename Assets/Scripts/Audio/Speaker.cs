using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : AudioComponent {

    public Speaker(int unitySampleRate, int unityBufferSize) : base(unitySampleRate, unityBufferSize)
    {

    }

    protected override void processBuffer()
    {
        float[] inL = io.audioInputs[0].getBuffer();
        float[] inR = io.audioInputs[1].getBuffer();

        //Limiter
        if (inL != null)
        {
            for (int i = 0; i < bufferLength; i++)
            {
                if (inL[i] > 1.0f) inL[i] = 1.0f;
                else if (inL[i] < -1.0f) inL[i] = -1.0f;
            }
        }

        if(inR != null)
        {
            for (int i = 0; i < bufferLength; i++)
            {
                if (inR[i] > 1.0f) inR[i] = 1.0f;
                else if (inR[i] < -1.0f) inR[i] = -1.0f;
            }
        }
    }
}


