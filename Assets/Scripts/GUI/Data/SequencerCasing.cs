using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerCasing : DataCasing {

	// Use this for initialization
	void Start () {

        Indicator[] indicators;

        indicators = GetComponentsInChildren<Indicator>();

        System.Array.Sort(indicators, delegate (Indicator ind1, Indicator ind2) { return ind1.indicatorNum.CompareTo(ind2.indicatorNum); });

        base.initialise();
        dataComponent = new SequencerComponent(audioEngine.bufferSize, indicators);
        base.postInitialse();
    }
}
