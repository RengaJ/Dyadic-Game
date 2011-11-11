using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/Environmental/Level End Volume")]
public class LevelEnd : MonoBehaviour
{
	private int playerCount = 0;
	void OnTriggerEnter(Collider other)
	{
		if (MultipleTags.HasTagIn(other.gameObject,"Player"))
			playerCount++;
	}
	void OnTriggerExit(Collider other)
	{
		if (MultipleTags.HasTagIn(other.gameObject,"Player"))
			playerCount--;
	}
	
	void LevelEnded()
	{
		GameObject the_camera = GameObject.Find("Main Camera");
		the_camera.GetComponent<Fade>().Begin();
	}
}
