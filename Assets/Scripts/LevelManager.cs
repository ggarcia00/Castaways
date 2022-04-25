using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Needed items for level completion")]
    [Range(0, 5)]
    public uint Knives;
    [Range(0, 5)]
    public uint Hammers;
    [Range(0, 5)]
    public uint Ropes;



    // Singleton
    [HideInInspector]
    public static LevelManager instance { get; private set;}


    void Awake()
    {
        if( instance == null)
        {
            instance = this;
        }else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        UIController.instance.SetRequiredItems(Knives, Hammers, Ropes);
    }

}
