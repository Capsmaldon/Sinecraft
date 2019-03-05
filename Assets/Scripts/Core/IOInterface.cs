using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOInterface
{
    CoreComponent adjoinedComponent;
    private int bufferLength;

    public List<CoreSocketComponent> sockets;

    //Reference to connected components
    public List<AudioSocketComponent> audioInputs;
    public List<AudioSocketComponent> audioOutputs;
    public List<DataSocketComponent> dataInputs;
    public List<DataSocketComponent> dataOutputs;

    public IOInterface(int bufferLength, CoreComponent adjoinedComponent)
    {
        this.adjoinedComponent = adjoinedComponent;
        //Core constants for all IO Interfaces
        this.bufferLength = bufferLength;

        //Initialise lists
        sockets = new List<CoreSocketComponent>();

        audioInputs = new List<AudioSocketComponent>();
        audioOutputs = new List<AudioSocketComponent>();
        dataInputs = new List<DataSocketComponent>();
        dataOutputs = new List<DataSocketComponent>();
    }

    //Configure i/o of block - Just tell the component how many inputs and outputs it has
    public void configureIO(int numOfAudioInputs, int numOfAudioOutputs, int numOfDataInputs, int numOfDataOutputs)
    {
        for (int i = 0; i < numOfAudioInputs; i++)
        {
            audioInputs.Add(new AudioSocketComponent(bufferLength, adjoinedComponent));
            sockets.Add(audioInputs[i]);
        }

        for (int i = 0; i < numOfAudioOutputs; i++)
        {
            audioOutputs.Add(new AudioSocketComponent(bufferLength, adjoinedComponent));
            sockets.Add(audioOutputs[i]);
        }

        for (int i = 0; i < numOfDataInputs; i++)
        {
            dataInputs.Add(new DataSocketComponent(adjoinedComponent));
            sockets.Add(dataInputs[i]);
        }

        for (int i = 0; i < numOfDataOutputs; i++)
        {
            dataOutputs.Add(new DataSocketComponent(adjoinedComponent));
            sockets.Add(dataOutputs[i]);
        }
    }

    //Connect components to eachother - called in audio engine
    public void createConnection(CoreComponent input, CoreComponent output, CoreSocketCasing inputSocket, CoreSocketCasing outputSocket)
    {
        if(inputSocket is AudioInputSocketCasing && outputSocket is AudioOutputSocketCasing)
        {
            AudioSocketComponent inSocComp = audioInputs[inputSocket.getSocketNum()];
            AudioSocketComponent outSocComp = output.io.audioOutputs[outputSocket.getSocketNum()];

            inSocComp.connect(outSocComp);
            inSocComp.setSocketNum(inputSocket.getSocketNum());

            outSocComp.connect(inSocComp);
            outSocComp.setSocketNum(outputSocket.getSocketNum());
        }
        else if(inputSocket is DataInputSocketCasing && outputSocket is DataOutputSocketCasing)
        {
            DataSocketComponent inSocComp = dataInputs[inputSocket.getSocketNum()];
            DataSocketComponent outSocComp = output.io.dataOutputs[outputSocket.getSocketNum()];

            DataSocketCasing dataInputSocket = inputSocket as DataSocketCasing;
            DataSocketCasing dataOutputSocket = outputSocket as DataSocketCasing;

            inSocComp.connect(outSocComp);
            inSocComp.setSocketNum(dataInputSocket.getSocketNum());
            inSocComp.setControlNum(dataInputSocket.getControlNum());

            outSocComp.connect(inSocComp);
            outSocComp.setSocketNum(dataOutputSocket.getSocketNum());
            outSocComp.setControlNum(dataOutputSocket.getControlNum());
        }
    }

    //Disconnect sockets
    public void disconnect(CoreSocketCasing socket)
    {
        if(socket is AudioInputSocketCasing)
        {
            audioInputs[socket.getSocketNum()].disconnect();
        }
        else if(socket is AudioOutputSocketCasing)
        {
            audioOutputs[socket.getSocketNum()].disconnect();
        }
        else if(socket is DataInputSocketCasing)
        {
            dataInputs[socket.getSocketNum()].disconnect();
        }
        else if (socket is DataOutputSocketCasing)
        {
            dataOutputs[socket.getSocketNum()].disconnect();
        }
    }

    //Use as condition of a while loop to pull each buffer from the list of input socket numbers provided
    //TODO: Improve efficiency, constantly pushing and popping from a list is inefficient
    public bool pollInputs(out float[] buffer, List<int> socketNums)
    {
        buffer = null;

        while (socketNums.Count > 0)
        {
            buffer = audioInputs[socketNums[0]].getBuffer(); // Grab the input socket corresponding the first list number
            socketNums.RemoveAt(0); // Remove the used socket number from the list
            if(buffer != null)return true; // Final checking incase the last stretch of sockets is all null
        }
        return false; // End of the list so stop polling    
    }

    public void delete()
    {
        foreach(CoreSocketComponent socket in sockets)
        {
            socket.disconnect();
        }
    }
}
