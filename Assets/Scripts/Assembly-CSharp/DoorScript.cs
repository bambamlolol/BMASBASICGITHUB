using UnityEngine;

public class DoorScript : MonoBehaviour
{
	public float openingDistance;

	public Transform player;

	public BaldiScript baldi;

	public MeshCollider barrier;

	public MeshCollider trigger;

	public MeshCollider invisibleBarrier;

	public MeshRenderer inside;

	public MeshRenderer outside;

	public AudioClip doorOpen;

	public AudioClip doorClose;

	public Material closed;

	public Material open;

	private bool bDoorOpen;

	private bool bDoorLocked;

	public int silentOpens;

	private float openTime;

	public float lockTime;

	private AudioSource myAudio;

    public SoundManager soundManager;

    public SoundObject doorOpenObject;

    public SoundObject doorCloseObject;

	private void Start()
	{
		myAudio = GetComponent<AudioSource>();
		soundManager = GetComponent<SoundManager>();
	}

	private void Update()
	{
		if (lockTime > 0f)
		{
			lockTime -= 1f * Time.deltaTime;
		}
		else if (bDoorLocked)
		{
			UnlockDoor();
		}
		if (openTime > 0f)
		{
			openTime -= 1f * Time.deltaTime;
		}
		if ((openTime <= 0f) & bDoorOpen)
		{
			barrier.enabled = true;
			invisibleBarrier.enabled = true;
			bDoorOpen = false;
			inside.sharedMaterial = closed;
			outside.sharedMaterial = closed;
			if (silentOpens <= 0)
			{
				soundManager.PlaySound(doorCloseObject);
			}
		}
		if (!(Input.GetMouseButtonDown(0) & (Time.timeScale != 0f)))
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo) && ((hitInfo.collider == trigger) & (Vector3.Distance(player.position, base.transform.position) < openingDistance) & !bDoorLocked))
		{
			if (baldi.isActiveAndEnabled & (silentOpens <= 0))
			{
				baldi.Hear(base.transform.position, 1f);
			}
			OpenDoor();
			if (silentOpens > 0)
			{
				silentOpens--;
			}
		}
	}

	public void OpenDoor()
	{
		if (silentOpens <= 0 && !bDoorOpen)
		{
			soundManager.PlaySound(doorOpenObject);
		}
		barrier.enabled = false;
		invisibleBarrier.enabled = false;
		bDoorOpen = true;
		inside.sharedMaterial = open;
		outside.sharedMaterial = open;
		openTime = 3f;
	}

	private void OnTriggerStay(Collider other)
	{
		if (!bDoorLocked & other.CompareTag("NPC"))
		{
			OpenDoor();
		}
	}

	public void LockDoor(float time)
	{
		bDoorLocked = true;
		lockTime = time;
	}

	public void UnlockDoor()
	{
		bDoorLocked = false;
	}

	public void SilenceDoor()
	{
		silentOpens = 4;
	}
}
