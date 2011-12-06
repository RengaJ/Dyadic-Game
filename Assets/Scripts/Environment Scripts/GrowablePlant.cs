using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class GrowablePlant: MonoBehaviour 
{
	private Vector3 startPosition;
	public Vector3 endPosition;
	private Vector3 toMove;
	private bool fullyGrown = false;
	public float growsmoothing = 10f;
	private bool growing = false;
	private bool withering = false;
	
	// Use this for initialization
	void Start () 
	{
			startPosition = transform.position;
		if(fullyGrown == true)
		{
			growing = true;
		//	doBehavior();
		}
	}
	void OnTriggerStay(Collider ci)
	{
		if(ci.collider.gameObject.tag == "Natural" && Input.GetButton("Controller_Grow_Button"))
		{
			if(fullyGrown == false)
			{
				growing = true;
				withering = false;
//				doBehavior();
			}
			else
			{
				growing = false;
				withering = true;
//				doBehavior();
			}
		}
	}

    public void Grow ()
    {
        growing = true;
        withering = false;
    }
	
	public void Update()
	{
		if(growing)
		{
			transform.position = Vector3.Lerp(transform.position,endPosition,Time.deltaTime*growsmoothing);
			if (Vector3.Distance(transform.position,endPosition)<=.1)
			{
				transform.position = endPosition;
				growing = false;
			}
		}
		if(withering)
		{
			transform.position = Vector3.Lerp(transform.position,startPosition,Time.deltaTime*growsmoothing);
			if (Vector3.Distance(transform.position,startPosition)<=.1)
			{
				transform.position = startPosition;
				withering = false;
			}
		}
	}
}