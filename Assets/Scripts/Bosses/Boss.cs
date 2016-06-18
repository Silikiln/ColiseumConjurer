using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class Boss : Trial
{
    public static Dictionary<int, Type> PossibleBosses { get; private set; }

    static Boss()
    {
        PossibleBosses = new Dictionary<int, Type>();
        foreach (Type t in typeof(Trial).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Boss))))
        {
            int bossValue = ((BossAttribute)t.GetCustomAttributes(false).Where(attr => attr is BossAttribute)).BossValue;
            PossibleBosses.Add(bossValue, t);
        }            
    }

    public class BossAttribute
    {
        internal BossAttribute(int bossValue)
        {
            BossValue = bossValue;
        }

        public int BossValue { get; set; }
    }
}
