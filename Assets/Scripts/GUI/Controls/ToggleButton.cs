using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : Control {

    private int state;
    private Vector3 position;
    private Material offMaterial;
    public Material onMaterial;

    public override void initialise()
    {
        position = transform.localPosition;
        offMaterial = transform.parent.GetComponent<Renderer>().material;
    }

    public override void select(Vector3 playerPoint)
    {
        if(state == 0)
        {
            transform.localPosition = new Vector3(position.x, position.y, position.z+0.3f);
            transform.parent.GetComponent<Renderer>().material = onMaterial;
            state = 1;
            setValue(state);
        }
        else if(state == 1)
        {
            transform.localPosition = new Vector3(position.x, position.y, position.z+0.3f);
            transform.parent.GetComponent<Renderer>().material = offMaterial;
            state = 0;
            setValue(state);
        }
    }

    public override void release()
    {
        if(state == 1)
        {
            transform.localPosition = new Vector3(position.x, position.y, position.z + 0.2f);
        }
        else if(state == 0)
        {
            transform.localPosition = new Vector3(position.x, position.y, position.z);
        }
    }
}
