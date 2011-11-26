using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Dyadic/Environmental/Metallic Object")]
public class MetallicObject : MonoBehaviour
{
	public enum ChargeMode { ATTRACT_ONLY, REPEL_ONLY, ATTRACT_AND_REPEL };
	public enum Direction { VERTICAL, HORIZONTAL, BOTH };
	
	public ChargeMode acceptableMode = ChargeMode.ATTRACT_ONLY;
	public Direction direction = Direction.VERTICAL;
	public bool moves = true;
	public bool springsBack = true;
	
	public MetallicObject[] other_objects;
	
	public GameObject positive_particles;
	public GameObject negative_particles;
	
	private GameObject particles;
	private bool charged = false;
	
	public float move_distance_limit = -1.0f;
	public float[] moveDistanceLimits;
	private Vector3 original_position;
	public float move_speed = 1.0f;
	
	private int charge_type;
	
	public AudioClip sound;
	private AudioSource source;
	
	// Use this for initialization
	void Start()
	{
		if (other_objects.Length == 0)
			throw new UnityException("MetallicObject Exception: At least one object reference is required in other_objects");
		if (moveDistanceLimits.Length != other_objects.Length)
			throw new UnityException("MetallicObject Exception: The number of move limits must be equal to the number of objects in other_objects");
		original_position = gameObject.transform.position;
		charge_type = 0;
		if (sound == null)
			sound = (AudioClip)Resources.LoadAssetAtPath("Assets/Media/PolarityChange",typeof(AudioClip));
		GameObject audioSource = new GameObject("Audio Source");
		audioSource.transform.parent = GameObject.Find("Main Camera").transform;
		audioSource.transform.localPosition = Vector3.zero;
		
		source = audioSource.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.DrawRay(transform.position,Vector3.up*10,Color.green);
		Debug.DrawRay(transform.position,Vector3.right*10,Color.red);
		Debug.DrawRay(transform.position,Vector3.forward*10,Color.blue);
		if (moves)
		{
			int[] indices = GetCharged();
			if (charged && indices.Length > 0)
			{
				foreach (int index in indices)
				{
					if (sameChargeType(other_objects[index]))
					{
						if (moveDistanceLimits[index] >= 0 && Mathf.Abs(Vector3.Distance(original_position, transform.position)) < moveDistanceLimits[index])
						{
							Vector3 directionV = (other_objects[index].transform.position - transform.position).normalized;
							if (this.direction != Direction.BOTH)
							{
								if (this.direction == Direction.VERTICAL) Utility.Mul(directionV, Vector3.up, out directionV);
								if (this.direction == Direction.HORIZONTAL) Utility.Mul(directionV, Vector3.right, out directionV);
							}
							if (charge_type == 1)
								transform.position = Vector3.Lerp(transform.position, transform.position + directionV, Time.deltaTime * move_speed);
							else if (charge_type == -1)
								transform.position = Vector3.Lerp(transform.position, transform.position - directionV, Time.deltaTime * move_speed);
						}
					}
				}
			}
			if (springsBack && (!charged || GetCharged().Length == 0))
			{
				if (transform.position != original_position)
					transform.position = Vector3.Lerp(transform.position, original_position, Time.deltaTime * move_speed);
				else
					charge_type = 0;
			}
		}
	}
	
	public bool isCharged()
	{
		return charged;
	}
	
	public void charge(MagneticDevice.GunMode magnetic_mode)
	{
		if (magnetic_mode == MagneticDevice.GunMode.Attract)
		{
			if (!charged)
			{
				if (acceptableMode == ChargeMode.ATTRACT_ONLY || acceptableMode == ChargeMode.ATTRACT_AND_REPEL)
				{
					charged = true;
					charge_type = 1;
					particles = (GameObject)Instantiate(positive_particles);
					particles.transform.parent = gameObject.transform;
					particles.transform.localPosition = Vector3.zero;
					source.PlayOneShot(sound);
				}
			}
			else
			{
				charged = false;
				particles.GetComponent<ParticleEmitter>().emit = false; // Autodestruct should automatically kill the particle system
				particles = null;
			}
		}
		if (magnetic_mode == MagneticDevice.GunMode.Repel)
		{
			if (!charged)
			{
				if (acceptableMode == ChargeMode.REPEL_ONLY || acceptableMode == ChargeMode.ATTRACT_AND_REPEL)
				{
					charged = true;
					charge_type = -1;
					particles = (GameObject)Instantiate(negative_particles);
					particles.transform.parent = gameObject.transform;
					particles.transform.localPosition = Vector3.zero;
					source.PlayOneShot(sound);
				}
			}
			else
			{
				charged = false;
				particles.GetComponent<ParticleEmitter>().emit = false;
				particles = null;
			}
		}
		if (magnetic_mode == MagneticDevice.GunMode.Grab)
		{
			charged = false;
			if (particles != null)
			{
				particles.GetComponent<ParticleEmitter>().emit = false;
				particles = null;
			}
		}
	}
	
	private int get_charge_type()
	{
		return charge_type;
	}
	
	private int[] GetCharged()
	{
		List<int> indices = new List<int>();
		for (int i = 0; i < other_objects.Length; i++)
			if (other_objects[i].isCharged())
				indices.Add(i);
		return indices.ToArray();
	}
	
	private bool sameChargeType(MetallicObject ood)
	{
		return charge_type == ood.get_charge_type();
	}
}