using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[XmlParse("Effect")]
public class MaterialEffect{
    /*
        These probably can just be boolean values, but its a bit more readable at the moment

        The type values can also be moved into an xml file if this method ends up being annoying
    */
    public enum EffectValueType { Flat, Multiplier }
    
    public struct EffectType
    {
        public EffectType(string name, bool positive)
        {
            Name = name;
            Positive = positive;
        }

        public string Name;
        public bool Positive;
    }

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

    static EffectType[] EffectTypes = {
        // Player Effects
        new EffectType("Player Health", true),      //0
        new EffectType("Player Speed", true),       //1
        new EffectType("Player Damage", true),      //2
        new EffectType("Player Lives", true),       //3
        new EffectType("Player Size", false),       //4
        new EffectType("Attack Speed", true),       //5

        // Monster Effects
        new EffectType("Monster Health", false),    //6
        new EffectType("Monster Speed", false),     //7
        new EffectType("Monster Damage", false),    //8
        new EffectType("Monster Size", false),      //9
        new EffectType("Monster Count", false),     //10

        // Trial Effects
        new EffectType("Time Limit", true),         //11
        new EffectType("Objectives", false),        //12
        new EffectType("Objects", false),           //13
    };

    public string ReadableType { get { return EffectTypes[Type].Name; } }
    public string ReadableValue {
        get {
            if (ValueType == EffectValueType.Flat)
                return (Value > 0 ? "+" : "") + Value;
            else
            {
                float value = (Value - 1) * 100;
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
            return EffectTypes[Type].Positive == Value > (ValueType == EffectValueType.Flat ? 0 : 1);
        }
    }

    [XmlParse("Type")]
    public int Type { get; private set; }

    [XmlParse("ValueType")]
    public EffectValueType ValueType { get; private set; }

    [XmlParse("Value")]
    public float Value { get; private set; }

    public string toString()
    {
        return string.Format("{0}\n{1}\n{2}", ReadableType, ValueType, Value);
    }

    public void ApplyEffect()
    {

    }

    public void RemoveEffect()
    {

    }
}
