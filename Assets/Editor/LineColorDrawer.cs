using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomPropertyDrawer(typeof(LineColor))]
public class LineColorDrawer : PropertyDrawer
{
    const int textHeight = 16;
    const int fieldBuffer = 2;
    static Color defaultColor = Color.white;

    // Here you must define the height of your property drawer. Called by Unity.
    public override float GetPropertyHeight(SerializedProperty prop,
                                             GUIContent label)
    {
        SerializedProperty colorType = prop.FindPropertyRelative("ColorType");

        switch (colorType.enumValueIndex)
        {
            case 0: return (textHeight + fieldBuffer) * 3;
            case 1: return (textHeight + fieldBuffer) * 4;
                /*
                SerializedProperty weightedValues = prop.FindPropertyRelative("WeightedValues");
                return (textHeight + fieldBuffer) * (5 + weightedValues.arraySize);
                */
        }

        return base.GetPropertyHeight(prop, label);
    }

    // Here you can define the GUI for your property drawer. Called by Unity.
    public override void OnGUI(Rect position,
                                SerializedProperty prop,
                                GUIContent label)
    {
        EditorGUI.BeginProperty(position, new GUIContent(prop.name), prop);
        EditorGUI.LabelField(position, prop.displayName);

        position.height = textHeight;
        position.x += 20;
        position.width -= 20;

        position.y += textHeight + fieldBuffer;
        SerializedProperty colorType = prop.FindPropertyRelative("ColorType");
        colorType.enumValueIndex = (int)(LineColor.ColorStyle)EditorGUI.EnumPopup(position, "Color Type", (LineColor.ColorStyle)colorType.enumValueIndex);

        SerializedProperty colors = prop.FindPropertyRelative("Colors");
        SerializedProperty color;

        switch (colorType.enumValueIndex)
        {
            // Solid
            case 0:
                FillColorArray(colors, 1);

                position.y += textHeight + fieldBuffer;
                color = colors.GetArrayElementAtIndex(0);
                color.colorValue = EditorGUI.ColorField(position, "Color", color.colorValue);
                break;

            // Gradient
            case 1:
                FillColorArray(colors, 2);

                position.y += textHeight + fieldBuffer;
                color = colors.GetArrayElementAtIndex(0);
                color.colorValue = EditorGUI.ColorField(position, "Start Color", color.colorValue);

                position.y += textHeight + fieldBuffer;
                color = colors.GetArrayElementAtIndex(1);
                color.colorValue = EditorGUI.ColorField(position, "End Color", color.colorValue);
                break;
        }

        /*
        switch (colorType.enumValueIndex)
        {
            // Weighted
            case 1:
                position.y += textHeight + fieldBuffer;
                SerializedProperty weightedValues = prop.FindPropertyRelative("WeightedValues");
                if (oldSides != sides.intValue)
                    GenerateUnweightedSidesArray(prop, weightedValues, sides.intValue);

                int oldSize = weightedValues.arraySize;
                weightedValues.arraySize = EditorGUI.IntField(position, "Weighted Value size", weightedValues.arraySize);
                if (oldSize != weightedValues.arraySize)
                    GenerateUnweightedSidesArray(prop, weightedValues, sides.intValue);

                position.x += 20;
                position.width -= 20;
                for (int i = 0; i < weightedValues.arraySize; i++)
                {

                    position.y += textHeight + fieldBuffer;
                    SerializedProperty weightedValue = weightedValues.GetArrayElementAtIndex(i);
                    Vector2 oldWeightedValue = weightedValue.vector2Value;
                    weightedValue.vector2Value = EditorGUI.Vector2Field(position, "Value #" + i, weightedValue.vector2Value);

                    if (oldWeightedValue.x != weightedValue.vector2Value.x)
                        GenerateUnweightedSidesArray(prop, weightedValues, sides.intValue);

                    if (oldWeightedValue.y != weightedValue.vector2Value.y)
                        ReproportionWeights(prop, weightedValues, weightedValue, oldWeightedValue.y, i);
                }
                break;
        } */

        EditorGUI.EndProperty();
    }


    void FillColorArray(SerializedProperty colors, int desiredSize)
    {
        if (colors.arraySize < desiredSize)
            colors.arraySize = desiredSize;
        colors.GetArrayElementAtIndex(colors.arraySize - 1).colorValue = defaultColor;
    }
}