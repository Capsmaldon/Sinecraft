using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : Control
{
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
        transform.localPosition = new Vector3(position.x, position.y, position.z + 0.3f);
        transform.parent.GetComponent<Renderer>().material = onMaterial;
        setValue(1);
    }

    public override void release()
    {
        transform.localPosition = new Vector3(position.x, position.y, position.z);
        transform.parent.GetComponent<Renderer>().material = offMaterial;
    }
}
