using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCasing : CoreCasing {

    protected AudioComponent audioComponent;

    // Use this for initialization
    protected new void initialise () 
    {
        base.initialise();
    }

    protected void postInitialse()
    {
        audioEngine.addComponent(audioComponent);
        setComponent(audioComponent);
        configureIO();
    }

    protected void configureIO()
    {
        audioComponent.io.configureIO(audioInputSockets.Count, audioOutputSockets.Count, dataInputSockets.Count, dataOutputSockets.Count);
    }

    public AudioComponent getAudioComponent()
    {
        return audioComponent;
    }

    public override void delete()
    {
        base.delete();
        audioEngine.removeComponent(audioComponent);
    }
}
