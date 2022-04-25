using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{

    public Material mat;
    private new Renderer renderer;
    private Material oldMat;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        oldMat = renderer.material;
    }

    void OnMouseEnter()
    {
        renderer.material = mat;
    }

    void OnMouseExit()
    {
        renderer.material = oldMat;    
    }


}
