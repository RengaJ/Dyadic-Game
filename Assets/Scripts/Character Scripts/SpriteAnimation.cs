using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/2D Character/Sprite Animation")]
public class SpriteAnimation : MonoBehaviour
{
	public enum State { Moving, Jumping, Idle, Action };
	public enum Direction { Left, Right };
	private int currentIdleFrame = 0;
	//private int currentJumpFrame = 0;
	private int currentLeftMovementFrame = -1;
	private int currentRightMovementFrame = -1;
	
	// Internal direction and state inforamtion
	private Direction currentDirection = Direction.Right;
	private State currentState = State.Idle;
	
	// Idle animation timing values
	public float IdleLoopTime = 1.0f;
	private float idleFrameTime = 0.0f;
	private float currentIdleTime = 0.0f;
	// Left Movement animation timing values
	public float LeftMoveLoopTime = 1.0f;
	private float leftMoveFrameTime = 0.0f;
	private float currentLeftMoveTime = 0.0f;
	// Right Movement animation timing values
	public float RightMoveLoopTime = 1.0f;
	private float rightMoveFrameTime = 0.0f;
	private float currentRightMoveTime = 0.0f;
	
	public Texture2D[] idleAnimation;
	public Texture2D[] leftMoveAnimation;
	public Texture2D[] rightMoveAnimation;
	//public Texture2D[] jumpAnimation;
	// Use this for initialization
	void Start ()
	{
		if (idleAnimation.Length > 0)
		{
			GetComponent<MeshRenderer>().material.mainTexture = idleAnimation[currentIdleFrame];
            GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2( -1 , -1 ); // This ensures proper xyz axis orientation (or close enough) with proper sprite orientation
			idleFrameTime = IdleLoopTime / (float)idleAnimation.Length;
		}
		else
			throw new UnityException("Animation Exception - Idle Animation must contain at least one frame of animation.");
		if (leftMoveAnimation.Length > 0)
			leftMoveFrameTime = LeftMoveLoopTime / (float) leftMoveAnimation.Length;
		else
			throw new UnityException("Animation Exception - Left Move Animation must contain at least one frame of animation.");
		if (rightMoveAnimation.Length > 0)
			rightMoveFrameTime = RightMoveLoopTime / (float) rightMoveAnimation.Length;
		else
			throw new UnityException("Animation Exception - Right Move Animation must contain at least one frame of animation.");
		
		ResetTimes();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentState == State.Action)
		{
		}
		if (currentState == State.Jumping)
		{
		}
		if (currentState == State.Moving)
		{
			if (currentDirection == Direction.Left)
			{
				if (currentLeftMoveTime >= leftMoveFrameTime)
				{
					currentLeftMovementFrame = (currentLeftMovementFrame + 1) % leftMoveAnimation.Length;
					currentLeftMoveTime = 0.0f;
					GetComponent<MeshRenderer>().material.mainTexture = leftMoveAnimation[currentLeftMovementFrame];
				}
				else
					currentLeftMoveTime += Time.smoothDeltaTime;
			}
			if (currentDirection == Direction.Right)
			{
				if (currentRightMoveTime >= rightMoveFrameTime)
				{
					currentRightMovementFrame = (currentRightMovementFrame + 1) % rightMoveAnimation.Length;
					currentRightMoveTime = 0.0f;
					GetComponent<MeshRenderer>().material.mainTexture = rightMoveAnimation[currentRightMovementFrame];
				}
				else
					currentRightMoveTime += Time.smoothDeltaTime;
			}
		}
		if (currentState == State.Idle)
		{
			if (currentIdleTime >= idleFrameTime)
			{
				currentIdleFrame = (currentIdleFrame + 1) % idleAnimation.Length;
				currentIdleTime = 0.0f; // Reset currentIdleTime for next frame of animation
				GetComponent<MeshRenderer>().material.mainTexture = idleAnimation[currentIdleFrame];
			}
			else
				currentIdleTime += Time.smoothDeltaTime;
		}
	}
	// State change functions
	public void SetState(State state) {	currentState = state; }
    public void MakeIdle ()           { currentState = State.Idle; ResetFrames(); ResetTimes(); }
	public void MakeMoving()          { currentState = State.Moving; }
	public void MakeJumping()         { currentState = State.Jumping; }
	public void MakeAction()          { currentState = State.Action; }
	// Direction change functions
	public void SetDirection(Direction direction)
	                                 { currentDirection = direction; }
	public void MakeLeftDirection()  { currentDirection = Direction.Left;  currentRightMoveTime = RightMoveLoopTime; currentRightMovementFrame = -1; }
	public void MakeRightDirection() { currentDirection = Direction.Right;  currentLeftMoveTime = LeftMoveLoopTime;  currentLeftMovementFrame = -1; }
	
	public State     GetState()      { return currentState; }
	public Direction GetDirection()  { return currentDirection; }

    // Utility functions
    public void ResetTimes()
    {
        currentIdleTime = IdleLoopTime;
        currentRightMoveTime = RightMoveLoopTime;
        currentLeftMoveTime = LeftMoveLoopTime;
    }
    public void ResetFrames ()
    {
        currentIdleFrame = -1;
        currentRightMovementFrame = -1;
        currentLeftMovementFrame = -1;
    }
}