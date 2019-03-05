using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public int controlNum;
    protected float value = 0;

    public float f_scaling = 1;
    public float f_offsetting;

    private CoreComponent component;

    void Start()
    {
        //Go up parents until an audio component is found
        CoreCasing casing = null;
        GameObject level = this.gameObject;
        component = null;
        while(casing == null)
        {
            if (level.transform.parent != null)
            {
                level = level.transform.parent.gameObject;
            }
            else break;
            casing = level.GetComponent<CoreCasing>();
        }
        if(casing != null)component = casing.getComponent();

        //Run the child's intitialisation
        initialise();
    }

    public virtual void move(Vector3 newPosition){}
    public virtual void initialise() {}
    public virtual void select(Vector3 playerPoint) { }
    public virtual void interact(Vector3 playerPoint) { }
    public virtual void release() { }
    public virtual float getValue() {return value;}
    protected void setValue(float position)
    {
        value = ((position + 1) * 0.5f);
        if (value < 0.0001f) value = 0;
        else if (value > 0.9999) value = 1;

        value *= f_scaling;
        value += f_offsetting;

        sendValue();
    }
    protected void sendValue()
    {
        if (component != null) component.processControl(controlNum, value);
        else print("Control not connected to component - Did you remember to change the script execution order?");
    }
}

