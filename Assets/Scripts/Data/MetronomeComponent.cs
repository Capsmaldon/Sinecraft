using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class MetronomeComponent : DataComponent {

    public System.Timers.Timer clock;

    public MetronomeComponent(int unityBufferSize) : base(unityBufferSize)
    {
        clock = new System.Timers.Timer(1000);
        clock.Elapsed += Clock_Elapsed;
        clock.Start();
    }

    void Clock_Elapsed(object sender, ElapsedEventArgs e)
    {
        processControl(1, 1);
    }

    public override void processControl(int controlNum, float value)
    {
        switch(controlNum)
        {
            case 0:
                clock.Interval = (1.0f-value) * 2000.0f;
                break;
            case 1:
                if (!io.dataOutputs[0].isConnected()) return;
                io.dataOutputs[0].set(value);
                io.dataOutputs[0].push();
                break;
        }
    }
}
