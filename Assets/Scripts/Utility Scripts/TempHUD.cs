using UnityEngine;
using System.Collections;

public class TempHUD : MonoBehaviour
{
	public Texture2D positive_mode;
	public Texture2D negative_mode;
	
	public Rect tex_coords;
	
	public void Start()
	{
		tex_coords = new Rect(Screen.width - 70.0f, Screen.height - 70.0f, 64.0f, 64.0f);
	}
	
	public void OnGUI()
	{
		if (MagneticDevice.GetSelectedGunMode() == MagneticDevice.GunMode.Attract)
			GUI.DrawTexture(tex_coords,positive_mode);
		else if (MagneticDevice.GetSelectedGunMode() == MagneticDevice.GunMode.Repel)
			GUI.DrawTexture(tex_coords, negative_mode);
	}
	
}
