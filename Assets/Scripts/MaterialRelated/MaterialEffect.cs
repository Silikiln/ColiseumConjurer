using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using EffectDescription = EffectDescriptions.EffectDescription;

[XmlParse("Effect")]
public class MaterialEffect{
    public enum EffectValueType { Flat, Multiplier }

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



    public string ReadableType { get { return Effect.GetDisplayName(); } }
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
            return Effect.IsPositive() == Value > (ValueType == EffectValueType.Flat ? 0 : 1);
        }
    }

    [XmlParse("Type")]
    public EffectDescription Effect { get; private set; }

    [XmlParse("ValueType")]
    public EffectValueType ValueType { get; private set; }

    [XmlParse("Value")]
    public float Value { get; private set; }

    public string toString()
    {
        return string.Format("{0}\n{1}\n{2}", ReadableType, ValueType, Value);
    }

    void ApplyEffect(ref float flatValue, ref float multiplierValue)
    {
        if (ValueType == EffectValueType.Flat)
            flatValue += Value;
        else
            multiplierValue *= Value;
    }

    void RemoveEffect(ref float flatValue, ref float multiplierValue)
    {
        if (ValueType == EffectValueType.Flat)
            flatValue -= Value;
        else
            multiplierValue /= Value;
    }

    void EffectAction(ref float flatValue, ref float multiplierValue, bool apply)
    {
        if (apply)
            ApplyEffect(ref flatValue, ref multiplierValue);
        else
            RemoveEffect(ref flatValue, ref multiplierValue);
    }

    void PassEffectValues(bool apply)
    {
        switch (Effect)
        {
            // Player Effects
            case EffectDescription.PlayerHealth:
                EffectAction(ref PlayerController.PlayerHealthAdded, ref PlayerController.PlayerHealthMultiplier, apply);
                break;
            case EffectDescription.PlayerSpeed:
                break;
            case EffectDescription.PlayerDamage:
                break;
            case EffectDescription.PlayerLives:
                break;
            case EffectDescription.PlayerSize:
                break;
            case EffectDescription.PlayerAttackSpeed:
                break;

            // Monster Effects
            case EffectDescription.MonsterHealth:
                break;
            case EffectDescription.MonsterSpeed:
                break;
            case EffectDescription.MonsterDamage:
                break;
            case EffectDescription.MonsterSize:
                break;
            case EffectDescription.MonsterCount:
                break;

            // Trial Effects
            case EffectDescription.TimeLimit:
                break;
            case EffectDescription.ObjectiveCount:
                break;
            case EffectDescription.ObjectCount:
                break;

        }
    }

    public void RemoveEffect()
    {

    }
}
