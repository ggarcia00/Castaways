using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public enum Items {
        Knives, Hammers, Ropes
    }


    void Start()
    {
        //Items needed to win the level / get from outside
        List<Items> neededItems = new List<Items>();
        neededItems.Add(Items.Ropes);
        neededItems.Add(Items.Hammers);
        foreach( Items item in neededItems)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
