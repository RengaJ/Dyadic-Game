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
	public float stoppingSpeed = 1.0f;

    public float jumpForce = 10.0f;
    private bool allowJump = false;

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
        if (Input.GetKeyDown( jumpKey ))
            allowJump = true;
		if (Input.GetKey(leftMovementKey))
		{
			if (spriteAnimation.GetState() != SpriteAnimation.State.Moving)
				spriteAnimation.MakeMoving();
            if (spriteAnimation.GetDirection() != SpriteAnimation.Direction.Left)
                spriteAnimation.MakeLeftDirection();
		}
        else if (Input.GetKey( rightMovementKey ))
        {
            if (spriteAnimation.GetState() != SpriteAnimation.State.Moving)
                spriteAnimation.MakeMoving();
            if (spriteAnimation.GetDirection() != SpriteAnimation.Direction.Right)
                spriteAnimation.MakeRightDirection();
        }
        else
        {
            if (spriteAnimation.GetState() != SpriteAnimation.State.Idle)
                spriteAnimation.MakeIdle();
        }
	}
    // Handle physical interactions here
    void FixedUpdate ()
	{
        if (spriteAnimation.GetState() == SpriteAnimation.State.Moving)
        {
			if (spriteAnimation.GetDirection() == SpriteAnimation.Direction.Left)
            	rigidbody.AddForce( Vector3.left * leftMovementSpeed * Time.fixedDeltaTime);
			if (spriteAnimation.GetDirection() == SpriteAnimation.Direction.Right)
            	rigidbody.AddForce( Vector3.right * rightMovementSpeed * Time.fixedDeltaTime);
        }

        if (allowJump)
        {
            allowJump = false;
            rigidbody.AddForce( Vector3.up * jumpForce );
        }
		
		if (spriteAnimation.GetState() == SpriteAnimation.State.Idle)
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