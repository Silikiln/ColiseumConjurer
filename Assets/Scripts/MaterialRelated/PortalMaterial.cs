using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/* TODO: Move color conversion to a public or static method here rather than material handler */
public class PortalMaterial /*: IEnumerable<MaterialEffect> */ {
    public string MaterialName { get; set; }
    public string imageLocation { get; set; }
    public char color { get; set; }
    private List<MaterialEffect> effects = new List<MaterialEffect>();

    public void AddEffect(MaterialEffect newEffect)
    {
        effects.Add(newEffect);
    }

    public string EffectTypeNames {
        get
        {
            string result = "";
            foreach (MaterialEffect effect in effects)
                result += effect.ReadableType + "\n";
            return result.Trim();
        }
    }

    public string EffectFormattedValues
    {
        get
        {
            string result = "";
            foreach (MaterialEffect effect in effects)
                result += effect.FormattedValue + "\n";
            return result.Trim();
        }
    }

    /*

    public IEnumerator<MaterialEffect> GetEnumerator()
    {
        return effects.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int EffectCount { get { return effects.Count; } }

    public MaterialEffect this[int i]
    {
        get { return effects[i]; }
    }

    */
}
