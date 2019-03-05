using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawWave{

    private float sawPhaseIncrement;
    private float sawPhase;
    private float frequency;
    private float twoPI;
    private float sampleTime;

    public SawWave(int unitySampleRate, int unityBufferSize, float frequency)
    {
        this.frequency = frequency;
        sampleTime = Mathf.PI / unitySampleRate;
        sawPhaseIncrement = frequency * 2 * Mathf.PI/unitySampleRate;
        twoPI = Mathf.PI * 2.0f;
    }

    public void setFreqency(float frequency)
    {
        this.frequency = frequency;
        sawPhaseIncrement = (this.frequency) * sampleTime * 2.0f;
    }

    public void processBuffer(float[] buffer, float[] freqBuffer)
    {
        if (freqBuffer == null)
        {
            for (int n = 0; n < buffer.Length; n++)
            {
                //Calculate a saw wave
                float saw = (2.0f * sawPhase / twoPI) - 1.0f;
                saw -= polyBLEP();
                buffer[n] = saw;
                sawPhase += sawPhaseIncrement;
                if (sawPhase >= twoPI) sawPhase -= twoPI;
                else if (sawPhase < 0) sawPhase += twoPI;
            }
        }
        else
        {
            for (int n = 0; n < buffer.Length; n++)
            {
                //Calculate a saw wave
                float saw = (2.0f * sawPhase / twoPI) - 1.0f;
                saw -= polyBLEP();
                buffer[n] = saw;
                sawPhaseIncrement = (frequency + freqBuffer[n]) * sampleTime * 2.0f;
                sawPhase += sawPhaseIncrement;
                if (sawPhase >= twoPI) sawPhase -= twoPI;
                else if (sawPhase < 0) sawPhase += twoPI;
            }
        }   
    }

    //Taken from http://www.martin-finke.de/blog/articles/audio-plugins-018-polyblep-oscillator/
    private float polyBLEP()
    {
        double t = sawPhase / twoPI; // Time
        double dt = sawPhaseIncrement / twoPI; // DeltaTime

        if(t < dt) // If time exceeds delta time
        {
            t /= dt;
            double intermediate = t + t - t * t - 1.0;
            return (float)intermediate;
        }
        else if(t > 1.0f - dt)
        {
            t = (t - 1.0f) / dt;
            double intermediate = t * t + t + t + 1.0;
            return (float)intermediate;
        }
        else
        {
            return 0;
        }
    }
}
