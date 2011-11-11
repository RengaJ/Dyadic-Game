using UnityEditor;
using UnityEngine;
using System.Collections;

public class TransformObject : EditorWindow
{
	Vector3 translation = Vector3.zero;
	Vector3 rotation = Vector3.zero;
	Vector3 scale = Vector3.one;
	
	bool useTranslate = false;
	bool useRotation = false;
	bool useScale = false;
	
	[MenuItem("Dyadic Utility/Transform Objects")]
	static void Init()
	{
		TransformObject window = (TransformObject)GetWindow(typeof(TransformObject));
		window.title = "Transform";
		window.ShowUtility();
	}
	
	void OnGUI()
	{
		GUILayout.Label("Use this to set position, rotation,\nand scale for multiple objects.");
		useTranslate = EditorGUILayout.Toggle("Use Translate: ", useTranslate);
		if (useTranslate)
			translation = EditorGUILayout.Vector3Field("Set Position: ",translation);
		useRotation = EditorGUILayout.Toggle("Use Rotation: ", useRotation);
		if (useRotation)
			rotation = EditorGUILayout.Vector3Field("Set Rotation: ", rotation);
		useScale = EditorGUILayout.Toggle("Use Scale: ", useScale);
		if (useScale)
			scale = EditorGUILayout.Vector3Field("Set Scale: ", scale);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Apply Transformations"))
			ApplyTransformations();
		if (GUILayout.Button("Close"))
			this.Close();
		
	}
	
	void OnInspectorUpdate()
	{
		Repaint();
	}
	
	void ApplyTransformations()
	{
		Transform[] transforms = Selection.transforms;
		foreach (Transform transform in transforms)
		{
			if (useTranslate)
				transform.position = translation;
			if (useRotation)
				transform.rotation = Quaternion.Euler(rotation);
			if (useScale)
				transform.localScale = scale;
		}
	}
}
