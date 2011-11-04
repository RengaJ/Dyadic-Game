using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/Environmental/Metal Block")]
public class MetalBlock : MonoBehaviour
{
    public enum DegreeOfFreedom { H_ONLY , V_ONLY , H_AND_V }
    public DegreeOfFreedom movementStyle = DegreeOfFreedom.V_ONLY;
    public Utility.Charge currentCharge = Utility.Charge.Uncharged;

    public float moveSpeed = 1.0f;
    public bool falls = true;
    public float fallSpeed = 1.0f;

    private Vector3 originalPosition = Vector3.zero;
    private Vector3 previousPosition = Vector3.zero;
    private Vector3 displacementVector = Vector3.zero;
    private bool canStillFall = false;
    private bool canStillRise = true;

    public bool unlocked = false;

    public Vector3 Displacement
    {
        get { return displacementVector; }
        set { displacementVector = value; }
    }

    public void Start ()
    {
        originalPosition = transform.position;
    }

    public void Unlock ()
    {
        unlocked = true;
        Debug.Log( name + " is unlocked!" );
    }

    public void Lock ()
    {
        unlocked = false;
    }

    public void Move ( Utility.MouseDirection direction )
    {
        switch (direction)
        {
            case Utility.MouseDirection.UP:
                IncreaseDisplacement( Vector3.up );
                break;
            case Utility.MouseDirection.DOWN:
                IncreaseDisplacement( Vector3.down );
                break;
            case Utility.MouseDirection.LEFT:
                IncreaseDisplacement( Vector3.left );
                break;
            case Utility.MouseDirection.RIGHT:
                IncreaseDisplacement( Vector3.right );
                break;
            case Utility.MouseDirection.UP_LEFT:
                IncreaseDisplacement( Vector3.up + Vector3.left );
                break;
            case Utility.MouseDirection.UP_RIGHT:
                IncreaseDisplacement( Vector3.up + Vector3.right );
                break;
            case Utility.MouseDirection.DOWN_LEFT:
                IncreaseDisplacement( Vector3.down + Vector3.left );
                break;
            case Utility.MouseDirection.DOWN_RIGHT:
                IncreaseDisplacement( Vector3.down + Vector3.right );
                break;
            case Utility.MouseDirection.NONE:
            default:
                break;
        }
    }

    public void Update ()
    {
        if (unlocked && canStillRise)
        {
            transform.position = Vector3.Lerp( transform.position , transform.position + Displacement , Time.deltaTime * moveSpeed );
            Vector3 travelled = Utility.ManhattanDistance( transform.position , previousPosition );
            DecreaseDisplacment( travelled );
            previousPosition = transform.position;
        }
        else
            UpdateDisplacement( Vector3.zero );
    }
    
    public void FixedUpdate()
    {
        if (!unlocked)
        {
            if (falls && canStillFall)
                transform.position = Vector3.Lerp( transform.position , originalPosition , Time.fixedDeltaTime * fallSpeed );
        }
    }

    public void UpdateDisplacement ( Vector3 displace )
    {
        Displacement = displace;
    }

    public void IncreaseDisplacement ( Vector3 additive )
    {
        if (movementStyle == DegreeOfFreedom.H_ONLY)
            Displacement += Utility.Mul( additive , new Vector3( 1 , 0 ) );
        if (movementStyle == DegreeOfFreedom.V_ONLY)
            Displacement += Utility.Mul( additive , new Vector3( 0 , 1 ) );
        if (movementStyle == DegreeOfFreedom.H_AND_V)
            Displacement += Utility.Mul( additive , new Vector3( 1 , 1 ) ); 
    }

    public void DecreaseDisplacment ( Vector3 removal )
    {
        if (movementStyle == DegreeOfFreedom.H_ONLY)
            Displacement -= Utility.Mul( removal , new Vector3( 1 , 0 ) );
        if (movementStyle == DegreeOfFreedom.V_ONLY)
            Displacement -= Utility.Mul( removal , new Vector3( 0 , 1 ) );
        if (movementStyle == DegreeOfFreedom.H_AND_V)
            Displacement -= Utility.Mul( removal , new Vector3( 1 , 1 ) ); 
    }

    public void OnTriggerEnter ( Collider other )
    {
        MultipleTags tags = other.collider.GetComponent<MultipleTags>();
        if (tags.HasTag( "Metal Plug" ))
        {
            canStillFall = false;
            originalPosition = transform.position;
        }
        if (tags.HasTag( "Metal Bolt" ))
            canStillRise = false;
    }
    public void OnTriggerExit ( Collider other )
    {
        MultipleTags tags = other.collider.GetComponent<MultipleTags>();
        if (tags.HasTag( "Metal Plug" ))
            canStillFall = true;
        if (tags.HasTag( "Metal Bolt" ))
            canStillRise = true;
    }
    public void OnTriggerStay ( Collider other )
    {
        MultipleTags tags = other.collider.GetComponent<MultipleTags>();
        if (tags.HasTag( "Metal Plug" ) && canStillFall)
            canStillFall = false;
        if (tags.HasTag( "Metal Bolt" ) && canStillRise)
            canStillRise = false;
    }
}
