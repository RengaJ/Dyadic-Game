using UnityEngine;
using System.Collections;

public class EarthCollision : MonoBehaviour {
	public GameObject main;
	private EarthMovement em;
	
	// Use this for initialization
	void Start () {
		em = main.GetComponent<EarthMovement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider co){
		Debug.Log("ENTER:"+co.gameObject.name);
		EarthMover mv = co.gameObject.GetComponent<EarthMover>();
		if(mv != null)
		{
			mv.SetEarth(em);
		}
	}
	
	void OnTriggerExit(Collider co){
		EarthMover mv = co.gameObject.GetComponent<EarthMover>();
		if(mv != null)
		{
			mv.SetEarth(em);
		}
	}
}
