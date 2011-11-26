using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/2D Character/Sprite Animation")]
public class SpriteAnimation : MonoBehaviour
{
	public enum State { Moving, Jumping, Idle, Action };
	public enum Direction { Left, Right };
	private int currentRightIdleFrame = 0;
	private int currentLeftIdleFrame = 0;
	//private int currentJumpFrame = 0;
	private int currentLeftMovementFrame = -1;
	private int currentRightMovementFrame = -1;
	
	// Internal direction and state inforamtion
	private Direction currentDirection = Direction.Right;
	private State currentState = State.Idle;
	
	// Left Idle animation timing values
	public float RightIdleLoopTime = 1.0f;
	private float RightIdleFrameTime = 0.0f;
	private float currentRightIdleTime = 0.0f;
	// Left Idle animation timing values
	public float LeftIdleLoopTime = 1.0f;
	private float LeftIdleFrameTime = 0.0f;
	private float currentLeftIdleTime = 0.0f;
	// Left Movement animation timing values
	public float LeftMoveLoopTime = 1.0f;
	private float leftMoveFrameTime = 0.0f;
	private float currentLeftMoveTime = 0.0f;
	// Right Movement animation timing values
	public float RightMoveLoopTime = 1.0f;
	private float rightMoveFrameTime = 0.0f;
	private float currentRightMoveTime = 0.0f;
	
	public Texture2D[] rightIdleAnimation;
	public Texture2D[] leftIdleAnimation;
	public Texture2D[] leftMoveAnimation;
	public Texture2D[] rightMoveAnimation;
	//public Texture2D[] jumpAnimation;
	// Use this for initialization
	void Start ()
	{
		if (rightIdleAnimation.Length > 0)
		{
			GetComponent<MeshRenderer>().material.mainTexture = rightIdleAnimation[currentRightIdleFrame];
            GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2( -1 , -1 ); // This ensures proper xyz axis orientation (or close enough) with proper sprite orientation
			RightIdleFrameTime = RightIdleLoopTime / (float)rightIdleAnimation.Length;
		}
		else
			throw new UnityException("Animation Exception - Right Idle Animation must contain at least one frame of animation.");
		if (leftIdleAnimation.Length > 0)
			LeftIdleFrameTime = LeftIdleLoopTime / (float)leftIdleAnimation.Length;
		else
			throw new UnityException("Animation Exception - Left Idle Animation must contain at least one frame of animation.");
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
				if (leftMoveAnimation.Length == 1)
				{
					Texture2D mainTexture =  (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
					if (mainTexture == leftMoveAnimation[0])
						return;
					GetComponent<MeshRenderer>().material.mainTexture = leftMoveAnimation[0];
				}
				else
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
			}
			if (currentDirection == Direction.Right)
			{
				if (rightMoveAnimation.Length == 1)
				{
					Texture2D mainTexture =  (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
					if (mainTexture == rightMoveAnimation[0])
						return;
					GetComponent<MeshRenderer>().material.mainTexture = rightMoveAnimation[0];
				}
				else
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
		}
		if (currentState == State.Idle)
		{
			if (currentDirection == Direction.Right)
			{
				if (currentRightIdleTime >= RightIdleFrameTime)
				{
					currentRightIdleFrame = (currentRightIdleFrame + 1) % rightIdleAnimation.Length;
					currentRightIdleTime = 0.0f; // Reset currentRightIdleTime for next frame of animation
					GetComponent<MeshRenderer>().material.mainTexture = rightIdleAnimation[currentRightIdleFrame];
				}
				else
					currentRightIdleTime += Time.smoothDeltaTime;
			}
			if (currentDirection == Direction.Left)
			{
				if (currentLeftIdleTime >= LeftIdleFrameTime)
				{
					currentLeftIdleFrame = (currentLeftIdleFrame + 1) % leftIdleAnimation.Length;
					currentLeftIdleTime = 0.0f; // Reset currentLeftIdleTime for next frame of animation
					GetComponent<MeshRenderer>().material.mainTexture = leftIdleAnimation[currentLeftIdleFrame];
				}
				else
					currentLeftIdleTime += Time.smoothDeltaTime;
			}
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
        currentRightIdleTime = RightIdleLoopTime;
		currentLeftIdleTime = LeftIdleLoopTime;
        currentRightMoveTime = RightMoveLoopTime;
        currentLeftMoveTime = LeftMoveLoopTime;
    }
    public void ResetFrames ()
    {
        currentRightIdleFrame = -1;
		currentLeftIdleFrame = -1;
        currentRightMovementFrame = -1;
        currentLeftMovementFrame = -1;
    }
}