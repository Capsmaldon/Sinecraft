using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : Control
{
    private Plane squarePlane;
    private float scalingFactor;

    public override void initialise()
    {
        scalingFactor = 1/transform.localPosition.x; // Assumes zero position

        squarePlane = new Plane(transform.forward, transform.position);
    }

    public override void move(Vector3 newPosition)
    {
        squarePlane.SetNormalAndPosition(transform.forward, transform.position);
    }

    public override void select(Vector3 playerPoint)
    {

    }

    public override void interact(Vector3 playerPoint)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        float dist;
        if (squarePlane.Raycast(ray, out dist))
        {
            Vector3 hitPoint = ray.GetPoint(dist);
            transform.position = hitPoint;

            transform.localPosition = new Vector3(transform.localPosition.x, 0, -0.5f);

            if(transform.localPosition.x > 0.39f)
            {
                transform.localPosition = new Vector3(0.39f, transform.localPosition.y, transform.localPosition.z);
            }
            else if(transform.localPosition.x < -0.39f)
            {
                transform.localPosition = new Vector3(-0.39f, transform.localPosition.y, transform.localPosition.z);
            }

            setValue((transform.localPosition.x * -1) * scalingFactor);
        }
    }
}

