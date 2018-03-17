using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BB.SaveLoadSystem
{
    [CustomEditor(typeof(SaveLoadManager))]
    public class SaveLoadManagerEditor : Editor
    {
        private SaveLoadManager manager = null;

        public override void OnInspectorGUI()
        {
            if (manager == null)
            {
                manager = (SaveLoadManager)target;
            }

            EditorGUILayout.LabelField("Savable objects", EditorStyles.helpBox);

            for (int i = 0; i < manager.savables.Count; ++i)
            {
                DrawSavableObject(i);
            }

            SlotsHandleButtons();
        }

        private void SlotsHandleButtons()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Create new slot"))
            {
                manager.savables.Add(null);
            }

            if (GUILayout.Button("Pop"))
            {
                manager.savables.RemoveAt(manager.savables.Count - 1);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawSavableObject(int id)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(string.Format("[{0}]", id));
            manager.savables[id] = (MonoBehaviour)EditorGUILayout.ObjectField(manager.savables[id], typeof(MonoBehaviour), true);

            EditorGUILayout.EndHorizontal();
        }
    }
}
