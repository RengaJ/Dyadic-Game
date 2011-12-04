using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/Game Manager")]
public class GameManager : MonoBehaviour
{
	public static int maxLevels = 2;
	private static int levelNumber = 0;
	private static GameObject currentLevel;
	private static AsyncOperation loading = null;
	
	public GameObject nyx_player;
	public GameObject helios_player;
	
	private GameObject heliosSpawn;
	private GameObject nyxSpawn;
	
	private static GameObject nyx;
	private static GameObject helios;
	
	public static bool statusChecked = true;
	
	private static bool helios_fadeout = false;
	private static bool helios_fadein = false;
	
	private static bool nyx_fadeout = false;
	private static bool nyx_fadein = false;
	
	private static bool helios_dead = false;
	private static bool nyx_dead = false;
	
	private float helios_alpha = 1.0f;
	private float nyx_alpha = 1.0f;
	
	public void Start()
	{
		maxLevels = Mathf.Clamp(maxLevels, 2, int.MaxValue);
		LoadLevel(4);
	}
	
	private static void LoadLevel(int level)
	{
		if (nyx != null)
			GameObject.DestroyObject(nyx);
		if (helios != null)
			GameObject.DestroyObject(helios);
		currentLevel = (GameObject)Instantiate(Resources.Load("Levels/Level" + levelNumber));
		currentLevel.transform.position = Vector3.zero;
		loading = Resources.UnloadUnusedAssets();
	}
	
	public static void LoadNextLevel()
	{
		levelNumber = (levelNumber + 1) % maxLevels;
		LoadLevel(levelNumber);
	}
	
	public static bool IsDone()
	{
		statusChecked = true;
		return (loading != null ? loading.isDone : false);
	}
	
	public void Update()
	{
		if (statusChecked && loading != null && loading.isDone)
		{
			statusChecked = false;
			GameObject[] spawns = (GameObject[])GameObject.FindGameObjectsWithTag("Spawn");
			foreach (GameObject spawn in spawns)
			{
				if (MultipleTags.HasTagIn(spawn,"Nyx"))
				{
					nyx = (GameObject)Instantiate(nyx_player);
					nyx.transform.position = spawn.transform.position;
					nyxSpawn = spawn;
				}
				else if (MultipleTags.HasTagIn(spawn, "Helios"))
				{
					helios = (GameObject)Instantiate(helios_player);
					helios.transform.position = spawn.transform.position;
					heliosSpawn = spawn;
				}
			}
		}
		if (helios_dead)
		{
			if (helios_fadeout)
			{
				if (helios_alpha == 0.0f)
				{
					helios_fadeout = false;
					helios_fadein = true;
					helios.transform.position = heliosSpawn.transform.position;
					helios.GetComponent<Rigidbody>().useGravity = false;
				}
				else
				{
					helios_alpha -= Time.deltaTime;
					helios_alpha = Mathf.Clamp01(helios_alpha);
					Utility.FadeChildren(helios, helios_alpha);
				}
				
			}
			if (helios_fadein)
			{
				if (helios_alpha == 1.0f)
				{
					helios_dead = false;
					helios_fadein = false;
					helios.GetComponent<CharacterMovement>().enabled = true;
					helios.GetComponent<Rigidbody>().useGravity = true;
				}
				else
				{
					helios_alpha += Time.deltaTime;
					helios_alpha = Mathf.Clamp01(helios_alpha);
					Utility.FadeChildren(helios, helios_alpha);
				}
			}
		}
		if (nyx_dead)
		{
			if (nyx_fadeout)
			{
				if (nyx_alpha == 0.0f)
				{
					nyx_fadeout = false;
					nyx_fadein = true;
					nyx.transform.position = nyxSpawn.transform.position;
					nyx.GetComponent<Rigidbody>().useGravity = false;
				}
				else
				{
					nyx_alpha -= Time.deltaTime;
					nyx_alpha = Mathf.Clamp01(nyx_alpha);
					Utility.FadeChildren(nyx, nyx_alpha);
				}
				
			}
			if (nyx_fadein)
			{
				if (nyx_alpha == 1.0f)
				{
					nyx_dead = false;
					nyx_fadein = false;
					nyx.GetComponent<CharacterMovement>().enabled = true;
					nyx.GetComponent<Rigidbody>().useGravity = true;
				}
				else
				{
					nyx_alpha += Time.deltaTime;
					nyx_alpha = Mathf.Clamp01(nyx_alpha);
					Utility.FadeChildren(nyx, nyx_alpha);
				}
			}
		}
	}
	
	public static void PlayerDied(int number)
	{
		number %= 2; // just to be on the safe side....
		if (number == 0) // HELIOS
		{
			helios_dead = true;
			helios_fadeout =  true;
			helios.GetComponent<CharacterMovement>().enabled = false;
		}
		else // Well, there's only one other option...
		{
			nyx_dead = true;
			nyx_fadeout = true;
			nyx.GetComponent<CharacterMovement>().enabled = false;
		}
	}
}