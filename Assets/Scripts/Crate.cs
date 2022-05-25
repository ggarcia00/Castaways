using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crate :  MonoBehaviour
{
    List<Item.Type> heldItems;

    public Mesh openCrateMesh;
    public Mesh closeCrateMesh;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public Material material2;

    public bool isOpen;

    Transform formerParent;

    private Rigidbody rb;
    private BoxCollider bColl;
    private NavMeshObstacle nMeshObstacle;

    private bool isThrowing;

    public GameObject hammerPrefab;
    public GameObject knifePrefab;
    public GameObject ropePrefab;

    void Start()
    {
        heldItems = new List<Item.Type>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        bColl = GetComponent<BoxCollider>();
        nMeshObstacle = GetComponent<NavMeshObstacle>();
        isOpen = true;
        isThrowing = false;
    }


    public void StoreItem(Item.Type item)
    {
        heldItems.Add(item);
    }

    public void retrieveItems()
    {
        foreach(Item.Type item in heldItems){
            switch (item)
            {
                case Item.Type.Knife:
                    Instantiate(knifePrefab, transform.position + new Vector3(1, 1, 0), Quaternion.identity);
                    break;
                case Item.Type.Hammer:
                    Instantiate(hammerPrefab, transform.position + new Vector3(0, 1, -1), Quaternion.identity);
                    break;
                case Item.Type.Rope:
                    Instantiate(ropePrefab, transform.position + new Vector3(-1, 1, 0), Quaternion.identity);
                    break;
            }
        }

        heldItems.Clear();
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
            retrieveItems();

        }
        isOpen = !isOpen;
        
    }

    public void PickUp(Transform new_parent)
    {
        formerParent = transform.parent;

        transform.parent = new_parent;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;

        bColl.enabled = false;
        rb.isKinematic = true;
        nMeshObstacle.enabled = false;
    }

    public void Drop()
    {
        transform.parent = formerParent;

        bColl.enabled = true;
        rb.isKinematic = false;
        nMeshObstacle.enabled = true;
    }


    public void Throw(Vector3 target)
    {
        if (!isThrowing)
        {
            StartCoroutine(ThrowImpl(target));
        }
    }


    private IEnumerator ThrowImpl(Vector3 target)
    {
        Vector3 original = transform.position;
        float progress = 0;
        float height = 2;

        isThrowing = true;
        rb.isKinematic = true;
        bColl.enabled = false;
        nMeshObstacle.enabled = false;

        while (progress <= 1)
        {
            progress += Time.deltaTime;
            transform.position = Vector3.Slerp(original, target, progress);
            transform.position = new Vector3(transform.position.x, target.y + Mathf.Sin(progress * 2) * height, transform.position.z);
            yield return null;
        }
        Debug.Log("Objetivo");
        isThrowing = false;
        rb.isKinematic = false;
        bColl.enabled = true;
        nMeshObstacle.enabled = true;
        yield break;
    }

    public void GetItems(out int knife, out int hammer, out int rope)
    {
        knife = hammer = rope = 0;
        
        foreach (Item.Type item in heldItems)
        {
            switch (item)
            {
                case Item.Type.Knife:
                    knife++;
                    break;
                
                case Item.Type.Hammer:
                    hammer++;
                    break;
                
                case Item.Type.Rope:
                    rope++;
                    break;
                        
                default:
                    break;
            }
        }
    }
}
