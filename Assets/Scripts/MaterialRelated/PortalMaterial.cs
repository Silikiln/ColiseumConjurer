using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System;

using Random = UnityEngine.Random;

public class PortalMaterial {
    #region Material Handler

    public enum MaterialColor { Red, Green, Blue, Yellow, Meta };

    public static MaterialColor RandomColor {
        get {
            Array values = Enum.GetValues(typeof(MaterialColor));
            return (MaterialColor)values.GetValue(Random.Range(0, values.Length - 1));
        }
    }

    public static List<PortalMaterial> AllMaterials { get; private set; }
    static Color[] VisualColors = {
        // Red
        new Color(1.0f, 0.4f, 0.4f, 1.0f),

        // Green
        new Color(0.4f, 1.0f, 0.4f, 1.0f),

        // Blue
        new Color(0.4f, 0.4f, 1.0f, 1.0f),

        // Yellow
        new Color(1.0f, 1.0f, 0.4f, 1.0f)
    };

    static PortalMaterial() {        
        ImportMaterials();
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
                            material.Name = reader.ReadInnerXml();
                            break;
                        case "Color":
                            string color = reader.ReadInnerXml();
                            try {
                                
                                material.Color = (MaterialColor)Enum.Parse(typeof(MaterialColor), color);
                            } catch (ArgumentException)
                            {
                                throw new ArgumentException("Could not parse material color, \"" + color + "\" is not valid");
                            }
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
    public static PortalMaterial RandomMaterialWithColor(MaterialColor color)
    {
        return RandomElement(AllMaterials.Where(pm => pm.Color == color || pm.Color == MaterialColor.Meta));
    }

    //get material by providing a name
    public static PortalMaterial GetMaterialByName(string givenName)
    {
        return AllMaterials.Where(pm => pm.Name == givenName).FirstOrDefault();
    }

    public static Color GetMaterialColor(MaterialColor color)
    {
        if ((int)color >= VisualColors.Length)
            return UnityEngine.Color.white;
        return VisualColors[(int)color];
    }

    #endregion

    public string Name { get; private set; }
    public string ImageLocation { get; private set; }
    public MaterialColor Color { get; private set; }

    private List<MaterialEffect> effects = new List<MaterialEffect>();

    public Color VisualColor { get { return GetMaterialColor(Color); } }

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
