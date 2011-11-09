using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Dyadic/Utility/Multiple Tags")]
public class MultipleTags : MonoBehaviour
{
    public List<string> tags;
    [HideInInspector]
    private string exceptionString = "MultipleTags Exception -- Index out of bounds.\n";

    public string[] GetTags ()
    {
        return tags.ToArray();
    }

    public string TagAt ( int index )
    {
        if (index < 0 || index > tags.Count-1)
            throw new UnityException( exceptionString + "Valid indices: 0 to " + ( tags.Count - 1 ) );
        return tags[index];
    }

    public int IndexOf ( string tag )
    {
        if (HasTag( tag ))
            return tags.IndexOf( tag );
        return -1;
    }

    public bool HasTag ( string tag )
    {
        foreach (string t in tags)
            if (string.Compare( t , tag ) == 0)
                return true;
        return false;
    }

    public void AddTag ( string tag )
    {
        if (!HasTag( tag ))
            tags.Add( tag );
    }

    public void RemoveTag ( string tag )
    {
        if (HasTag( tag ))
            tags.Remove( tag );
    }

    public void ModifyTag ( string original , string newTag )
    {
        if (HasTag( original ))
            tags[IndexOf( original )] = newTag;
        else
            Debug.LogWarning( "MultipleTags Warning -- Cannot find tag " + original + " to modify." );
    }

    public static string[] GetTagsFrom ( GameObject g )
    {
        if (g.GetComponent<MultipleTags>() != null)
            return g.GetComponent<MultipleTags>().GetTags();
        return null;
    }

    public static bool HasTagIn ( GameObject g , string tag )
    {
        if (g.GetComponent<MultipleTags>() != null)
            return g.GetComponent<MultipleTags>().HasTag( tag );
        return false;
    }
}