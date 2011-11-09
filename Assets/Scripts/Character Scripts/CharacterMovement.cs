using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/2D Character/Character Movement")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpriteAnimation))]
public class CharacterMovement : MonoBehaviour
{
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
	
	private float leftAcceleration = 0.0f;
	private float rightAcceleration = 0.0f;
	private float jumpAcceleration = 0.0f;

	private SpriteAnimation spriteAnimation;
	void Start ()
	{
		spriteAnimation = GetComponent<SpriteAnimation>();
		if (spriteAnimation == null)
			throw new UnityException("SpriteAnimation Exception - failed to find SpriteAnimation attached to " + gameObject.name + "."); // This should NEVER arise, but just in case...
	}
    // Handle user input here.
	void Update ()
	{
        airControlModifier = Mathf.Clamp( airControlModifier , 0.01f , 1.0f );
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
    // Handle physical interactions here
    void FixedUpdate ()
	{
		Vector3 movement = Vector3.zero;
        if (spriteAnimation.GetState() == SpriteAnimation.State.Moving)
        {
            if (leftKeyPressed)
			{
				                rigidbody.AddForce( Vector3.left * leftMovementSpeed, ForceMode.VelocityChange);
//				movement.x -= leftMovementSpeed * Time.fixedDeltaTime * (canJump ? 1.0f : airControlModifier);
				//GetComponent<CharacterController>().Move(Vector3.left * leftMovementSpeed * Time.fixedDeltaTime * (canJump ? 1.0f : airControlModifier));
			}
                //rigidbody.velocity += Vector3.left * leftMovementSpeed * Time.fixedDeltaTime * (canJump ? 1.0f : airControlModifier);
            if (rightKeyPressed)
			{
				rigidbody.AddForce( Vector3.right * rightMovementSpeed, ForceMode.VelocityChange);
				//movement.x += rightMovementSpeed * Time.fixedDeltaTime * (canJump ? 1.0f : airControlModifier);
				//GetComponent<CharacterController>().Move(Vector3.right * rightMovementSpeed * Time.fixedDeltaTime * (canJump ? 1.0f : 
			}
                //rigidbody.velocity += Vector3.right * rightMovementSpeed * Time.fixedDeltaTime * ( canJump ? 1.0f : airControlModifier );
        }

        if (canJump && allowJump)
        {
            allowJump = false;
//			movement.y = (jumpForce) * Time.fixedDeltaTime;
//            rigidbody.velocity += (rigidbody.velocity + Vector3.up).normalized * jumpForce * (rightKeyPressed || leftKeyPressed ? airControlModifier : 1.0f);
			rigidbody.AddForce((rigidbody.velocity + Vector3.up).normalized * jumpForce * (rightKeyPressed || leftKeyPressed ? airControlModifier : 1.0f), ForceMode.VelocityChange);
        }
		
		//movement.y += Physics.gravity.y/4.0f * Time.deltaTime;
		
		//GetComponent<CharacterController>().Move(movement);
		
		if (spriteAnimation.GetState() == SpriteAnimation.State.Idle)
		{
		//	if (rigidbody.velocity.magnitude > 0.1f)
		//	{
		//		Vector3 remainingVelocity = Vector3.zero - rigidbody.velocity;
		//		remainingVelocity *= stoppingSpeed * Time.fixedDeltaTime;
			
		//		rigidbody.velocity += remainingVelocity;
		//	}
		}
    }

    public void UpdateStatus ( CharacterCollisionData data )
    {
        if (data.side == CharacterSide.Floor)
            canJump = data.status;
    }
}