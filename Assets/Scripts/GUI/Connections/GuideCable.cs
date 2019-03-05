using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GuideCable : MonoBehaviour{

    public Material guideCableMaterial;

    LineRenderer lineRenderer;
    private Camera playerCamera;
    private float f_interactionDistance;
    private int layerMask;
    private bool drawing = false;

    void Start()
    {
        lineRenderer = this.transform.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.sharedMaterial = guideCableMaterial;
    }

    public void initialise(Camera playerCamera, float f_interactionDistance, int layerMask)
    {
        this.playerCamera = playerCamera;
        this.f_interactionDistance = f_interactionDistance;
        this.layerMask = layerMask;
    }

    public void startDrawing()
    {
        drawing = true;
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, f_interactionDistance, layerMask))
        {
            if(hit.transform.gameObject.layer == LAYER.SOCKET)
            {
                CoreSocketCasing socketHit = hit.transform.GetComponent<CoreSocketCasing>();

                lineRenderer = this.gameObject.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, socketHit.transform.position);
            }
        }

    }
	
    public void draw()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, f_interactionDistance, layerMask))
        {
            if (hit.transform.gameObject.layer == LAYER.SOCKET)
            {
                lineRenderer.SetPosition(1, hit.transform.position);
            }
            else
            {
                lineRenderer.SetPosition(1, hit.point);
            }
                
        }
        else lineRenderer.SetPosition(1, playerCamera.transform.position + playerCamera.transform.forward * 2);

    }

    public bool isDrawing()
    {
       return drawing;
    }

    public void release()
    {
        drawing = false;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
    }
}

