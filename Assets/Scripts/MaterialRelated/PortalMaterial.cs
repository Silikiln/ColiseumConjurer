using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortalMaterial {
    public string materialName { get; set; }
    public string imageLocation { get; set; }
    private List<MaterialEffect> effects = new List<MaterialEffect>();

    public string toString(){
        string requestedString = "Name: " + materialName;
        return requestedString;
    }

    public void AddEffect(MaterialEffect newEffect)
    {
        effects.Add(newEffect);
    }

    public string GetAllEffects()
    {
        string fullEffectString = "";
        foreach(MaterialEffect effect in effects){
            fullEffectString = fullEffectString + "Effect -- " + effect.toString() + "\n";
        }

        return fullEffectString;
    }
    
}
