using System;
using UnityEngine;


public class SwingingDoorScript : MonoBehaviour
{

	public bool heardDoor;

	public GameControllerScript gc;

	public BaldiScript baldi;

	public MeshCollider barrier;

	public GameObject obstacle;

	public MeshCollider trigger;

	public MeshRenderer inside;

	public MeshRenderer outside;

	public Material closed;

	public Material open;

	public Material locked;

	public AudioClip doorOpen;

	public AudioClip baldiDoor;

	[SerializeField]
	private float openTime;

	private float lockTime;

	public bool bDoorOpen;

	public bool bDoorLocked;

	private bool requirementMet;

	private AudioSource myAudio;

    public SoundManager soundManager;

    public SoundObject doorOpenObject;

	private void Start()
	{
		this.myAudio = base.GetComponent<AudioSource>();
        this.UnlockDoor();
        this.requirementMet = true;
		soundManager = GetComponent<SoundManager>();
    }

	private void Update()
	{
		if (this.openTime > 0f)
		{
			this.openTime -= 1f * Time.deltaTime;
		}
		if (this.lockTime > 0f)
		{
			this.lockTime -= Time.deltaTime;
		}
		else if (this.bDoorLocked)
		{
			this.UnlockDoor();
		}
		if (this.openTime <= 0f & this.bDoorOpen & !this.bDoorLocked)
		{
			this.heardDoor = false;
			this.bDoorOpen = false;
			this.inside.material = this.closed;
			this.outside.material = this.closed;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if ((other.tag == "Player" || (other.tag == "NPC" && other.isTrigger)) && !this.bDoorLocked)
		{
			if (other.tag == "Player")
			{
				this.heardDoor = true;
				this.bDoorOpen = true;
				this.inside.material = this.open;
				this.outside.material = this.open;
				this.openTime = 2f;
				return;
			}
			if (other.isTrigger)
			{
				this.heardDoor = true;
				this.bDoorOpen = true;
				this.inside.material = this.open;
				this.outside.material = this.open;
				this.openTime = 2f;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.tag == "Player") && !this.heardDoor)
		{
			if (!this.bDoorLocked)
			{
				soundManager.PlaySound(doorOpenObject);
				if (other.tag == "Player" && this.baldi.isActiveAndEnabled)
				{
					this.baldi.Hear(base.transform.position, 1f);
				}     
			}
		}
	}

	public void LockDoor(float time)
	{
		this.barrier.enabled = true;
		this.obstacle.SetActive(true);
		this.bDoorLocked = true;
		this.lockTime = time;
		this.inside.material = this.locked;
		this.outside.material = this.locked;
	}

	private void UnlockDoor()
	{
		this.barrier.enabled = false;
		this.obstacle.SetActive(false);
		this.bDoorLocked = false;
		this.inside.material = this.closed;
		this.outside.material = this.closed;
	}
}