using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JournalEntryList : MonoBehaviour
{
	public Texture2D journalTexture;
	private Rect Nyx_rect    = new Rect(Screen.width/8.0f,Screen.height/9.60f, Screen.width/3.0f, Screen.height/1.32f);
	private Rect Helios_rect = new Rect(Screen.width/2.0f,Screen.height/9.60f, Screen.width/3.0f, Screen.height/1.32f);

    public Rect helios_rect
    {
        get { return Helios_rect; }
        set { }
    }

    public Rect nyx_rect
    {
        get { return Nyx_rect; }
        set { }
    }

	private bool show = false;

	public List<JournalEntry> list = new List<JournalEntry>();
	private int currentEntry = 0;
	
	public void AddNewEntry()
	{
		list.Add(new JournalEntry());
	}
	
	public void RemoveLastEntry()
	{
		if (list.Count > 0)
			list.RemoveAt(list.Count-1);
	}
	
	public void ShowJournal()
	{
		show = true;
	}
	
	public void HideJournal()
	{
		show = false;
	}
	
	public void NextEntry()
	{
		currentEntry = (currentEntry + 1) % list.Count;
	}
	
	public void Draw()
	{
		if (show)
		{
			GUI.skin.box.alignment = TextAnchor.MiddleLeft;
			GUI.skin.box.onActive.background = null;
			if (journalTexture != null)
				GUI.DrawTexture(new Rect(-1,-1,Screen.width+1,Screen.height+1),journalTexture);
			if (list.Count > 0 && currentEntry < list.Count)
			{
				GUI.Box(nyx_rect, list[currentEntry].nyx_entry);
				GUI.Box(helios_rect, list[currentEntry].helios_entry);
			}
		}
	}
}