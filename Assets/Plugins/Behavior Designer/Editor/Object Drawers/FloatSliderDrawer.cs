﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.ObjectDrawers;

namespace BehaviorDesigner.Editor.ObjectDrawers
{
#if UNITY_EDITOR
    [CustomObjectDrawer(typeof(FloatSliderAttribute))]
    public class FloatSliderDrawer : ObjectDrawer
    {
        public override void OnGUI(GUIContent label)
        {
            var floatSliderAttribute = (FloatSliderAttribute)attribute;
            if (value is SharedFloat) {
                var sharedFloat = value as SharedFloat;
                sharedFloat.Value = EditorGUILayout.Slider(label, sharedFloat.Value, floatSliderAttribute.min, floatSliderAttribute.max);
            } else {
                value = EditorGUILayout.Slider(label, (float)value, floatSliderAttribute.min, floatSliderAttribute.max);
            }
        }
    }
#endif
}