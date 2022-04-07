using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craftable : MonoBehaviour
{
    public GameObject ropes;

    public enum Type { Palm}
    public Type type;

    Dictionary<Item.Type, GameObject> craftables = new Dictionary<Item.Type, GameObject>();


    void Start()
    {
        craftables.Add(Item.Type.Knife, ropes);  
    }


    public void Craft(Item.Type itemType)
    {
        Debug.Log("Craftado");
        Instantiate(craftables[itemType], transform.position + new Vector3(1,1,1), Quaternion.identity);
    }

    public bool HasInteraction(Item.Type type)
    {
        return craftables.ContainsKey(type);
    }
}
