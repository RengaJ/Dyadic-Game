using UnityEngine;
using System.Collections;

public class TextureAnimation : MonoBehaviour 
{
	public Texture2D[] frames;
	public float cycleTime = 0.0f;
	public float waitTime = 0.0f;
	
	private bool wait = false;
	private int currFrame = 0;
	private float frameTime = 0.0f;
	private float currFrameTime = 0.0f;
	private float currWaitTime = 0.0f;
	
	public void Start()
	{
		if (frames.Length == 0)
			throw new UnityException("Texture Animation Exception - frames must contain at least one frame of animation!");
		
		frameTime = cycleTime/(float)frames.Length;
	}
	
	public void Update()
	{
		if (!wait)
		{
			if (currFrameTime < frameTime)
			{
				currFrameTime += Time.deltaTime;
				return;
			}
			currFrameTime = 0.0f;
			currFrame = (currFrame + 1)%frames.Length;
			GetComponent<MeshRenderer>().material.mainTexture = frames[currFrame];
			if (currFrame == 0)
			{
				wait = true;
				currWaitTime = 0.0f;
				return;
			}
		}
		if (currWaitTime < waitTime)
			currWaitTime += Time.deltaTime;
		else
			wait = false;
	}
}
