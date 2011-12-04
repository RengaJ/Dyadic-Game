using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/2D Character/Devices/Character Ability Base (DO NOT USE)")]
public class CharacterDevice : MonoBehaviour
{
    public Camera cameraScreen;

    public GameObject gunObject;

    // Firing Variables
    public bool fired = false;

    // view cone variables
    public int rayCount = 3;
    public float angle = 45.0f;
    public bool limitAngles = false; /// Constrain the angle of rotation?
    public float minRotationAngle = 0.0f; // Lower bound of angle constraint
    public float maxRotationAngle = 60.0f; // Upper bound of angle constraint
    protected float halfAngle = 0.0f;
    protected float angleDivisions = 0.0f;
    protected float maxWidth = 0.0f;

    public bool flipCharacter = false;

    protected bool flip = false;
    protected Vector3 mouseDirection = Vector3.zero;

    public virtual void FixedUpdate()
    {
        if (fired)
            fired = false;
    }
	public virtual void Update ()
    {
        Vector3 mousePos;
        mousePos = cameraScreen.camera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        /** KEEP BEGIN **/
        mousePos.z = transform.position.z;
        mouseDirection = mousePos - transform.position;
        if (gunObject != null)
        {
            Quaternion target = Quaternion.LookRotation(-mouseDirection.normalized);
            gunObject.transform.rotation = Quaternion.Slerp( gunObject.transform.rotation, target, 10.0f*Time.deltaTime );
        }
        //Debug.DrawLine(transform.position, mousePos, Color.red);
        float angleOffset = Vector3.Angle(mouseDirection, transform.forward);
        angleDivisions = angle/(float)(rayCount-1);
        halfAngle = angle * 0.5f;
        maxWidth = 40 * Mathf.Tan(halfAngle * Mathf.Deg2Rad);
        if (mousePos.y < transform.position.y)
        {
            halfAngle -= angleOffset;

            if (limitAngles && halfAngle - (rayCount / 2) * angleDivisions < minRotationAngle)
                halfAngle = minRotationAngle + ((rayCount / 2) * angleDivisions);
        }
        else
        {
            halfAngle += angleOffset;
            if (limitAngles && halfAngle - (rayCount / 2) * angleDivisions > maxRotationAngle)
                halfAngle = maxRotationAngle + ((rayCount / 2) * angleDivisions);
        }
        int side = 1;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 angledVector = new Vector3(side*Mathf.Cos((halfAngle - (angleDivisions *i))*Mathf.Deg2Rad),
                                               Mathf.Sin((halfAngle - (angleDivisions *i))*Mathf.Deg2Rad));
            angledVector *= 50;
            angledVector += transform.forward;
            Debug.DrawRay(transform.position, angledVector, Color.cyan);
        }
        /** KEEP END **/
        if (Input.GetButtonUp("Fire1"))
            fired = true;
  /*      if (flipCharacter)
        {
            if (Input.GetAxis("CharHorizontal") < 0 && !flipped)
            {
                flipped = true;
                flip = true;
            }
            else if (Input.GetAxis("CharHorizontal") > 0 && !flipped)
            {
                flipped = true;
                flip = false;
            }
            else
                flipped = false;
        } */
	}
}