using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public class AngleAttribute : PropertyAttribute
{
    public float knobSize;
    public AngleAttribute(float knobSize)
    {
        this.knobSize = knobSize;
    }
}

[CustomPropertyDrawer(typeof(AngleAttribute))]
public class AngleDrawer : PropertyDrawer
{
    private readonly MethodInfo knobMethodInfo = typeof(EditorGUI).GetMethod("Knob",
           BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);

    private float Knob(Rect position, Vector2 knobSize,
                          float currentValue, float start,
                          float end, string unit,
                          Color backgroundColor, Color activeColor,
                          bool showValue)
    {
        var controlID = GUIUtility.GetControlID("Knob".GetHashCode(),
                                                  FocusType.Passive, position);

        var invoke = knobMethodInfo.Invoke(null, new object[] {
        position, knobSize, currentValue,
        start, end, unit, backgroundColor,
        activeColor, showValue,
        controlID });
        return (float)(invoke ?? 0);
    }
}
