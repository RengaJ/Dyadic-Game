using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(JournalEntryList))]
public class JournalEntryListEditor : Editor
{
	// Use this for initialization
	override public void OnInspectorGUI()
	{
		JournalEntryList entry_list = (JournalEntryList)target;
		EditorGUILayout.BeginVertical();
		GUILayoutOption[] options = { GUILayout.Width(256), GUILayout.Height(192) };
		entry_list.journalTexture = (Texture2D)EditorGUILayout.ObjectField(entry_list.journalTexture,typeof(Texture2D),options);
		
		EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Helios Rectangle:");
			EditorGUILayout.RectField(entry_list.helios_rect);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Nyx Rectangle:");
			EditorGUILayout.RectField(entry_list.nyx_rect);
		EditorGUILayout.EndHorizontal();
		
		
		for (int i = 0; i < entry_list.list.Count; i++)
		{
			GUILayout.Label("Helios Journal Entry (" + i + "):");
			entry_list.list[i].helios_entry = EditorGUILayout.TextArea(entry_list.list[i].helios_entry, GUILayout.Height(75));

			GUILayout.Label("Nyx Journal Entry (" + i + "):");
			entry_list.list[i].nyx_entry = EditorGUILayout.TextArea(entry_list.list[i].nyx_entry, GUILayout.Height(75));
		}
		if (GUILayout.Button("Add New Entry"))
			entry_list.AddNewEntry();
		if (GUILayout.Button("Remove Last Entry"))
			entry_list.RemoveLastEntry();
		if (GUI.changed)
			EditorUtility.SetDirty(target);
		EditorGUILayout.EndVertical();
	}
}
