using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/Game Manager")]
public class GameManager : MonoBehaviour
{
	public static int maxLevels = 6;
	private static int levelNumber = -1;
	private static GameObject currentLevel;
	private static AsyncOperation loading = null;
	
	public GameObject nyx_player;
	public GameObject helios_player;
	
	private bool waitForInput = false;
	
	public Texture2D fadeTexture;
	public Texture2D fogTexture;
	
	private bool pressedA = false;
	private bool pressedSpace = false;
	
	private GameObject heliosSpawn;
	private GameObject nyxSpawn;
	
	private static GameObject nyx;
	private static GameObject helios;
	
	private bool readyToLoad = false;
	public static bool statusChecked = true;
	
	private static bool helios_fadeout = false;
	private static bool helios_fadein = false;
	
	private static bool nyx_fadeout = false;
	private static bool nyx_fadein = false;
	
	private static bool helios_dead = false;
	private static bool nyx_dead = false;
	
	private float helios_alpha = 1.0f;
	private float nyx_alpha = 1.0f;
	
	private float alpha = 0.0f;
	
	private bool fadingIn = false;
	private bool fadingOut = false;

    private Rect notify_A = new Rect( Screen.width/5.4f , Screen.height/1.3f , Screen.width/5.12f , Screen.height/24.0f );
    private Rect notifySpace = new Rect( Screen.width/1.7f , Screen.height/1.3f , Screen.width/4.5f , Screen.height/24.0f );

	private static JournalEntryList journalEntries;
	
	public void Start()
	{
		maxLevels = Mathf.Clamp(maxLevels, 6, int.MaxValue);
		//LoadLevel(0);
		journalEntries = GetComponent<JournalEntryList>();
		if (journalEntries == null)
			throw new UnityException("Game Manager :: Dyadic Exception - The journal entries must exist!");
	}
	
	private static void LoadLevel(int level)
	{
        if (nyx != null)
        {
            GameObject.DestroyObject( nyx );
            nyx = null;
        }
        if (helios != null)
        {
            GameObject.DestroyObject( helios );
            helios = null;
        }
        GameObject.Destroy( currentLevel ); // We do not want this level anymore, so let's remove it from memory.
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
	
	public void OnGUI()
	{
		if (fadingIn)
		{
			if (alpha < 1.0f)
				alpha = Mathf.Clamp01(alpha + (Time.deltaTime * 0.5f));
			else
			{
				fadingIn = false;
				readyToLoad = true;
                waitForInput = true;
                ShowJournal();
			}
		}
		if (fadingOut)
		{
            if (alpha > 0.0f)
                alpha = Mathf.Clamp01( alpha - ( Time.deltaTime * 0.5f ) );
            else // RGB value --> Light Green :  ( 0, 153, 18 )
            {
                fadingOut = false;
                pressedA = false;
                pressedSpace = false;
            }
		}

        Color tempColor = GUI.color;
        GUI.color = new Color( 0 , 0 , 0 , alpha );
        GUI.DrawTexture( new Rect( 0 , 0 , Screen.width , Screen.height ) , fadeTexture );
        GUI.color = tempColor;

        journalEntries.Draw();

        if (waitForInput)
        {
            if (!pressedA)
                GUI.Box( notify_A , "Press the (A) button to continue." );
            else
                GUI.Box( notify_A , "Waiting for Helios..." );

            if (!pressedSpace)
                GUI.Box( notifySpace , "Press the [Space] button to continue." );
            else
                GUI.Box( notifySpace , "Waiting for Nyx..." );

        }

        GUI.color = new Color( 1 , 1 , 1 , 0.25f );
		GUI.DrawTexture(new Rect(0,10.0f + (10.0f * Mathf.Sin(Time.time)), Screen.width, Screen.height), fogTexture);
	}
	
	public void Update()
	{
        if (Input.GetKeyDown( KeyCode.Return ))
            fadingIn = true;
		if (readyToLoad)
		{
			readyToLoad = false;
            statusChecked = true;
			LoadNextLevel();
		}
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
		if (waitForInput)
		{
            if (!pressedA || !pressedSpace)
            {
                if (!pressedA)
                    //pressedA = Input.GetButtonDown( "Controller_Jump" );
                    pressedA = Input.GetKeyDown( KeyCode.A );
                if (!pressedSpace)
                    pressedSpace = Input.GetKeyDown( KeyCode.Space );
            }
            else
            {
                waitForInput = false;
                NextJournal(); // doesn't show the next journal, but just prepares to do so.
                fadingOut = true;
            }
			return;
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
	
	public static void ShowJournal()
	{
		journalEntries.ShowJournal();
	}
	
	public static void NextJournal()
	{
		journalEntries.HideJournal();
		journalEntries.NextEntry(); // prepare for the next level!
	}
}