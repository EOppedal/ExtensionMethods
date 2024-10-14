﻿using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace ExtensionMethods {
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredReferenceAttribute : PropertyAttribute {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class RequiredReferenceEditor : Editor {
        public override void OnInspectorGUI() {
            var targetType = target.GetType();
            var fields = targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields) {
                var attributes = field.GetCustomAttributes(typeof(RequiredReferenceAttribute), true);
                if (attributes.Length <= 0) continue;

                var isSerialized = field.IsPublic || Attribute.IsDefined(field, typeof(SerializeField));
                if (isSerialized) continue;

                var valueToAssign = ((MonoBehaviour)target).GetOrAdd(field.FieldType);
                field.SetValue(target, valueToAssign);
            }

            DrawDefaultInspector();
        }
    }
#endif
}