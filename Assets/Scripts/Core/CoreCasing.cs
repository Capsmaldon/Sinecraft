using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreCasing : MonoBehaviour {

    protected List<Control> controls;
    protected CoreComponent coreComponent;

    protected List<CoreSocketCasing> sockets;
    protected List<AudioInputSocketCasing> audioInputSockets;
    protected List<AudioOutputSocketCasing> audioOutputSockets;
    protected List<DataInputSocketCasing> dataInputSockets;
    protected List<DataOutputSocketCasing> dataOutputSockets;

    protected AudioEngine audioEngine;

    // Use this for initialization
    public void initialise()
    {
        scanInterface();
        audioEngine = (AudioEngine)GameObject.FindObjectOfType(typeof(AudioEngine));
    }

    private void scanInterface()
    {
        controls = new List<Control>();
        audioInputSockets = new List<AudioInputSocketCasing>();
        audioOutputSockets = new List<AudioOutputSocketCasing>();
        dataInputSockets = new List<DataInputSocketCasing>();
        dataOutputSockets = new List<DataOutputSocketCasing>();
        sockets = new List<CoreSocketCasing>();

        //Find every child (every branch, so children of children of children etc.)
        Transform[] childrenArr = this.GetComponentsInChildren<Transform>();

        //If one of the following scripts are found in the children add it to the corresponding list
        foreach (Transform child in childrenArr)
        {
            Control control = child.GetComponent<Control>();
            if (control != null) controls.Add(control);

            AudioInputSocketCasing audioInput = child.GetComponent<AudioInputSocketCasing>();
            if (audioInput != null)
            {
                audioInputSockets.Add(audioInput);
                sockets.Add(audioInput);
            }

            AudioOutputSocketCasing audioOutput = child.GetComponent<AudioOutputSocketCasing>();
            if (audioOutput != null)
            {
                audioOutputSockets.Add(audioOutput);
                sockets.Add(audioOutput);
            }

            DataInputSocketCasing dataInput = child.GetComponent<DataInputSocketCasing>();
            if (dataInput != null) 
            {
                dataInputSockets.Add(dataInput);
                sockets.Add(dataInput);
            }

            DataOutputSocketCasing dataOutput = child.GetComponent<DataOutputSocketCasing>();
            if (dataOutput != null) 
            {
                dataOutputSockets.Add(dataOutput);
                sockets.Add(dataOutput);
            }
        }
    }

    public void move(Vector3 newPosition, Quaternion newRotation)
    {
        transform.position = newPosition;
        transform.rotation = newRotation;
        
        foreach (CoreSocketCasing socket in sockets)
        {
            socket.redrawCable();
        }

        foreach (Control control in controls)
        {
            control.move(newPosition);
        }
    }

    protected void setComponent(CoreComponent component)
    {
        coreComponent = component;
    }

    public CoreComponent getComponent()
    {
        return coreComponent;
    }

    public virtual void delete()
    {
        coreComponent.io.delete();
        foreach (CoreSocketCasing socket in sockets)
        {
            socket.undrawCable();
        }
    }
}
