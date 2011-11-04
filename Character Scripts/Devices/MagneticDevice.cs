using UnityEngine;
using System.Collections;

[AddComponentMenu("Dyadic/2D Character/Devices/Magnetic Device")]
public class MagneticDevice : CharacterDevice
{
    public enum GunMode { Repel , Attract , Grab }
    public GunMode currentMode = GunMode.Grab; // Grab onto an object and move gun to move object
    private GunMode selectedMode = GunMode.Attract; // Will switch between Attract And Repel modes

    public float effectiveRange = 10.0f;

    private Vector2 previousPosition;

    private MetalBlock affectedBlock;

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
            if (affectedBlock == null)
            {
                if (currentMode == GunMode.Grab)
                {
                    // Search for one....
                    Ray[] rays = GetConeRays();
                    foreach (Ray ray in rays)
                    {
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
                                }
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                Utility.MouseDirection direction = CheckMouseDirection();
                affectedBlock.Move(direction);
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
            if (affectedBlock != null)
            {
                affectedBlock.Lock();
                affectedBlock = null;
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

    public GunMode GetCurrentGunMode ()
    {
        return currentMode;
    }

    public GunMode GetSelectedGunMode ()
    {
        return selectedMode;
    }
}
