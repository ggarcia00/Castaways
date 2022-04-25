using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Texture iconKnife;
    public Texture iconHammer;
    public Texture iconRope;

    public RawImage icon1;
    public RawImage icon2;

    public TextMeshProUGUI iconQuantity1;
    public TextMeshProUGUI iconQuantity2;

    // Singleton
    [HideInInspector]
    public static UIController instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void SetRequiredItems(uint Knives, uint Hammers, uint Ropes)
    {
        if (Ropes > 0)
        {
            icon1.texture = iconRope;
            iconQuantity1.text = Ropes.ToString();
        }
        if (Hammers > 0)
        {
            icon2.texture = iconHammer;
            iconQuantity2.text = Hammers.ToString();
        }
    }
}
