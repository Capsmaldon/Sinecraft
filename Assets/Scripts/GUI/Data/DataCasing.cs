using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Is currently generic but will be expanded to include sequencers/metronomes
public class DataCasing : CoreCasing {

    protected DataComponent dataComponent;

    protected new void initialise()
    {
        base.initialise();
    }

    protected void postInitialse()
    {
        setComponent(dataComponent);
        configureIO();
    }

    private void configureIO()
    {
        dataComponent.io.configureIO(audioInputSockets.Count, audioOutputSockets.Count, dataInputSockets.Count, dataOutputSockets.Count);
    }
}
