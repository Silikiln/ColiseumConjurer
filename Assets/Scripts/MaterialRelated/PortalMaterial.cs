using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System;

using MaterialColor = MaterialColors.MaterialColor;
using Random = UnityEngine.Random;

[XmlParse("Material")]
public class PortalMaterial : IEnumerable<MaterialEffect> {
    #region Material Handler

    public static List<PortalMaterial> AllMaterials { get; private set; }

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
        return RandomElement(AllMaterials.Where(pm => pm.Color == color));
    }

    //get material by providing a name
    public static PortalMaterial GetMaterialByName(string givenName)
    {
        return AllMaterials.Where(pm => pm.Name == givenName).FirstOrDefault();
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

    public Color VisualColor { get { return Color.GetVisualColor(); } }

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
