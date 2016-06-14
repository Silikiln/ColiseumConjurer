using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System;

using Random = UnityEngine.Random;

/* TODO: Move color conversion to a public or static method here rather than material handler */
public class PortalMaterial {
    #region Material Handler

    public static List<PortalMaterial> AllMaterials { get; private set; }

    static PortalMaterial() {        
        ImportMaterials();

        foreach (PortalMaterial pm in AllMaterials.Where(m => m.color == 'R' || m.color == 'M'))
            Debug.Log(pm.MaterialName);
    }

    static void ImportMaterials()
    {
        AllMaterials = new List<PortalMaterial>();

        //import some stuff
        using (XmlReader reader = XmlReader.Create("Assets/PortalMaterials.xml"))
        {
            PortalMaterial material = new PortalMaterial();
            MaterialEffect effect = new MaterialEffect();
            reader.ReadToFollowing("Material");
            while (!reader.EOF)
            {
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement)
                    switch (reader.Name)
                    {
                        case "Material":
                            material = new PortalMaterial();
                            break;
                        case "Name":
                            material.MaterialName = reader.ReadInnerXml();
                            break;
                        case "Color":
                            material.color = char.Parse(reader.ReadInnerXml());
                            break;
                        case "Effect":
                            effect = new MaterialEffect();
                            break;
                        case "Effect-Type":
                            effect.effectType = int.Parse(reader.ReadInnerXml());
                            break;
                        case "Effect-Value":
                            effect.effectValue = float.Parse(reader.ReadInnerXml());
                            break;

                    }
                else
                    switch (reader.Name)
                    {
                        case "Material":
                            AllMaterials.Add(material);
                            if (material.color == 'M')
                                Debug.Log("Loaded Meta");
                            break;
                        case "Effect":
                            material.AddEffect(effect);
                            break;
                    }
            }
        }
    }

    static T RandomElement<T>(IEnumerable<T> enumerable)
    {
        var list = enumerable as IList<T> ?? enumerable.ToList();
        return list.Count == 0 ? default(T) : list[Random.Range(0, list.Count)];
    }

    //select a random material from the sortedlist and return it to caller
    public static PortalMaterial RandomMaterial { get { return RandomElement(AllMaterials); } }

    //select a random material of the given color
    public static PortalMaterial RandomMaterialWithColor(char color)
    {
        return RandomElement(AllMaterials.Where(pm => pm.color == color || pm.color == 'M'));
    }

    //get material by providing a name
    public static PortalMaterial GetMaterialByName(string givenName)
    {
        return AllMaterials.Where(pm => pm.MaterialName == givenName).FirstOrDefault();
    }

    public static Color GetMaterialColor(char color)
    {
        switch (color)
        {
            case 'R':
                return new Color(1.0f, 0.4f, 0.4f, 1.0f);
            case 'G':
                return new Color(0.4f, 1.0f, 0.4f, 1.0f);
            case 'B':
                return new Color(0.4f, 0.4f, 1.0f, 1.0f);
            case 'M':
                return new Color(1.0f, 1.0f, 0.4f, 1.0f);
        }
        return new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    #endregion

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
}
