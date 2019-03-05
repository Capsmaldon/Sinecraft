using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amplifier : AudioComponent
{
    private float flatAmp;

    public Amplifier(int unitySampleRate, int unityBufferSize) : base(unitySampleRate, unityBufferSize)
    {
        flatAmp = 1.0f;
    }

    protected override void processBuffer()
    {
        float[] input = io.audioInputs[0].getBuffer();
        float[] ampBuffer = io.audioInputs[1].getBuffer();
        float[] output = io.audioOutputs[0].getBuffer();

        if (output != null && input != null)
        {
            if (ampBuffer != null)
            {
                for (int n = 0; n < bufferLength; n++)
                {
                    output[n] = input[n] * (ampBuffer[n] * 0.5f + 0.5f);
                }
            }
            else
            {
                for (int n = 0; n < bufferLength; n++)
                {
                    output[n] = input[n] * flatAmp;
                }
            }
        }
        else if(output != null && input == null) // This works for now and doesn't break anything - pushes a buffer full of zeros if theres an output connected but no input
        {
            for (int n = 0; n < bufferLength; n++)
            {
                output[n] = 0;
            }
        }

        io.audioOutputs[0].push();
    }

    public override void processControl(int controlNum, float value)
    {
        switch (controlNum)
        {
            case 0:
                flatAmp = value;
                break;
        }
    }
}
