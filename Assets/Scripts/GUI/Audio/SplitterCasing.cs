using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterCasing : AudioCasing {

    // Use this for initialization
    void Start()
    {
        initialise();
        //Make the audio component
        audioComponent = new Splitter(audioEngine.sampleRate, audioEngine.bufferSize);
        postInitialse();
    }
}
