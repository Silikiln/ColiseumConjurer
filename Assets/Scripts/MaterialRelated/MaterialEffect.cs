using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaterialEffect{

    public int effectType { get; set; }
    public float effectValue { get; set; }

    public string toString()
    {
        string requestedString = "Type:" + effectType + " Value: " + effectValue;
        return requestedString;
    }

    public void ApplyEffect()
    {

    }

    public void RemoveEffect()
    {

    }
}
