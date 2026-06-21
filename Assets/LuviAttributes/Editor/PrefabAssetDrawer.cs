using UnityEngine;
using UnityEditor;

namespace LuviKunG.Attributes.Editor
{
    using UnityObject = UnityEngine.Object;

    [CustomPropertyDrawer(typeof(PrefabAssetAttribute), true)]
    public class PrefabAssetDrawer : PropertyDrawer
    {
        private const string WARNING_MESSAGE_NULL_REFERENCE = "This field cannot be null. Please assign a prefab asset.";
        private const string WARNING_MESSAGE_INVALID_PREFAB = "This field is intended for prefab references from the asset database. Please assign a valid prefab asset.";

        private static readonly GUIContent _nullContent = new(WARNING_MESSAGE_NULL_REFERENCE);
        private static readonly GUIContent _invalidContent = new(WARNING_MESSAGE_INVALID_PREFAB);
        private float _cachedWidth = 0f;

        private float GetHelpBoxHeight(GUIContent content, float width)
        {
            return EditorStyles.helpBox.CalcHeight(content, width);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(property, label, true);
            UnityObject target = property.serializedObject.targetObject;
            if (target != null && target is Component)
            {
                float width = _cachedWidth > 0f ? _cachedWidth : EditorGUIUtility.currentViewWidth;
                if (IsNull(property))
                    height += GetHelpBoxHeight(_nullContent, width) + EditorGUIUtility.standardVerticalSpacing;
                else if (!IsAssetPrefab(property))
                    height += GetHelpBoxHeight(_invalidContent, width) + EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _cachedWidth = position.width;
            UnityObject target = property.serializedObject.targetObject;
            bool isComponent = target != null && target is Component;
            bool isError = isComponent && IsNull(property);
            bool isWarning = isComponent && !isError && !IsAssetPrefab(property);
            if (isError)
            {
                float helpBoxHeight = GetHelpBoxHeight(_nullContent, position.width);
                Rect helpBoxRect = new(position.x, position.y, position.width, helpBoxHeight);
                EditorGUI.HelpBox(helpBoxRect, WARNING_MESSAGE_NULL_REFERENCE, MessageType.Error);

                Rect propertyRect = new(position.x, position.y + helpBoxHeight + EditorGUIUtility.standardVerticalSpacing, position.width, position.height - helpBoxHeight - EditorGUIUtility.standardVerticalSpacing);
                Color cache = GUI.color;
                GUI.color = Color.red;
                EditorGUI.PropertyField(propertyRect, property, label, true);
                GUI.color = cache;
            }
            else if (isWarning)
            {
                float helpBoxHeight = GetHelpBoxHeight(_invalidContent, position.width);
                Rect helpBoxRect = new(position.x, position.y, position.width, helpBoxHeight);
                EditorGUI.HelpBox(helpBoxRect, WARNING_MESSAGE_INVALID_PREFAB, MessageType.Warning);

                Rect propertyRect = new(position.x, position.y + helpBoxHeight + EditorGUIUtility.standardVerticalSpacing, position.width, position.height - helpBoxHeight - EditorGUIUtility.standardVerticalSpacing);
                Color cache = GUI.color;
                GUI.color = Color.yellow;
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

        private bool IsAssetPrefab(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference || property.objectReferenceValue == null)
                return false;

            GameObject obj = property.objectReferenceValue as GameObject;
            if (obj == null)
            {
                Component component = property.objectReferenceValue as Component;
                if (component != null)
                    obj = component.gameObject;
                else
                    return false;
            }

            if (!AssetDatabase.Contains(obj))
                return false;

            PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(obj);
            return assetType == PrefabAssetType.Regular || assetType == PrefabAssetType.Variant;
        }
    }
}
