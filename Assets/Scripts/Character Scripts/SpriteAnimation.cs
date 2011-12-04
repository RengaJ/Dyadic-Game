using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/2D Character/Sprite Animation")]
public class SpriteAnimation : MonoBehaviour
{
	public enum Mode { Standard, Special };              // What animation mode are we in? ( Standard is plain, Special is a secondary mode)
	public enum State { Moving, Jumping, Idle, Action }; // Movement , Jump , Idle and Casting Animation States
	public enum Direction { Left, Right };               // What direction are we facing?
	
	public bool useSpecialMode = false;
	private int currentRightIdleFrame = 0;
	private int currentLeftIdleFrame = 0;
	//private int currentJumpFrame = 0;
	private int currentLeftMovementFrame = -1;
	private int currentRightMovementFrame = -1;
	private int currentLeftJumpFrame = -1;
	private int currentRightJumpFrame = -1;
	private int currentLeftCastFrame = -1;
	private int currentRightCastFrame = -1;
	
	// Internal direction and state inforamtion
	private Direction currentDirection = Direction.Right;
	private State currentState = State.Idle;
	private Mode currentMode = Mode.Standard;
	// Previous State Information
	private State previousState = State.Idle;
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
	// Right Jump animation timing values
	public float RightJumpLoopTime = 1.0f;
	private float rightJumpFrameTime = 0.0f;
	private float currentRightJumpFrameTime = 0.0f;
	// Left Jump animation timing values
	public float LeftJumpLoopTime = 1.0f;
	private float leftJumpFrameTime = 0.0f;
	private float currentLeftJumpFrameTime = 0.0f;
	// Right Casting animation timing values
	public float RightCastLoopTime = 1.0f;
	private float rightCastFrameTime = 0.0f;
	private float currentRightCastFrameTime = 0.0f;
	// Left Casting animation timing values
	public float LeftCastLoopTime = 1.0f;
	private float leftCastFrameTime = 0.0f;
	private float currentLeftCastFrameTime = 0.0f;
	
	public Texture2D[] rightIdleAnimation;
	public Texture2D[] leftIdleAnimation;
	public Texture2D[] leftMoveAnimation;
	public Texture2D[] rightMoveAnimation;
	public Texture2D[] rightJumpAnimation;
	public Texture2D[] leftJumpAnimation;
	public Texture2D[] rightCastAnimation;
	public Texture2D[] leftCastAnimation;
	
	public Texture2D[] specialRightIdleAnimation;
	public Texture2D[] specialLeftIdleAnimation;
	public Texture2D[] specialLeftMoveAnimation;
	public Texture2D[] specialRightMoveAnimation;
	public Texture2D[] specialRightJumpAnimation;
	public Texture2D[] specialLeftJumpAnimation;
	public Texture2D[] specialRightCastAnimation;
	public Texture2D[] specialLeftCastAnimation;
	
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
		if (rightJumpAnimation.Length > 0)
			rightJumpFrameTime = RightJumpLoopTime / (float) rightJumpAnimation.Length;
		else
			throw new UnityException("Animation Exception - Right Jump Animation must contain at least one frame of animation.");
		if (leftJumpAnimation.Length > 0)
			leftJumpFrameTime = LeftJumpLoopTime / (float) leftJumpAnimation.Length;
		else
			throw new UnityException("Animation Exception - Left Jump Animation must contain at least one frame of animation.");
		if (rightCastAnimation.Length > 0)
			rightCastFrameTime = RightCastLoopTime / (float) rightCastAnimation.Length;
		else
			throw new UnityException("Animation Exception - Right Casting Animation must contain at least one frame of animation.");
		if (leftCastAnimation.Length > 0)
			leftCastFrameTime = LeftCastLoopTime / (float) leftCastAnimation.Length;
		else
			throw new UnityException("Animation Exception - Left Casting Animation must contain at least one frame of animation.");
		
		if (useSpecialMode)
		{
			if (specialRightIdleAnimation.Length != rightIdleAnimation.Length)
				throw new UnityException("Animation Exception - Special Right Idle Animation must have the same number of frames as Right Idle Animation.");
			if (specialLeftIdleAnimation.Length != leftIdleAnimation.Length)
				throw new UnityException("Animation Exception - Special Left Idle Animation must have the same number of frames as Left Idle Animation.");
			if (specialLeftMoveAnimation.Length != leftMoveAnimation.Length)
				throw new UnityException("Animation Exception - Special Left Move Animation must have the same number of frames as Left Move Animation.");
			if (specialRightMoveAnimation.Length != rightMoveAnimation.Length)
				throw new UnityException("Animation Exception - Special Right Move Animation must have the same number of frames as Right Move Animation.");
			if (specialRightJumpAnimation.Length != rightJumpAnimation.Length)
				throw new UnityException("Animation Exception - Special Right Jump Animation must have the same number of frames as Right Jump Animation.");
			if (specialLeftJumpAnimation.Length != leftJumpAnimation.Length)
				throw new UnityException("Animation Exception - Special Left Jump Animation must have the same number of frames as Left Jump Animation.");
			if (specialRightCastAnimation.Length != rightCastAnimation.Length)
				throw new UnityException("Animation Exception - Special Right Casting Animation must have the same number of frames as Left Casting Animation.");
			if (specialLeftCastAnimation.Length != leftCastAnimation.Length)
				throw new UnityException("Animation Exception - Special Left Casting Animation must have the same number of frames as Right Casting Animation.");
		}
		
		ResetTimes();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentState == State.Action) // Casting
		{
			if (currentDirection == Direction.Left)
			{
				if (leftCastAnimation.Length == 1)
				{
					Texture2D mainTexture = (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
					if (currentMode == Mode.Standard)
					{
						if (mainTexture == leftCastAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = leftCastAnimation[0];
					}
					else
					{
						if (mainTexture == specialLeftCastAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = specialLeftCastAnimation[0];
					}
				}
				else
				{
					if (currentLeftCastFrameTime >= leftCastFrameTime)
					{
						currentLeftCastFrame = (currentLeftCastFrame + 1) % leftCastAnimation.Length;
						currentLeftCastFrameTime = 0.0f;
						if (currentMode == Mode.Standard)
							GetComponent<MeshRenderer>().material.mainTexture = leftCastAnimation[currentLeftCastFrame];
						else
							GetComponent<MeshRenderer>().material.mainTexture = specialLeftCastAnimation[currentLeftCastFrame];
					}
					else
						currentLeftCastFrameTime += Time.smoothDeltaTime;
				}
			}
			if (currentDirection == Direction.Right)
			{
				if (rightCastAnimation.Length == 1)
				{
					Texture2D mainTexture = (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
					if (currentMode == Mode.Standard)
					{
						if (mainTexture == rightCastAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = rightCastAnimation[0];
					}
					else
					{
						if (mainTexture == specialRightCastAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = specialRightCastAnimation[0];
					}
				}
				else
				{
					if (currentRightCastFrameTime >= rightCastFrameTime)
					{
						currentRightCastFrame = (currentRightCastFrame + 1) % rightCastAnimation.Length;
						currentRightCastFrameTime = 0.0f;
						if (currentMode == Mode.Standard)
							GetComponent<MeshRenderer>().material.mainTexture = rightCastAnimation[currentRightCastFrame];
						else
							GetComponent<MeshRenderer>().material.mainTexture = specialRightCastAnimation[currentRightCastFrame];
					}
					else
						currentRightCastFrameTime += Time.smoothDeltaTime;
				}
			}
		}
		if (currentState == State.Jumping)
		{
			if (currentDirection == Direction.Left)
			{
				if (leftJumpAnimation.Length == 1)
				{
					Texture2D mainTexture = (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
					if (currentMode == Mode.Standard)
					{
						if (mainTexture == leftJumpAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = leftJumpAnimation[0];
					}
					else
					{
						if (mainTexture == specialLeftJumpAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = specialLeftJumpAnimation[0];
					}
				}
				else
				{
					if (currentLeftJumpFrameTime >= leftJumpFrameTime)
					{
						currentLeftJumpFrame = (currentLeftJumpFrame + 1) % leftJumpAnimation.Length;
						currentLeftJumpFrameTime = 0.0f;
						if (currentMode == Mode.Standard)
							GetComponent<MeshRenderer>().material.mainTexture = leftJumpAnimation[currentLeftJumpFrame];
						else
							GetComponent<MeshRenderer>().material.mainTexture = specialLeftJumpAnimation[currentLeftJumpFrame];
					}
					else
						currentLeftJumpFrameTime += Time.smoothDeltaTime;
				}
			}
			if (currentDirection == Direction.Right)
			{
				if (rightJumpAnimation.Length == 1)
				{
					Texture2D mainTexture = (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
					if (currentMode == Mode.Standard)
					{
						if (mainTexture == rightJumpAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = rightJumpAnimation[0];
					}
					else
					{
						if (mainTexture == specialRightJumpAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = specialRightJumpAnimation[0];
					}
				}
				else
				{
					if (currentRightJumpFrameTime >= rightJumpFrameTime)
					{
						currentRightJumpFrame = (currentRightJumpFrame + 1) % rightJumpAnimation.Length;
						currentRightJumpFrameTime = 0.0f;
						if (currentMode == Mode.Standard)
							GetComponent<MeshRenderer>().material.mainTexture = rightJumpAnimation[currentRightJumpFrame];
						else
							GetComponent<MeshRenderer>().material.mainTexture = specialRightJumpAnimation[currentRightJumpFrame];
					}
					else
						currentRightJumpFrameTime += Time.smoothDeltaTime;
				}
			}
		}
		if (currentState == State.Moving)
		{
			if (currentDirection == Direction.Left)
			{
				if (leftMoveAnimation.Length == 1)
				{
					Texture2D mainTexture =  (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
					if (currentMode == Mode.Standard)
					{
						if (mainTexture == leftMoveAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = leftMoveAnimation[0];
					}
					else
					{
						if (mainTexture == specialLeftMoveAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = specialLeftMoveAnimation[0];
					}
				}
				else
				{
					if (currentLeftMoveTime >= leftMoveFrameTime)
					{
						currentLeftMovementFrame = (currentLeftMovementFrame + 1) % leftMoveAnimation.Length;
						currentLeftMoveTime = 0.0f;
						if (currentMode == Mode.Standard)
							GetComponent<MeshRenderer>().material.mainTexture = leftMoveAnimation[currentLeftMovementFrame];
						else
							GetComponent<MeshRenderer>().material.mainTexture = specialLeftMoveAnimation[currentLeftMovementFrame];
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
					if (currentMode == Mode.Standard)
					{
						if (mainTexture == rightMoveAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = rightMoveAnimation[0];
					}
					else
					{
						if (mainTexture == specialRightMoveAnimation[0])
							return;
						GetComponent<MeshRenderer>().material.mainTexture = specialRightMoveAnimation[0];
					}
				}
				else
				{
					if (currentRightMoveTime >= rightMoveFrameTime)
					{
						currentRightMovementFrame = (currentRightMovementFrame + 1) % rightMoveAnimation.Length;
						currentRightMoveTime = 0.0f;
						if (currentMode == Mode.Standard)
							GetComponent<MeshRenderer>().material.mainTexture = rightMoveAnimation[currentRightMovementFrame];
						else
							GetComponent<MeshRenderer>().material.mainTexture = specialRightMoveAnimation[currentRightMovementFrame];
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
					if (currentMode == Mode.Standard)
						GetComponent<MeshRenderer>().material.mainTexture = rightIdleAnimation[currentRightIdleFrame];
					else
						GetComponent<MeshRenderer>().material.mainTexture = specialRightIdleAnimation[currentRightIdleFrame];
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
					if (currentMode == Mode.Standard)
						GetComponent<MeshRenderer>().material.mainTexture = leftIdleAnimation[currentLeftIdleFrame];
					else
						GetComponent<MeshRenderer>().material.mainTexture = specialLeftIdleAnimation[currentLeftIdleFrame];
				}
				else
					currentLeftIdleTime += Time.smoothDeltaTime;
			}
		}
	}
	// State change functions
	public void SetState(State state) {	previousState = currentState; currentState = state; }
    public void MakeIdle ()           { SetState(State.Idle); ResetFrames(); ResetTimes(); }
	public void MakeMoving()          { SetState(State.Moving); }
	public void MakeJumping()         { SetState(State.Jumping); }
	public void MakeAction()          { SetState(State.Action); }
	
	public void MakePreviousState()	  { SetState(previousState); }
	// Direction change functions
	public void SetDirection(Direction direction)
	                                 { currentDirection = direction; }
	public void MakeLeftDirection()  { currentDirection = Direction.Left;  currentRightMoveTime = RightMoveLoopTime; currentRightMovementFrame = -1; }
	public void MakeRightDirection() { currentDirection = Direction.Right;  currentLeftMoveTime = LeftMoveLoopTime;  currentLeftMovementFrame = -1; }
	
	// Mode change functions
	public void SetMode(Mode mode) { currentMode = mode; }
	public void MakeStandard()     { currentMode = Mode.Standard; }
	public void MakeSpecial()      { currentMode = Mode.Special; }
	
	// Get functions
	public State     GetState()			{ return currentState; }
	public State	 GetPreviousState() { return previousState; }
	public Direction GetDirection()		{ return currentDirection; }
	public Mode		 GetMode()			{ return currentMode; }

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