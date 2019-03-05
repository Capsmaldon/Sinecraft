using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplifierCasing : AudioCasing {

	// Use this for initialization
	void Start () 
    {
        initialise();
        //Make the audio component
        audioComponent = new Amplifier(audioEngine.sampleRate, audioEngine.bufferSize);
        postInitialse();
    }
}