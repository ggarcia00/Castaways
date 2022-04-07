using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate :  MonoBehaviour
{
    List<Item.Type> heldItems;

    public Mesh openCrateMesh;
    public Mesh closeCrateMesh;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public Material material2;

    public bool isOpen;

    void Start()
    {
        heldItems = new List<Item.Type>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        isOpen = true;
    }


    public void StoreItem(Item.Type item)
    {
        heldItems.Add(item);
    }
    
    public void _Debug()
    {
        foreach (Item.Type item in heldItems){
            Debug.Log(item);
        }
    }

    public void OpenClose()
    {
        if (isOpen)
        {
            meshFilter.mesh = closeCrateMesh;
            meshRenderer.materials = new Material[2] {meshRenderer.material, material2 };

        }
        else
        {
            meshFilter.mesh = openCrateMesh;
            meshRenderer.materials = new Material[1] { meshRenderer.material };
        }
        isOpen = !isOpen;
    }
}
