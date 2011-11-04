using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/Utility/Constant Rotation")]
public class ConstantRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public Vector3 localRotationAxis = Vector3.forward;
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.RotateAroundLocal( localRotationAxis , rotationSpeed * Time.deltaTime );
	}
}
