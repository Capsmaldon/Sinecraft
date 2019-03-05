using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : AudioComponent {

    public Splitter(int unitySampleRate, int unityBufferSize) : base(unitySampleRate, unityBufferSize){}

    protected override void processBuffer()
    {
        float[] inputBuffer = io.audioInputs[0].getBuffer();
        float[] out1 = io.audioOutputs[0].getBuffer();
        float[] out2 = io.audioOutputs[1].getBuffer();

        if(inputBuffer != null)
        {
            if(out1 != null)
            {
                for (int i = 0; i < bufferLength; i++)
                {
                    out1[i] = inputBuffer[i];
                }
            }
            
            if (out2 != null)
            {
                for (int i = 0; i < bufferLength; i++)
                {
                    out2[i] = inputBuffer[i];
                }
            }
        }
        else
        {
            foreach (AudioSocketComponent output in io.audioOutputs)
            {
                output.zero();
            }
        }

        for (int i = 0; i < io.audioOutputs.Count; i++)
        {
            io.audioOutputs[i].push();
        }



    }
}
