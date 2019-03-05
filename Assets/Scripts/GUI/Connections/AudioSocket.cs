using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSocket : MonoBehaviour {

    public Material selectedSocketMaterial;
    public Material unselectedSocketMaterial;
    public Material cableMaterial;

	// Use this for initialization
	void Start () {
		
	}

    public void select()
    {
        GetComponent<Renderer>().material = selectedSocketMaterial;
    }

    public void deselect()
    {
        GetComponent<Renderer>().material = unselectedSocketMaterial;
    }

    public virtual void makeAudioConnection(AudioSocketOutput outputSocket, AudioSocketInput inputSocket) {}
    public virtual void severAudioConnection(AudioSocketInput inputSocket) {}
}

public class AudioSocketOutput : AudioSocket
{

    // Use this for initialization
    public void initialise()
    {

    }
}

public class AudioSocketInput : AudioSocket
{
    // Use this for initialization
    public void initialise()
    {

    }
}
