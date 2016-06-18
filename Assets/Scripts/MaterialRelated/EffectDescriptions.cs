using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public static class EffectDescriptions {
    public class EffectAttribute : Attribute
    {
        internal EffectAttribute(string name, bool positive)
        {
            DisplayName = name;
            IsPositive = positive;
        }
        public string DisplayName { get; private set; }
        public bool IsPositive { get; private set; }
    }

    public enum EffectDescription
    {
        [Effect("Player Health", true)]     PlayerHealth,      //0
        [Effect("Player Speed", true)]      PlayerSpeed,       //1
        
        [Effect("Player Damage", true)]     PlayerDamage,      //2
        [Effect("Player Lives", true)]      PlayerLives,       //3
        [Effect("Player Size", false)]      PlayerSize,       //4
        [Effect("Attack Speed", true)]      PlayerAttackSpeed,       //5

        // Monster Effects
        [Effect("Monster Health", false)]   MonsterHealth,    //6
        [Effect("Monster Speed", false)]    MonsterSpeed,     //7
        [Effect("Monster Damage", false)]   MonsterDamage,    //8
        [Effect("Monster Size", false)]     MonsterSize,      //9
        [Effect("Monster Count", false)]    MonsterCount,     //10

        // Trial Effects
        [Effect("Time Limit", true)]        TimeLimit,         //11
        [Effect("Objectives", false)]       ObjectiveCount,        //12
        [Effect("Objects", false)]          ObjectCount,           //13

        [Effect("Player Max Speed", true)]
        PlayerMaxSpeed,
    }

    public static string GetDisplayName(this EffectDescription p)
    {
        return GetAttr(p).DisplayName;
    }

    public static bool IsPositive(this EffectDescription ed)
    {
        return GetAttr(ed).IsPositive;
    }

    private static EffectAttribute GetAttr(EffectDescription ed)
    {
        return (EffectAttribute)Attribute.GetCustomAttribute(ForValue(ed), typeof(EffectAttribute));
    }

    private static MemberInfo ForValue(EffectDescription ed)
    {
        return typeof(EffectDescription).GetField(Enum.GetName(typeof(EffectDescription), ed));
    }
}
