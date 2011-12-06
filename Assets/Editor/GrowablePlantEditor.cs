using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(GrowablePlant))]
public class GrowablePlantEditor : Editor
{
    override public void OnInspectorGUI ()
    {
        GrowablePlant plant = (GrowablePlant) target;
        plant.endPosition = EditorGUILayout.Vector3Field( "End Position" , plant.endPosition );
        GUILayout.Label( "Grow Smoothing" );
        plant.growsmoothing = EditorGUILayout.Slider( plant.growsmoothing , 0.0f , 20.0f );

        if (GUILayout.Button( "Test" ))
        {
            Debug.Log( "Durp" );
            plant.Grow();
        }
    }
    void OnSceneGUI ()
    {
        GrowablePlant plant = (GrowablePlant)target;
        Handles.color = Color.green;
        plant.endPosition = Handles.PositionHandle( plant.endPosition , Quaternion.identity );
        Handles.SphereCap( 0 , plant.endPosition , Quaternion.identity , 1.0f );
        Handles.DrawLine( plant.transform.position , plant.endPosition );
    }
}
