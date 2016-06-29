using UnityEngine;
using UnityEditor;
using System.Collections;
using Devdog.InventorySystem.Models;

namespace Devdog.InventorySystem.Editors
{
    [CustomEditor(typeof(InventoryEquippableField))]
    public class InventoryEquippableFieldEditor : Editor
    {
        private SerializedProperty equipTypes;

        private UnityEditorInternal.ReorderableList list;

        public void OnEnable()
        {
            equipTypes = serializedObject.FindProperty("_equipTypes");

            list = new UnityEditorInternal.ReorderableList(serializedObject, equipTypes, true, true, true, true);
            list.drawHeaderCallback += rect => EditorGUI.LabelField(rect, "Which types can be placed in this field?");
            list.drawElementCallback += (rect, index, active, focused) =>
            {
                rect.height = 16;
                rect.y += 3;

                var i = equipTypes.GetArrayElementAtIndex(index);
                i.intValue = EditorGUI.Popup(rect, i.intValue, ItemManager.database.equipTypesStrings);
                //EditorGUI.PropertyField(rect, equipTypes.GetArrayElementAtIndex(index));
                //EditorGUILayout.Popup(e.intValue, InventoryEditorUtility.equipTypesStrings);

            };
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical();

            DrawPropertiesExcluding(serializedObject, new string[]
            {
                "m_Script",
                "ID",
                "_equipTypes"
            });
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            if (GUILayout.Button("Edit types"))
            {
                InventoryMainEditor.window.Show();
                InventoryMainEditor.SelectTab(typeof(EquipTypeEditor));
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Define which types are allowed in this wrapper.\n\nFor example when selecting helmet and necklace both items with equipment type helmet and neckalce can be equipped to this slot.", InventoryEditorStyles.labelStyle);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            list.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

    }
}