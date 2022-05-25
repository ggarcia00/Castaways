using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Item : MonoBehaviour
{
    public enum Type { 
        Knife, Hammer, Rope
    };

    // Dicionário que guarda a posição que cada item deve ficar na mão do personagem
    Dictionary<Type, Vector3> itemsPositions = new Dictionary<Type, Vector3>();
    Dictionary<Type, Vector3> itemsRotations = new Dictionary<Type, Vector3>();


    public Type itemType;

    Transform formerParent;

    //Cache vars
    BoxCollider bColl;
    Rigidbody rb;
    NavMeshObstacle nMeshObstacle;

    // Start is called before the first frame update
    void Start()
    {
        bColl = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        nMeshObstacle = GetComponent<NavMeshObstacle>();

        itemsPositions.Add(Type.Knife, new Vector3(0, -0.04f, -0.03f));
        itemsRotations.Add(Type.Knife, new Vector3(0, 0, -50));

        itemsPositions.Add(Type.Hammer, new Vector3(0.12f, -0.04f, -0.1f));
        itemsRotations.Add(Type.Hammer, new Vector3(90, 180, -120));

        itemsPositions.Add(Type.Rope, new Vector3(0.09f, -0.3f, -0.15f));
        itemsRotations.Add(Type.Rope, new Vector3(-90, -180, 0));
    }


    public void PickupItem(Transform new_parent)
    {
        formerParent = transform.parent;

        transform.parent = new_parent;
        transform.localPosition = itemsPositions[this.itemType];
        transform.localEulerAngles = itemsRotations[this.itemType];

        bColl.enabled = false;
        rb.isKinematic = true;
        nMeshObstacle.enabled = false;
        

    }

    public void DropItem(Transform dropLocation)
    {
        transform.parent = formerParent;
        transform.position = dropLocation.position;

        bColl.enabled = true;
        rb.isKinematic = false;
        nMeshObstacle.enabled = true;
    }
}
