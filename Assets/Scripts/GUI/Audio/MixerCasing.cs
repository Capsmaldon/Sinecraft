﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerCasing : AudioCasing
{
    // Use this for initialization
    void Start()
    {
        initialise();
        //Make the audio component
        audioComponent = new Mixer(audioEngine.sampleRate, audioEngine.bufferSize);
        postInitialse();
    }
}
