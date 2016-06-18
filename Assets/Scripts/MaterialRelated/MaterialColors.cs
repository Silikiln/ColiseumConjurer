using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

using Random = UnityEngine.Random;

public static class MaterialColors
{
    public class MaterialColorAttribute : Attribute
    {
        internal MaterialColorAttribute(float r, float g, float b, float a)
        {
            Color = new Color(r, g, b, a);
        }
        internal MaterialColorAttribute(float r, float g, float b) : this(r, g, b, 1) { }
        public Color Color { get; set; }
    }

    public enum MaterialColor
    {
        [MaterialColor(1.0f, 0.4f, 0.4f)]
            Red = 0,

        [MaterialColor(0.4f, 1.0f, 0.4f)]
            Green = 2,

        [MaterialColor(0.4f, 0.4f, 1.0f)]
            Blue = 4,

        [MaterialColor(0.729f, 0.333f, 0.827f)]
            Purple = 6,

        [MaterialColor(1, 1, 0)]
            Yellow = 8,

        [MaterialColor(1.0f, 0.55f, 0.78f)]
            Pink = 10,

        [MaterialColor(1, 0.549f, 0)]
            Orange = 12,

        [MaterialColor(0, 0, 0)]
            Black = 14,

        [MaterialColor(1, 1, 1)]
            White = 16,
    };

    public static Color GetVisualColor(this MaterialColor mc)
    {
        return GetAttr(mc).Color;
    }

    private static MaterialColorAttribute GetAttr(MaterialColor mc)
    {
        return (MaterialColorAttribute)Attribute.GetCustomAttribute(ForValue(mc), typeof(MaterialColorAttribute));
    }

    private static MemberInfo ForValue(MaterialColor mc)
    {
        return typeof(MaterialColor).GetField(Enum.GetName(typeof(MaterialColor), mc));
    }
}
