using UnityEngine;
using System.Collections;

public class EarthMover : MonoBehaviour {
/*	public KeyCode leftMovementKey;
	public KeyCode rightMovementKey;
	public KeyCode upMovementKey;
	public KeyCode downMovementKey; */
/*	public KeyCode activateKey; */
	
	public float movementSpeed = 1.0f;
	public float stoppingSpeed = 1.0f;
	
	public EarthMovement em;
	
	private bool left = false;
	private bool right = false;
	private bool up = false;
	private bool down = false;
	
	// Use this for initialization
	void Start () {
		em = null;
		this.rigidbody.useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
		float joystick_vertical = Input.GetAxis("Right_Joystick_Vertical");
		float joystick_horizontal = Input.GetAxis("Right_Joystick_Horizontal");
		
		Debug.Log("(" + joystick_horizontal + ", " + joystick_vertical + ")");
		
		if (joystick_vertical == -1)
		{
			up = true;
			down = false;
		}
		else if (joystick_vertical == 1)
		{
			down = true;
			up = false;
		}
		else
		{
			down = false;
			up = false;
		}
		
		if (joystick_horizontal == 1) // right
		{
			right = true;
			left = false;
		}
		else if (joystick_horizontal == -1) // lefty
		{
			left = true;
			right = false;
		}
		else
		{
			left = false;
			right = false;
		}
		
		if (Input.GetButtonDown("Controller_Activate_Cursor"))	
		{
			if (em != null)
				em.Activate();
		}
		/*
		if (Input.GetKeyDown(activateKey))
		{
			Debug.Log("ACTIVATE");
			if(em != null)
			{
				em.Activate();
				Debug.Log("ACTIVATED");
			}
		}
		
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
		}	*/
	}
	
	void FixedUpdate () {
		if(up||down||left||right)
		{
			if(up)
			{
				rigidbody.AddForce( Vector3.up * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
			}
			else if(down)
			{
				rigidbody.AddForce( Vector3.down * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
			}
			
			if(left)
			{
				rigidbody.AddForce( Vector3.left * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
			}
			else if(right)
			{
				rigidbody.AddForce( Vector3.right * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
			}
		}
		/*else
		{
			if (rigidbody.velocity.magnitude > 0.1f)
			{
				Vector3 remainingVelocity = Vector3.zero - rigidbody.velocity;
				remainingVelocity *= stoppingSpeed * Time.fixedDeltaTime;
			
				rigidbody.velocity += remainingVelocity;
			}
		} */
	}
	
	public void SetEarth (EarthMovement e) {
		if(em != null)
		{
			em.DeActivate();
		}
		em = e;
	}
}
