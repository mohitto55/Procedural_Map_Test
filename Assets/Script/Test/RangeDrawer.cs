﻿using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Range2Attribute))]
internal sealed class RangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position,
                      SerializedProperty property, GUIContent label)
    {
        Range2Attribute range2 = (Range2Attribute)attribute;

        if (property.propertyType == SerializedPropertyType.Integer)
            EditorGUI.IntSlider(position, property, range2.min, range2.max, label);
        else
            EditorGUI.PropertyField(position, property, label);
    }
}