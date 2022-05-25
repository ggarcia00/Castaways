using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Needed items for level completion")]
    [Range(0, 5)]
    public uint Knives;
    [Range(0, 5)]
    public uint Hammers;
    [Range(0, 5)]
    public uint Ropes;

    [Header("Position of Raft Spawn")]
    public Transform raftCraftLocation;

    public GameObject raftPrefab;

    [Header("Crate of the level")] public Crate crate;


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

    public bool CheckItems()
    {
        int knife, hammer, rope;
        crate.GetItems(out knife, out hammer, out rope);

        return Knives == knife && Hammers == hammer && Ropes == rope;
    }

}
