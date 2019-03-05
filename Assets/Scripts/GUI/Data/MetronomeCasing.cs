using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeCasing : DataCasing {

	// Use this for initialization
	void Start () {
        base.initialise();
        dataComponent = new MetronomeComponent(audioEngine.bufferSize);
        base.postInitialse();
    }
}
