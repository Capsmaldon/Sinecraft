using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerCasing : AudioCasing {

	// Use this for initialization
	void Start () {
        initialise();
        audioComponent = new Speaker(audioEngine.sampleRate, audioEngine.bufferSize);
        postInitialse();
    }
}
