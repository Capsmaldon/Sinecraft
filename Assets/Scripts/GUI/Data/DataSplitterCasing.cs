using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSplitterCasing : DataCasing
{
    void Start()
    {
        base.initialise();
        dataComponent = new DataSplitterComponent(audioEngine.bufferSize);
        base.postInitialse();
    }
}
