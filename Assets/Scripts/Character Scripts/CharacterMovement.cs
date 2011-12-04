using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/2D Character/Character Movement")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpriteAnimation))]
public class CharacterMovement : MonoBehaviour
{
	public bool usingController = false;
	public KeyCode leftMovementKey;
	public KeyCode rightMovementKey;
	public KeyCode jumpKey;

    public float leftMovementSpeed = 1.0f;
    public float rightMovementSpeed = 1.0f;
    public float airControlModifier = 0.25f;
	public float stoppingSpeed = 1.0f;

    public float jumpForce = 10.0f;
    private bool allowJump = false;
    private bool canJump = true; // This is true if the player is on the ground

    private bool leftKeyPressed = false;
    private bool rightKeyPressed = false;

	private SpriteAnimation spriteAnimation;
	
	private GameObject door;
	void Start ()
	{
		spriteAnimation = GetComponent<SpriteAnimation>();
		if (spriteAnimation == null)
			throw new UnityException("SpriteAnimation Exception - failed to find SpriteAnimation attached to " + gameObject.name + "."); // This should NEVER arise, but just in case...
		door = null; // just to be sure that this will work
	}
    // Handle user input here.
	void Update ()
	{
        airControlModifier = Mathf.Clamp( airControlModifier , 0.01f , 1.0f );
		if (!usingController)
		{
	        if (Input.GetKeyDown( jumpKey ))
	            allowJump = true;
			if (Input.GetKey(leftMovementKey))
			{
				if (spriteAnimation.GetState() != SpriteAnimation.State.Moving)
					spriteAnimation.MakeMoving();
	            if (spriteAnimation.GetDirection() != SpriteAnimation.Direction.Left)
	                spriteAnimation.MakeLeftDirection();
	            if (!leftKeyPressed)
	                leftKeyPressed = true;
			}
	        else if (Input.GetKey( rightMovementKey ))
	        {
	            if (spriteAnimation.GetState() != SpriteAnimation.State.Moving)
	                spriteAnimation.MakeMoving();
	            if (spriteAnimation.GetDirection() != SpriteAnimation.Direction.Right)
	                spriteAnimation.MakeRightDirection();
	            if (!rightKeyPressed)
	                rightKeyPressed = true;
	        }
	        else
	        {
	            if (spriteAnimation.GetState() != SpriteAnimation.State.Idle)
	                spriteAnimation.MakeIdle();
	        }
	
	        if (Input.GetKeyUp(leftMovementKey))
	            leftKeyPressed = false;
	        if (Input.GetKeyUp(rightMovementKey))
	            rightKeyPressed = false;
		}
		else
		{
			if (Input.GetButtonDown("Controller_Jump"))
			{
				allowJump = true;
			}
			float axis_value = Input.GetAxis("Left_Joystick_Horizontal");
			axis_value = Mathf.Clamp(axis_value, -1.0f, 1.0f);
			if (axis_value == 1.0f) // RIGHT
			{
	            if (spriteAnimation.GetState() != SpriteAnimation.State.Moving)
	                spriteAnimation.MakeMoving();
	            if (spriteAnimation.GetDirection() != SpriteAnimation.Direction.Right)
	                spriteAnimation.MakeRightDirection();
	            if (!rightKeyPressed)
	                rightKeyPressed = true;
			}
			
			else if (axis_value == -1.0f) // LEFT
			{
				if (spriteAnimation.GetState() != SpriteAnimation.State.Moving)
					spriteAnimation.MakeMoving();
	            if (spriteAnimation.GetDirection() != SpriteAnimation.Direction.Left)
	                spriteAnimation.MakeLeftDirection();
	            if (!leftKeyPressed)
	                leftKeyPressed = true;
			}
	        else
	        {
	            if (spriteAnimation.GetState() != SpriteAnimation.State.Idle)
	                spriteAnimation.MakeIdle();
				if (leftKeyPressed)
					leftKeyPressed = false;
				if (rightKeyPressed)
					rightKeyPressed = false;
	        }
		}
	}
    // Handle physical interactions here
    void FixedUpdate ()
	{
		if (door == null)
		{
	        if (spriteAnimation.GetState() == SpriteAnimation.State.Moving)
	        {
	            if (leftKeyPressed)
					                rigidbody.AddForce( Vector3.left * leftMovementSpeed, ForceMode.VelocityChange);
	            if (rightKeyPressed)
					rigidbody.AddForce( Vector3.right * rightMovementSpeed, ForceMode.VelocityChange);
	        }
	
	        if (canJump && allowJump)
	        {
	            allowJump = false;
				rigidbody.AddForce((rigidbody.velocity + Vector3.up).normalized * jumpForce * (rightKeyPressed || leftKeyPressed ? airControlModifier : 1.0f), ForceMode.VelocityChange);
	        }
		}
    }

    public void UpdateStatus ( CharacterCollisionData data )
    {
        if (data.side == CharacterSide.Floor)
            canJump = data.status;
    }
	
	public void ExitAnimation(GameObject to_door)
	{
		door = to_door;
	}
}