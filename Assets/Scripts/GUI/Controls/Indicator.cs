using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour {

    private Material offMaterial;
    public Material onMaterial;
    private Renderer indicatorRenderer;
    public int indicatorNum;

    private void Start()
    {
        indicatorRenderer = GetComponent<Renderer>();
        offMaterial = indicatorRenderer.material;
    }

    public void turnIndicatorOn()
    {
        UnityThread.executeInUpdate(() =>
        {
            indicatorRenderer.material = onMaterial;
        });
    }
    public void turnIndicatorOff()
    {
        UnityThread.executeInUpdate(() =>
        {
            indicatorRenderer.material = offMaterial;
        });
    }
}
