using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaterialEffect{
    /*
        These probably can just be boolean values, but its a bit more readable at the moment

        The type values can also be moved into an xml file if this method ends up being annoying
    */
    enum EffectValueType { Flat, Modifier }
    enum EffectResultType { Positive, Negative }

    static Color negativeTextColor = Color.red;
    static Color positiveTextColor = Color.green;

    static string ColorToHex(Color color)
    {
        int r = (int)(color.r * 255);
        int g = (int)(color.g * 255);
        int b = (int)(color.b * 255);
        int a = (int)(color.a * 255);
        return string.Format("{0:x2}{1:x2}{2:x2}{3:x2}", r, g, b, a);
    }

    static string[] EffectTypeNames = {
        "Player Health",
        "Monster Health",
        "Objectives"
    };

    static EffectValueType[] EffectValueTypes = {
        EffectValueType.Modifier,
        EffectValueType.Modifier,
        EffectValueType.Flat
    };

    static EffectResultType[] EffectResultTypes = {
        EffectResultType.Positive,
        EffectResultType.Negative,
        EffectResultType.Negative
    };

    public string ReadableType { get { return EffectTypeNames[effectType]; } }
    public string ReadableValue {
        get {
            if (EffectValueTypes[effectType] == EffectValueType.Flat)
                return (effectValue > 0 ? "+" : "-") + effectValue;
            else
            {
                float value = (effectValue - 1) * 100;
                return (value > 0 ? "+" : "") + value + "%";
            }
        }
    }
    public string FormattedValue {
        get {
            return string.Format("<color=#{0}>{1}</color>", ColorToHex(IsPositive ? positiveTextColor : negativeTextColor), ReadableValue);
        }
    }

    public bool IsPositive {
        get {
            bool positiveEffect = EffectResultTypes[effectType] == EffectResultType.Positive;
            if (EffectValueTypes[effectType] == EffectValueType.Flat)
                return positiveEffect == effectValue > 0;
            else return positiveEffect == effectValue > 1;
        }
    }

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
