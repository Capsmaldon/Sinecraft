using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatorComponent : AudioComponent{

    int state = 1;
    private SineWave sine;
    private SawWave saw;
    private float frequency;
    private int wavetype;

    bool autoTune = true;
    int key = 0;

    public OscillatorComponent(int unitySampleRate, int unityBufferSize, float frequency) : base(unitySampleRate, unityBufferSize)
    {
        sine = new SineWave(unitySampleRate, unityBufferSize, frequency);
        saw = new SawWave(unitySampleRate, unityBufferSize, frequency);
    }

    public void setWavetype(int wavetype)
    {
        this.wavetype = wavetype;
    }

    protected override void processBuffer()
    {
        float[] input = io.audioInputs[0].getBuffer();
        float[] output = io.audioOutputs[0].getBuffer();

        if (output != null)
        {
            if (state == 0)
            {
                //Very crude for now - when a midi note of zero is requested:
                io.audioOutputs[0].zero();
                io.audioOutputs[0].push();
                //Does goes some nasty glitches for sine waves
                return;
            }

            switch (wavetype)
            {
                case 0:
                    sine.processBuffer(output, input);
                    break;
                case 1:
                    saw.processBuffer(output, input);
                    break;
            }
        }
        io.audioOutputs[0].push();
    }

    public override void processControl(int controlNum, float value)
    {
        switch(controlNum)
        {
            case 0:
                state = 1;
                sine.setFreqency(value);
                saw.setFreqency(value);
                break;
            case 1:
                wavetype = (int)value;
                break;
            case 2:
                if (value > -0.0001f && value < 0.0001f)
                {
                    state = 0;
                }
                else state = 1;

                int note = fitToKey(value);
                if (!autoTune) note = (int)value;

                sine.setFreqency(mtof(note));
                saw.setFreqency(mtof(note));
                break;
            case 3:
                toggleAutoTune(value);
                break;
            case 4:
                setKey(value);
                break;
        }
    }

    private float mtof(int midiNote)
    {
        return 440.0f * Mathf.Pow(2.0f, (midiNote - 69.0f) / 12.0f);
    }

    private void setKey(float value)
    {
        key = (int)(value + 0.2f);
    }

    private int fitToKey(float value)
    {
        int note = (int) value;
        int interval = note % 12;

        int[] majorKey = new int[5];
        majorKey[0] = 1;
        majorKey[1] = 3;
        majorKey[2] = 6;
        majorKey[3] = 8;
        majorKey[4] = 10;

        for (int i = 0; i < majorKey.Length; i++)
        {
            if (interval == majorKey[i])
            {
                note++;
                break;
            }
        }

        return note + key;
    }

    private void toggleAutoTune(float value)
    {
        if ((int)(value + 0.1f) == 1) autoTune = false;
        else autoTune = true;
    }
}
