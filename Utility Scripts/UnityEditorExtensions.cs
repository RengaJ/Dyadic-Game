using UnityEditor;
using UnityEngine;
using System.Collections;

public class UnityEditorExtensions : MonoBehaviour
{
    [MenuItem( "GameObject/Create Empty With MultipleTags %#M" )]
    public static void CreateMultipleTagGameObject ()
    {
        GameObject g = new GameObject( "New Game Object" );
        g.AddComponent<MultipleTags>();
    }

    [MenuItem( "Dyadic Utility/Assign Multiple Tags To Everything" )]
    public static void ApplyMultipleTagsToEverything ()
    {
        GameObject[] objects = (GameObject[]) GameObject.FindSceneObjectsOfType( typeof( GameObject ) );
        foreach (GameObject go in objects)
            if (go.GetComponent<MultipleTags>() == null)
                go.AddComponent<MultipleTags>();
    }
    [MenuItem( "Dyadic Utility/Center Child %&C")]
    public static void CenterChild ()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected.transform.parent != null)
            selected.transform.localPosition = Vector3.zero;
    }

    [MenuItem( "Dyadic Utility/Print Path To Console" )]
    public static void PrintPath ()
    {
        Debug.Log( Utility.GetPathOf( Selection.activeGameObject.transform ) );
    }
}
