using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer : AudioComponent {

    private float outputVolume;
    private float[] channelVolumes;

    public Mixer(int unitySampleRate, int unityBufferSize) : base(unitySampleRate, unityBufferSize)
    {
        outputVolume = 0.5f;
        channelVolumes = new float[4]; // Number of channels is hard coded here, must match with inputs and controls
        for (int i = 0; i < 4; i++)
        {
            channelVolumes[i] = 0.5f;
        }

    }

    protected override void processBuffer()
    {
        bool firstConnectedSocketFound = false;
        float[] output = io.audioOutputs[0].getBuffer();

        if(output != null)
        {
            for (int i = 0; i < io.audioInputs.Count; i++)
            {
                AudioSocketComponent input = io.audioInputs[i];

                float[] tempBuffer = input.getBuffer();

                if (tempBuffer != null)
                {
                    if (!firstConnectedSocketFound)
                    {
                        firstConnectedSocketFound = true;
                        tempBuffer.CopyTo(output, 0);

                        for (int n = 0; n < bufferLength; n++)
                        {
                            output[n] *= channelVolumes[i];
                        }

                    }
                    else
                    {
                        for (int n = 0; n < bufferLength; n++)
                        {
                            output[n] += tempBuffer[n] * channelVolumes[i];
                        }
                    }
                }
            }

            for (int n = 0; n < bufferLength; n++)
            {
                output[n] *= outputVolume;
            }

            if (!firstConnectedSocketFound)
            {
                for (int n = 0; n < bufferLength; n++)
                {
                    output[n] = 0;
                }
            }
            io.audioOutputs[0].setBuffer(output);
        }
        io.audioOutputs[0].push();
    }

    public override void processControl(int controlNum, float value)
    {
        channelVolumes[controlNum] = value;
    }
}
