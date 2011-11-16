using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/2D Character/Physical Interaction")]
public class CharacterInteraction : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
	}
	// Update is called once per frame
	void Update ()
    {
		if (name.Contains("Nyx") && Input.GetButtonDown("Controller_Activate_Cursor"))
			GetComponent<CharacterMovement>().enabled = !(GetComponent<CharacterMovement>().enabled);
	}

    public void UpdateStatus ( CharacterCollisionData data )
    {
        Debug.Log( data.side + "      " + data.status );
    }

    public void Die ()
    {
        if (name.Contains("Nyx"))
			GameManager.PlayerDied(1);
		else if (name.Contains("Helios"))
			GameManager.PlayerDied(0);
    }
}
