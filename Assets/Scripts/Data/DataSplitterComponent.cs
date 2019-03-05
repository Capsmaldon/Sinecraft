using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSplitterComponent : DataComponent {

    public DataSplitterComponent(int unityBufferSize) : base(unityBufferSize)
    {

    }

    public override void processControl(int controlNum, float value)
    {
        //Doesn't always send with certain socket configurations - look into that
        switch (controlNum)
        {
            case 0:
                foreach(DataSocketComponent dataOutput in io.dataOutputs)
                {
                    dataOutput.set(value);
                    dataOutput.push();
                }
                break;
        }
    }

}
