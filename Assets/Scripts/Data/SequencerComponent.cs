using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerComponent : DataComponent
{
    private int index;
    private int[] values;
    private int[] states;
    private Indicator[] indicators;
    public bool noteFound;

    public SequencerComponent(int unityBufferSize, Indicator[] indicators) : base(unityBufferSize)
    {
        this.indicators = indicators;
        index = 0;
        noteFound = false;
        values = new int[8];
        states = new int[8];

        for (int i = 0; i < 8; i++)
        {
            values[i] = 36;
            states[i] = 0;
        }
    }

    public int getValue(int index)
    {
        return values[index];
    }
    public int getState(int index)
    {
        return states[index];
    }

    private void tick(float value)
    {
        SequencerComponent prevSequencer = null;
        if (io.dataInputs[0].isConnected())
        {
            prevSequencer = io.dataInputs[0].connectedSocket.adjoinedComponent as SequencerComponent;
        }

        if (prevSequencer != null) noteFound = prevSequencer.noteFound;
        else noteFound = false;

        //If the sequence here isn't complete and no note has already been found
        if (index < 8 && !noteFound)
        {
            updateIndicator();
            if (io.dataOutputs[0].isConnected())
            {
                noteFound = true;
                io.dataOutputs[0].set(values[index] * states[index]);
                io.dataOutputs[0].push();
            }
            index++;
        }
        else
        {
            indicatorOff();
            if (io.dataOutputs[0].isConnected())
            {
                io.dataOutputs[0].set(value);
                io.dataOutputs[0].push();
            }
        }

        SequencerComponent nextSequencer = null;
        if (io.dataOutputs[0].isConnected())
        {
            nextSequencer = io.dataOutputs[0].connectedSocket.adjoinedComponent as SequencerComponent;

        }

        if (index >= 8 && nextSequencer == null)
        {
            resetChain();
        }
    }

    public void reset()
    {
        index = 0;
    }

    public void resetChain()
    {
        reset();
        SequencerComponent prevSequencer = this;

        while (prevSequencer.io.dataInputs[0].isConnected())
        {
            if (prevSequencer.io.dataInputs[0].connectedSocket.adjoinedComponent is SequencerComponent)
            {
                prevSequencer = prevSequencer.io.dataInputs[0].connectedSocket.adjoinedComponent as SequencerComponent;
                prevSequencer.reset();
            }
            else
            {
                break;
            }
        }
    }

    public override void processControl(int controlNum, float value)
    {
        if(controlNum == 0) // Tick socket
        {
            tick(value);
        }

        else if(controlNum > 0 && controlNum < 9) // Value sliders
        {
            values[controlNum - 1] = (int)value;
        }
        else if(controlNum >= 9 && controlNum < 17) // On/off buttons
        {
            states[controlNum - 9] = (int)value;
        }
    }

    private void updateIndicator()
    {
        if (index == 8) indicators[7].turnIndicatorOff();
        else if (index > 0)
        {
            indicators[index].turnIndicatorOn();
            indicators[index - 1].turnIndicatorOff();
        }
        else
        {
            indicators[0].turnIndicatorOn();
            indicators[7].turnIndicatorOff();
        }
    }

    private void indicatorOff()
    {
        indicators[7].turnIndicatorOff();
    }
}
