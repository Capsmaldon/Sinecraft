using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Global enumeration
public class AVal
{
    public const int INPUT = 0;
    public const int OUTPUT = 1;
    public const int LEFT = 0;
    public const int RIGHT = 1;
}

[RequireComponent(typeof(AudioSource))]
public class AudioEngine : MonoBehaviour {

    public int sampleRate;
    public int bufferSize;

    private List<AudioComponent> audioComponents;
    private List<Speaker> speakers;

    private bool muted = false;

    void Start()
    {
        AudioConfiguration settings;
        settings.sampleRate = sampleRate;
        settings.dspBufferSize = bufferSize;

        AudioConfiguration actualSettings = AudioSettings.GetConfiguration();
        Debug.Log(actualSettings.speakerMode);

        //Initialise the AudioComponent list
        audioComponents = new List<AudioComponent>();
        speakers = new List<Speaker>();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        UnityThread.initUnityThread();
    }

    //Connect two components sockets together -- Receiving Socket --> Sending Socket
    public void createConnection(CoreSocketCasing socket1, CoreSocketCasing socket2)
    {
        //Interprets the correct way around - input->output
        CoreComponent input = null, output = null;
         CoreSocketCasing inSoc = null, outSoc = null;

        if (socket1 is AudioInputSocketCasing || socket1 is DataInputSocketCasing)
        {
            input = socket1.GetComponentInParent<CoreCasing>().getComponent();
            output = socket2.GetComponentInParent<CoreCasing>().getComponent();

            inSoc = socket1;
            outSoc = socket2;
        }
        else if(socket1 is AudioOutputSocketCasing || socket1 is DataOutputSocketCasing)
        {
            input = socket2.GetComponentInParent<CoreCasing>().getComponent();
            output = socket1.GetComponentInParent<CoreCasing>().getComponent();

            inSoc = socket2;
            outSoc = socket1;
        }
        else return;

        input.io.createConnection(input, output, inSoc, outSoc);

        socket1.drawCable(socket2);

        print("Connection made");
    }

    public void destroyConnection(CoreSocketCasing socket)
    {
        socket.undrawCable();
        socket.GetComponentInParent<CoreCasing>().getComponent().io.disconnect(socket);
        print("Connection unmade");
    }

    public void addComponent(AudioComponent audioComponent)
    {
        audioComponents.Add(audioComponent);
        if (audioComponent is Speaker) speakers.Add(audioComponent as Speaker);
    }

    public void removeComponent(AudioComponent audioComponent)
    {
        audioComponents.Remove(audioComponent);
        if (audioComponent is Speaker) speakers.Remove(audioComponent as Speaker);
    }

    public void addSpeaker(Speaker speaker)
    {
        speakers.Add(speaker);
    }

    public void pause() { muted = true; }
    public void play() { muted = false; }

    private void OnAudioFilterRead(float[] outputBuffer, int channels)
    {
        //Don't do anything if it's muted
        if (muted) return;

        //Tick every component in order
        foreach(AudioComponent audioComponent in audioComponents)
        {
            audioComponent.tickBuffer();
        }

        //Mix and output all signals sent to speaker objects
        foreach (Speaker speaker in speakers)
        {
            speaker.tickBuffer();
            float[] speakerBufferL = speaker.io.audioInputs[AVal.LEFT].getBuffer();
            float[] speakerBufferR = speaker.io.audioInputs[AVal.RIGHT].getBuffer();

            if(speakerBufferL != null)
            {
                //The output buffer reference cannot be changed - so each value must be assigned explicitly
                for (int n = 0; n < bufferSize; n++)
                {
                    //Outputbuffer = 2N (Interlaced), SpeakerBuffer = N
                    outputBuffer[n * 2] += speakerBufferL[n];
                }
            }
            
            if(speakerBufferR != null)
            {
                for (int n = 0; n < bufferSize; n++)
                {
                    outputBuffer[(n * 2) + 1] += speakerBufferR[n];
                }
            }
        }
    }
}