using UnityEngine;
using System.Collections;

public enum CharacterSide { Left , Right , Head , Floor };

public struct CharacterCollisionData
{
    public CharacterSide side;
    public bool status;
}

[AddComponentMenu( "Dyadic/2D Character/Collision Volume" )]
public class CharacterCollision : MonoBehaviour
{
    public CharacterSide side;
    public CharacterCollisionData data;
	// Use this for initialization
    public void Start ()
    {
        data.side = side;
        data.status = false;
    }
    public void OnTriggerEnter (Collider other)
    {
        MultipleTags tags = other.collider.GetComponent<MultipleTags>();
        if (tags.HasTag( "Floor" ))
        {
            data.status = ( side == CharacterSide.Floor );
            SendMessageUpwards( "UpdateStatus" , data );
        }
		if (tags.HasTag( "Lethal" ))
			SendMessageUpwards( "Die" , null);
    }
    public void OnTriggerExit ( Collider other )
    {
        MultipleTags tags = other.collider.GetComponent<MultipleTags>();
        if (tags.HasTag( "Floor" ))
        {
            data.status = ( side != CharacterSide.Floor );
            SendMessageUpwards( "UpdateStatus" , data );
        }
        // No need to check "Lethal" tag, since it will kill you instantly anyway
    }
    public void OnTriggerStay ( Collider other )
    {
        MultipleTags tags = other.collider.GetComponent<MultipleTags>();

		if (tags.HasTag( "Wall" ))
        {
            data.status = ( side == CharacterSide.Floor || side == CharacterSide.Head );
            SendMessageUpwards( "UpdateStatus" , data );
        }
    }
}
