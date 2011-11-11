using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/Utility/Fade")]
public class Fade : MonoBehaviour
{
	public float fadeTime = 1.0f;
	private float alpha_value = 0.0f;
	public GameObject blackPlane;
	private bool fadeIn = false;
	private bool fadeOut = false;
	public float waitTime = 0.0f;
	private float currWaitTime = 0.0f;
	private bool waiting = false;
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (fadeIn || fadeOut)
		{
			if (fadeIn)
				alpha_value += Time.deltaTime / fadeTime;
			if (fadeOut)
				alpha_value -= Time.deltaTime / fadeTime;
			alpha_value = Mathf.Clamp01(alpha_value);
			Utility.FadeChildren(blackPlane,alpha_value);
			
			if (fadeOut && alpha_value == 0.0f)
				fadeOut = false;
			if (fadeIn && alpha_value == 1.0f)
			{
				fadeIn = false;
				waiting = true;
			}
		}
		if (waiting)
		{
			if (currWaitTime < waitTime)
				currWaitTime += Time.deltaTime;
			else
			{
				waiting = false;
				fadeOut = true;
				currWaitTime = 0.0f;
			}
		}
	}
	
	public void Begin()
	{
		fadeIn = true;
		fadeOut = false;
	}
	
}
