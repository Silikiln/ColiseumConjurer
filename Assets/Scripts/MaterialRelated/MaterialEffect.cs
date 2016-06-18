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
                EffectAction(ref PlayerController.PlayerMoveSpeedAdded, ref PlayerController.PlayerMoveSpeedMultiplier, apply);
                break;
            case EffectDescription.PlayerDamage:
                EffectAction(ref PlayerController.PlayerDamageDealtAdded, ref PlayerController.PlayerDamageDealtMultiplier, apply);
                break;
            case EffectDescription.PlayerLives:
                if (apply)
                {
                    if (ValueType == EffectValueType.Flat)
                        PlayerController.Lives += (int)Value;
                    else
                        PlayerController.Lives = (int)(PlayerController.Lives * Value);
                }
                break;
            case EffectDescription.PlayerSize:
                EffectAction(ref PlayerController.PlayerSizeAdded, ref PlayerController.PlayerSizeMultiplier, apply);
                break;
            case EffectDescription.PlayerAttackSpeed:
                EffectAction(ref PlayerController.PlayerAttackSpeedAdded, ref PlayerController.PlayerAttackSpeedMultiplier, apply);
                break;
            case EffectDescription.PlayerMaxSpeed:
                EffectAction(ref PlayerController.PlayerMaxSpeedAdded, ref PlayerController.PlayerMaxSpeedMultiplier, apply);
                break;

            // Monster Effects
            case EffectDescription.MonsterHealth:
                EffectAction(ref EnemyController.EnemyHealthAdded, ref EnemyController.EnemyHealthMultiplier, apply);
                break;
            case EffectDescription.MonsterSpeed:
                EffectAction(ref EnemyController.EnemyMoveSpeedAdded, ref EnemyController.EnemyMoveSpeedMultiplier, apply);
                break;
            case EffectDescription.MonsterDamage:
                EffectAction(ref EnemyController.EnemyDamageAdded, ref EnemyController.EnemyDamageMultiplier, apply);
                break;
            case EffectDescription.MonsterSize:
                EffectAction(ref EnemyController.EnemySizeAdded, ref EnemyController.EnemySizeMultiplier, apply);
                break;
            case EffectDescription.MonsterCount:
                EffectAction(ref EnemyController.EnemyCountAdded, ref EnemyController.EnemyCountMultiplier, apply);
                break;

            // Trial Effects
            case EffectDescription.TimeLimit:
                EffectAction(ref TrialHandler.TimeLimitAdded, ref TrialHandler.TimeLimitMultiplier, apply);
                break;
            case EffectDescription.ObjectiveCount:
                EffectAction(ref TrialHandler.ObjectivesAdded, ref TrialHandler.ObjectiveMultiplier, apply);
                break;
            case EffectDescription.ObjectCount:
                EffectAction(ref TrialHandler.ObjectsAdded, ref TrialHandler.ObjectMultiplier, apply);
                break;

        }
    }

    public void ApplyEffect()
    {
        PassEffectValues(true);
    }

    public void RemoveEffect()
    {
        PassEffectValues(false);
    }
}
