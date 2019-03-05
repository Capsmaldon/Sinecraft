using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatorCasing : AudioCasing {
   
    public float frequency;

    // Use this for initialization
    void Start () {
        base.initialise();
        //Make the audio component
        audioComponent = new OscillatorComponent(audioEngine.sampleRate, audioEngine.bufferSize, frequency);
        base.postInitialse();
    }
}
