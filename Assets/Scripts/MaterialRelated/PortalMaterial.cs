using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System;

using Random = UnityEngine.Random;

[XmlParse("Material")]
public class PortalMaterial : IEnumerable<MaterialEffect> {
    #region Material Handler

    public enum MaterialColor { Red, Green, Blue, Purple, Yellow, Meta };

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

        // Purple
        new Color(0.729f, 0.333f, 0.827f),

        // Yellow
        new Color(1.0f, 1.0f, 0.4f, 1.0f),
    };

    static PortalMaterial() {
        AllMaterials = XmlParseAttribute.ReadFileIntoList<PortalMaterial>("Assets/PortalMaterials.xml");
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

    [XmlParse("Name")]
    public string Name { get; private set; }

    [XmlParse("Description")]
    public string Description { get; private set; }

    [XmlParse("ImagePath")]
    public string ImagePath { get; private set; }

    [XmlParse("Color")]
    public MaterialColor Color { get; private set; }

    [XmlParse("Effects")]
    private List<MaterialEffect> effects = new List<MaterialEffect>();

    public Color VisualColor { get { return GetMaterialColor(Color); } }

    public Sprite Image { get { return Resources.Load<Sprite>("Materials/" + ImagePath); } }

    public void AddEffect(MaterialEffect newEffect)
    {
        effects.Add(newEffect);
    }

    public IEnumerator<MaterialEffect> GetEnumerator()
    {
        return ((IEnumerable<MaterialEffect>)effects).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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
