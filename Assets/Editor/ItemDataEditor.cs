using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty typeProp = serializedObject.FindProperty("type");
        SerializedProperty displayNameProp = serializedObject.FindProperty("displayName");
        SerializedProperty desProp = serializedObject.FindProperty("des");
        SerializedProperty iconProp = serializedObject.FindProperty("icon");
        SerializedProperty prefabProp = serializedObject.FindProperty("prefab");
        SerializedProperty consumablesProp = serializedObject.FindProperty("consumables");
        SerializedProperty equipablesProp = serializedObject.FindProperty("equipables");
        SerializedProperty canStackProp = serializedObject.FindProperty("canStack");
        SerializedProperty maxStackAmountProp = serializedObject.FindProperty("maxStackAmount");

        // Info
        EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(displayNameProp);
        EditorGUILayout.PropertyField(desProp);
        EditorGUILayout.PropertyField(typeProp);
        EditorGUILayout.PropertyField(iconProp);
        EditorGUILayout.PropertyField(prefabProp);

        // Consumable
        if ((ItemType)typeProp.enumValueIndex == ItemType.Consumable)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Consumable", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(consumablesProp, true);
        }

        // Stacking
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Stacking", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(canStackProp);
        EditorGUILayout.PropertyField(maxStackAmountProp);


        // Equipable
        if ((ItemType)typeProp.enumValueIndex == ItemType.Eqipable)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Equipable", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(equipablesProp, true);
        }


        serializedObject.ApplyModifiedProperties();
    }
}
