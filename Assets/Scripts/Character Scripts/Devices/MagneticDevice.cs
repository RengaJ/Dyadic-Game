using UnityEditor;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/2D Character/Devices/Magnetic Device")]
public class MagneticDevice : CharacterDevice
{
    public enum GunMode { Repel , Attract , Grab }
    public static GunMode currentMode = GunMode.Grab; // Grab onto an object and move gun to move object
    private static GunMode selectedMode = GunMode.Attract; // Will switch between Attract And Repel modes
	private bool hit_object = false;
    public float effectiveRange = 10.0f;
    private Vector2 previousPosition;
    private MetalBlock affectedBlock;
	private GameObject visorObject;
	private LineRenderer line;
	
	private GameObject renderedLine;
	
	private Object[] grab_textures;
	private int grab_index = 0;
	
    public Ray[] GetConeRays ()
    {
        Ray[] rays = new Ray[rayCount];
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 angledVector = new Vector3( Mathf.Cos( ( halfAngle - ( angleDivisions * i ) ) * Mathf.Deg2Rad ) ,
                                               Mathf.Sin( ( halfAngle - ( angleDivisions * i ) ) * Mathf.Deg2Rad ) );
            rays[i] = new Ray( transform.position , angledVector );
        }

        return rays;
    }

	void Start ()
    {
		if (cameraScreen == null)
			cameraScreen = GameObject.Find("Main Camera").GetComponent<Camera>();
		if (visorObject == null)
			visorObject = GameObject.Find("Visor");
		renderedLine = new GameObject("Magnetic Device Line");
		renderedLine.transform.parent = transform;
		line = renderedLine.AddComponent<LineRenderer>();
		line.SetVertexCount(2);
		line.SetPosition(0, visorObject.transform.position);
		line.SetWidth(0.0f, 1.0f);
		line.enabled = false;
		
		grab_textures = Resources.LoadAll("Magnetic",typeof(Texture2D));
	}

    public Utility.MouseDirection CheckMouseDirection ()
    {
        Utility.MouseDirection direction = Utility.MouseDirection.NONE;
        Vector2 currentPosition = Input.mousePosition;
        if (currentPosition == previousPosition)
            direction = Utility.MouseDirection.NONE;
        else
        {
            if (currentPosition.x - previousPosition.x > 0)
            {
                if (currentPosition.y - previousPosition.y > 0)
                    direction = Utility.MouseDirection.UP_RIGHT;
                else if (currentPosition.y - previousPosition.y < 0)
                    direction = Utility.MouseDirection.DOWN_RIGHT;
                else
                    direction = Utility.MouseDirection.RIGHT;
            }
            else if (currentPosition.x - previousPosition.x < 0)
            {
                if (currentPosition.y - previousPosition.y > 0)
                    direction = Utility.MouseDirection.UP_LEFT;
                else if (currentPosition.y - previousPosition.y < 0)
                    direction = Utility.MouseDirection.DOWN_LEFT;
                else
                    direction = Utility.MouseDirection.LEFT;
            }
            else
            {
                if (currentPosition.y - previousPosition.y > 0)
                    direction = Utility.MouseDirection.UP;
                else if (currentPosition.y - previousPosition.y < 0)
                    direction = Utility.MouseDirection.DOWN;
            }
        }
        previousPosition = currentPosition;

        return direction;
    }

    public override void FixedUpdate ()
    {
        if (fired)
		{
		    if (currentMode == GunMode.Grab)
            {
            	if (affectedBlock == null)
            	{
                    // Search for one....
                  //  Ray[] rays = GetConeRays();
                  //  foreach (Ray ray in rays)
                  //  {
						Ray ray = cameraScreen.ScreenPointToRay(Input.mousePosition);
                        RaycastHit information;
                        if (Physics.Raycast( ray , out information ))
                        {
                            GameObject collided = information.collider.gameObject;
                            if (MultipleTags.HasTagIn( collided , "Metal" ))
                            {
                                MetalBlock block = collided.GetComponent<MetalBlock>();
                                if (block != null)
                                {
                                    block.Unlock();
                                    affectedBlock = block;
									transform.parent.gameObject.GetComponent<CharacterMovement>().enabled = false;
									line.SetPosition(1,information.point);
									line.enabled = true;
									line.material = (Material)Resources.Load("Materials/MagneticGrabMaterial");
									line.material.mainTexture = (Texture2D)grab_textures[grab_index];
                                }
                            }
                        }
                    //}
                }
				else
	            {
					grab_index = (grab_index + 1) % 4;
	                Utility.MouseDirection direction = CheckMouseDirection();
	                affectedBlock.Move(direction);
					line.material.mainTexture = (Texture2D)grab_textures[grab_index];
	            }
            }
			else // GunMode == GunMode.Attract or GunMode.Repel
			{
				Ray ray = cameraScreen.ScreenPointToRay(Input.mousePosition);
				//Ray ray = new Ray(transform.position, mouseDirection.normalized);
				RaycastHit information;
				if (Physics.Raycast (ray, out information) && !hit_object)
				{
					GameObject collided = information.collider.gameObject;
					if (Mathf.Abs(Vector3.Distance(collided.GetComponent<MeshCollider>().ClosestPointOnBounds(visorObject.transform.position), transform.position)) <= effectiveRange)
					{
						if (collided.GetComponent<MetallicObject>() != null)
						{
							collided.GetComponent<MetallicObject>().charge(currentMode);
							hit_object = true;
						}
					}
				}
			}
        }
    }
	public override void Update ()
    {
        base.Update();

        // Primary Firing Modes
        if (Input.GetButton( "Fire1" ))
        {
            previousPosition = Input.mousePosition;
            base.fired = true;
            currentMode = GunMode.Grab;
        }
        if (Input.GetButtonUp( "Fire1" ))
        {
            base.fired = false;
            if (affectedBlock != null)
            {
                affectedBlock.Lock();
                affectedBlock = null;
				transform.parent.gameObject.GetComponent<CharacterMovement>().enabled = true;
            }
        }

        // Secondary Firing Modes
        if (Input.GetButton( "Fire2" ))
        {
            base.fired = true;
            currentMode = selectedMode;
        }
        if (Input.GetButtonUp( "Fire2" ))
        {
            base.fired = false;
			hit_object = false;
            if (affectedBlock != null)
            {
                affectedBlock.Lock();
                affectedBlock = null;
				line.enabled = false;
            }
        }

        // MouseWheel Scroll (selecting the modes for Attract and Repel)
        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            if (selectedMode == GunMode.Attract)
                selectedMode = GunMode.Repel;
            else
                selectedMode = GunMode.Attract;
        }
    }

    public static GunMode GetCurrentGunMode ()
    {
        return currentMode;
    }

    public static GunMode GetSelectedGunMode ()
    {
        return selectedMode;
    }
	
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, effectiveRange);
	}
}
