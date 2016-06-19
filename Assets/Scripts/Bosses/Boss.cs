using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class Boss : Trial
{
    public static SortedDictionary<int, Type> PossibleBosses { get; private set; }

    static Boss()
    {
        PossibleBosses = new SortedDictionary<int, Type>();
        foreach (Type t in typeof(Trial).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Boss))))
        {
            int bossValue = ((BossAttribute)t.GetCustomAttributes(false).Where(attr => attr is BossAttribute).First()).BossValue;
            PossibleBosses.Add(bossValue, t);
        }            
    }

    public class BossAttribute : Attribute
    {
        internal BossAttribute(int bossValue)
        {
            BossValue = bossValue;
        }

        public int BossValue { get; set; }
    }

    protected override T LoadResource<T>(string resource)
    {
        return Resources.Load<T>("Bosses/" + GetType().Name + "/" + resource);
    }
}
