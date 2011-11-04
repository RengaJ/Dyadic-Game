using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utility
{
    public enum Charge
    {
        Uncharged ,
        Charged_Attract ,
        Charged_Repel
    }

    public enum MouseDirection
    {
        NONE, UP, DOWN, LEFT, RIGHT, UP_LEFT, UP_RIGHT, DOWN_LEFT, DOWN_RIGHT
    }

    public static void FadeChildren ( GameObject mainObject , float fadeValue )
    {
        if (mainObject.GetComponent<MeshRenderer>() != null)
        {
            if (mainObject.GetComponent<MeshRenderer>().material.shader.name != "RimLighting")
            {
                Color color = mainObject.GetComponent<MeshRenderer>().material.color;
                color.a = fadeValue;
                mainObject.GetComponent<MeshRenderer>().material.color = color;
            }
            else
            {
                Color diffuseColor = mainObject.GetComponent<MeshRenderer>().material.GetColor( "_DiffuseColor" );
                Color rimColor = mainObject.GetComponent<MeshRenderer>().material.GetColor( "_RimColor" );
                diffuseColor.a = rimColor.a = fadeValue;
                mainObject.GetComponent<MeshRenderer>().material.SetColor( "_DiffuseColor" , diffuseColor );
                mainObject.GetComponent<MeshRenderer>().material.SetColor( "_RimColor" , rimColor );
            }
        }
        for (int i = 0 ; i < mainObject.transform.GetChildCount() ; i++)
            FadeChildren( mainObject.transform.GetChild( i ).gameObject , fadeValue );
    }

    // Reflect a point off of a surface
    public static Vector3 Reflect ( Vector3 point1 , Vector3 point2 , Vector3 reflectOver )
    {

        // Calculate the reflected point (formula found at http://mathworld.wolfram.com/Reflection.html)
        Vector2 X1 = new Vector2( point1.x , point1.y );
        Vector2 nHat = new Vector2( reflectOver.x , reflectOver.y );
        Vector2 X0 = new Vector2( point2.x , point2.y );
        Vector2 X1Prime = -X1 + 2 * X0 + 2 * nHat * ( Vector2.Dot( ( X1 - X0 ) , nHat ) );
        return new Vector3( X1Prime.x , X1Prime.y , point1.z );
    }

    public static string GetPathOf ( Transform g )
    {
        List<string> path = new List<string>();
        while (g != null)
        {
            path.Add( g.name );
            g = g.parent;
        }
        path.Reverse();

        string output = "";
        for (int i = 0 ; i < path.Count ; i++)
            output += ("/" + path[i]);

        return output;
    }

    public static void Mul ( Vector3 vector1 , Vector3 vector2 , out Vector3 output )
    {
        output = Vector3.zero;
        for (int i = 0 ; i < 3 ; i++)
            output[i] = vector1[i] * vector2[i];
    }

    public static Vector3 Mul ( Vector3 vector1 , Vector3 vector2 )
    {
        Vector3 output = Vector3.zero;
        for (int i = 0 ; i < 3 ; i++)
            output[i] = vector1[i] * vector2[i];

        return output;
    }

    public static bool SameXZ ( GameObject objectInQuestion , Vector3 locationInQuestion ) // this tests if an object and a position are located on the same X/Z coordinates
    {
        // Throw the necessary components into Vector2's so that it is easier to understand / compare
        Vector2 objectPosition = Vector2.zero;
        objectPosition.x = objectInQuestion.transform.position.x;
        objectPosition.y = objectInQuestion.transform.position.z;

        Vector2 locationPosition = Vector2.zero;
        locationPosition.x = locationInQuestion.x;
        locationPosition.y = locationInQuestion.z;
        // Compare the Components
        if (RoundToNearestHalf( objectPosition ) != RoundToNearestHalf( locationPosition ))
            return false;
        // All is good here
        return true;
    }
    public static bool SameXZ ( Vector3 location1 , Vector3 location2 )
    {
        Vector3 roundedLocation1 = RoundToNearestHalf( location1 );
        Vector3 roundedLocation2 = RoundToNearestHalf( location2 );
        if (roundedLocation1.x != roundedLocation2.x || roundedLocation1.z != roundedLocation2.z)
            return false;
        return true;
    }

    public static float RoundToNearestHalf ( float value ) // make sure that we snap to nodes when we finish moving
    {
        int top = (int) value;
        float bottom = Mathf.Abs(value - top);
        if (bottom < 0.25)
            bottom = 0.0f;
        else if (bottom < 0.75)
            bottom = 0.5f;
        else
            bottom = 1.0f;
        if (value < 0)
            return top - bottom;
        return top + bottom;
    }
    public static Vector3 RoundToNearestHalf ( Vector3 value ) // a Vector3 version that rounds the X and Z coordinates to the nearest half
    {
        Vector3 newValue = Vector3.zero;
        newValue.x = RoundToNearestHalf( value.x );
        newValue.y = value.y;
        newValue.z = RoundToNearestHalf( value.z );
        return newValue;
    }
    public static Vector2 RoundToNearestHalf ( Vector2 value )
    {
        Vector2 newValue = Vector2.zero;
        newValue.x = RoundToNearestHalf( value.x );
        newValue.y = RoundToNearestHalf( value.y );
        return newValue;
    }

    public static float RoundToNearestTenth ( float value )
    {
        int top = (int) value;
        float bottom = Mathf.Abs( value - top );
        float newBottom = bottom * 10.0f;
        int newBottomBottom = (int) newBottom;
        newBottom = Mathf.Abs( newBottom - newBottomBottom );
        if (newBottom > 0.5f)
            newBottomBottom++;
        if (value < 0)
            return top - ( newBottomBottom / 10.0f );
        return top + ( newBottomBottom / 10.0f );
   }

    public static Vector3 RoundToNearestTenth ( Vector3 value )
    {
        Vector3 returnVal = new Vector3();
        for (int i = 0 ; i < 3 ; i++)
            returnVal[i] = RoundToNearestTenth( value[i] );
        return returnVal;
    }

    public static Vector3[] RoundToNearestTenth ( Vector3[] values )
    {
        Vector3[] newValues = new Vector3[values.Length];
        for (int i = 0 ; i < newValues.Length ; i++)
            newValues[i] = RoundToNearestTenth( values[i] );

        return newValues;
    }

    public static void RoundToNearestTenth ( float value , out float output )
    {
        int top = (int) value;
        float bottom = Mathf.Abs( value - top );
        float newBottom = bottom * 10.0f;
        int newBottomBottom = (int) newBottom;
        newBottom = Mathf.Abs( newBottom - newBottomBottom );
        if (newBottom > 0.5f)
            newBottomBottom++;
        if (value < 0)
            output = top - ( newBottomBottom / 10.0f );
        output = top + ( newBottomBottom / 10.0f );
    }

    public static void RoundToNearestTenth ( Vector3 value , out  Vector3 output )
    {
        output = Vector3.zero;
        for (int i = 0 ; i < 3 ; i++)
        {
            float outputValue = 0.0f;
            RoundToNearestTenth( value[i] , out outputValue );
            output[i] = outputValue;
        }
    }

    public static void RoundToNearestTenth ( Vector3[] values , out Vector3[] output )
    {
        output = new Vector3[values.Length];
        for (int i = 0 ; i < values.Length ; i++)
        {
            Vector3 outputValue = Vector3.zero;
            RoundToNearestTenth( values[i] , out outputValue );
            output[i] = outputValue;
        }
    }

    public static Vector3 ManhattanDistance ( Vector3 point1 , Vector3 point2 )
    {
        Vector3 output = Vector3.zero;
        for (int i = 0 ; i < 3 ; i++)
            output[i] = point1[i] - point2[i];

        return output;
    }
}
