using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Dyadic/Environmental/Level End Volume")]
public class LevelEnd : MonoBehaviour
{
	public GameObject nyxDoor;
	public GameObject heliosDoor;
	private int playerCount = 0;
	
	private List<GameObject> players;
	
	void Start()
	{
		players = new List<GameObject>();
	}
	void OnTriggerEnter(Collider other)
	{
		if (MultipleTags.HasTagIn(other.gameObject,"Player"))
			playerCount++;
		if (!players.Contains(other.gameObject))
			players.Add(other.gameObject);
		
		if (players.Count == 2)
			LevelEnded();
	}
	void OnTriggerExit(Collider other)
	{
		if (MultipleTags.HasTagIn(other.gameObject,"Player"))
			playerCount--;
		if (players.Contains(other.gameObject))
			players.Remove(other.gameObject);
	}
	
	void LevelEnded()
	{
		GameObject the_camera = GameObject.Find("Main Camera");
		the_camera.GetComponent<Fade>().Begin();
		
		foreach (GameObject player in players)
		{
			if (MultipleTags.HasTagIn(player,"Helios"))
				player.GetComponent<CharacterMovement>().ExitAnimation(heliosDoor);
			if (MultipleTags.HasTagIn(player,"Nyx"))
				player.GetComponent<CharacterMovement>().ExitAnimation(nyxDoor);
		}
	}
}
