using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWave
{
    private float sinePhaseIncrement;
    private float sinePhase;
    private float frequency;
    private float sampleTime;

    public SineWave(int unitySampleRate, int unityBufferSize, float frequency)
    {
        this.frequency = frequency;
        sampleTime = 1.0f / unitySampleRate;
        sinePhaseIncrement = this.frequency * sampleTime;
    }

    public void setFreqency(float frequency)
    {
        this.frequency = frequency;
        sinePhaseIncrement = this.frequency * sampleTime;
    }

    public void processBuffer(float[] buffer, float[] freqBuffer)
    {
        if (freqBuffer == null)
        {
            for (int n = 0; n < buffer.Length; n++)
            {
                //Calculate a sine wave
                float sine = Mathf.Sin(sinePhase * 2.0f * Mathf.PI);
                sinePhaseIncrement = this.frequency * sampleTime;
                sinePhase += sinePhaseIncrement;
                if (sinePhase > 1.0f) sinePhase -= 1.0f;
                else if (sinePhase < 0) sinePhase += 1.0f;
                buffer[n] = sine;
            }
        }
        else
        {
            for (int n = 0; n < buffer.Length; n++)
            {
                //Calculate a sine wave
                float sine = Mathf.Sin(sinePhase * 2.0f * Mathf.PI);
                sinePhaseIncrement = (frequency + freqBuffer[n]) * sampleTime;
                sinePhase += sinePhaseIncrement;
                if (sinePhase > 1.0f) sinePhase -= 1.0f;
                else if (sinePhase < 0) sinePhase += 1.0f;
                buffer[n] = sine;
            }
        }
    }
}
