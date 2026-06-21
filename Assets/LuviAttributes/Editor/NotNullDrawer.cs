using UnityEngine;
using UnityEditor;

namespace LuviKunG.Attributes.Editor
{
    using UnityObject = UnityEngine.Object;

    [CustomPropertyDrawer(typeof(NotNullAttribute), true)]
    public class NotNullDrawer : PropertyDrawer
    {
        private const string WARNING_MESSAGE = "This field cannot be null.";

        private static readonly GUIContent _warningContent = new(WARNING_MESSAGE);
        private float _cachedWidth = 0f;

        private float GetHelpBoxHeight(float width)
        {
            return EditorStyles.helpBox.CalcHeight(_warningContent, width);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(property, label, true);
            UnityObject target = property.serializedObject.targetObject;
            if (target != null && target is Component && IsNull(property))
            {
                float width = _cachedWidth > 0f ? _cachedWidth : EditorGUIUtility.currentViewWidth;
                height += GetHelpBoxHeight(width) + EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _cachedWidth = position.width;
            UnityObject target = property.serializedObject.targetObject;
            bool isComponent = target != null && target is Component;
            if (isComponent && IsNull(property))
            {
                float helpBoxHeight = GetHelpBoxHeight(position.width);
                Rect helpBoxRect = new(position.x, position.y, position.width, helpBoxHeight);
                EditorGUI.HelpBox(helpBoxRect, WARNING_MESSAGE, MessageType.Error);

                Rect propertyRect = new(position.x, position.y + helpBoxHeight + EditorGUIUtility.standardVerticalSpacing, position.width, position.height - helpBoxHeight - EditorGUIUtility.standardVerticalSpacing);
                Color cache = GUI.color;
                GUI.color = Color.red;
                EditorGUI.PropertyField(propertyRect, property, label, true);
                GUI.color = cache;
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        private bool IsNull(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null;
        }
    }
}
