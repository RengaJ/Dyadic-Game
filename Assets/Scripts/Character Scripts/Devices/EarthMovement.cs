using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class EarthMovement : MonoBehaviour {

	public KeyCode leftMovementKey;
	public KeyCode rightMovementKey;
	public KeyCode upMovementKey;
	public KeyCode downMovementKey;
	
	public float movementSpeed = 1.0f;
	public float stoppingSpeed = 1.0f;
	
	public bool active;
	
	private bool left = false;
	private bool right = false;
	private bool up = false;
	private bool down = false;
	
	void Start () {
	}
	
	void Update () {
		if(active)
		{
			if (Input.GetKey(leftMovementKey))
			{
				left = true;
				right = false;
			}
			else if (Input.GetKey(rightMovementKey))
			{
				right = true;
				left = false;
			}
			else
			{
				right = false;
				left = false;
			}
			
			if (Input.GetKey(downMovementKey))
			{
				down = true;
				up = false;
			}
			else if (Input.GetKey(upMovementKey))
			{
				up = true;
				down = false;
			}
			else
			{
				up = false;
				down = false;
			}
		}
	}
	
	public void Activate(){
		if(active)
		{
			this.rigidbody.useGravity = true;
			active = false;
		}
		else
		{
			this.rigidbody.useGravity = false;
			active = true;
		}
	}
	
	public void DeActivate(){
		active = false;
		this.rigidbody.useGravity = true;
	}
	
	void FixedUpdate () {
		if(up||down||left||right)
		{
			if(up)
			{
				rigidbody.AddForce( Vector3.up * movementSpeed * Time.fixedDeltaTime);
			}
			else if(down)
			{
				rigidbody.AddForce( Vector3.down * movementSpeed * Time.fixedDeltaTime);
			}
			
			if(left)
			{
				rigidbody.AddForce( Vector3.left * movementSpeed * Time.fixedDeltaTime);
			}
			else if(right)
			{
				rigidbody.AddForce( Vector3.right * movementSpeed * Time.fixedDeltaTime);
			}
		}
		else
		{
			if (rigidbody.velocity.magnitude > 0.1f)
			{
				Vector3 remainingVelocity = Vector3.zero - rigidbody.velocity;
				remainingVelocity *= stoppingSpeed * Time.fixedDeltaTime;
			
				rigidbody.velocity += remainingVelocity;
			}
		}
	}
}
